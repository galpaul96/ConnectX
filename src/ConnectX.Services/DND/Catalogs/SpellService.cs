using System.Text.Json;
using ConnectX.Domain.Entities.DND.Catalogs.Spells;
using ConnectX.Domain.Services.DND.Catalogs.Spells;
using ConnectX.Ef;
using Microsoft.EntityFrameworkCore;

namespace ConnectX.Services.DND.Catalogs;

public sealed class SpellService(IRepository repository) : ISpellService
{
    // Interactive-server events can overlap; the scoped EF context cannot.
    private readonly SemaphoreSlim operations = new(1, 1);

    public async Task<SpellPageDto> GetPageAsync(int pageNumber = 1, int pageSize = 24,
        string? search = null, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        if (pageSize is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100.");
        await operations.WaitAsync(cancellationToken);
        try
        {
            var query = repository.GetAllAsync<Spell>().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLowerInvariant();
                query = query.Where(x => x.Name.ToLower().Contains(term));
            }
            var count = await query.CountAsync(cancellationToken);
            var pages = Math.Max(1, (int)Math.Ceiling(count / (double)pageSize));
            var page = Math.Min(pageNumber, pages);
            var rows = await query.OrderBy(x => x.Name).ThenBy(x => x.Id)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new
                {
                    x.Id, x.Name,
                    CardComponents = x.Components
                        .Where(c => c.Type == "level" || c.Type == "school" || c.Type == "imageUrl")
                        .Select(c => new { c.Type, c.Value }).ToList()
                }).ToListAsync(cancellationToken);
            var items = rows.Select(row =>
            {
                string? Text(string type, string? property = null)
                {
                    var value = row.CardComponents.FirstOrDefault(c => c.Type == type)?.Value ?? default;
                    if (property is not null)
                        value = value.ValueKind == JsonValueKind.Object && value.TryGetProperty(property, out var field) ? field : default;
                    return value.ValueKind == JsonValueKind.String ? value.GetString() : null;
                }
                return new SpellPageEntry(row.Id, new SpellSummaryDto(row.Name, Text("imageUrl"),
                    Text("level", "label") ?? Text("level", "levelNumber"), Text("school")));
            }).ToList();
            return new(items, count, page, pageSize);
        }
        finally { operations.Release(); }
    }

    public async Task<SpellDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await operations.WaitAsync(cancellationToken);
        try
        {
            return await repository.GetAllAsync<Spell>().AsNoTracking().Where(x => x.Id == id)
                .Select(x => new SpellDto
                {
                    Id = x.Id, SourceId = x.SourceId, Slug = x.Slug,
                    Name = x.Name, StorageFile = x.StorageFile,
                    Components = x.Components.OrderBy(c => c.Position).ThenBy(c => c.Id)
                        .Select(c => new SpellComponentDto
                        {
                            Type = c.Type, Label = c.Label, Position = c.Position,
                            Value = c.Value, Html = c.Html
                        }).ToList()
                }).SingleOrDefaultAsync(cancellationToken);
        }
        finally { operations.Release(); }
    }

    public Task<Guid> CreateAsync(SpellWriteDto input, CancellationToken cancellationToken = default) => AddAsync(input, cancellationToken);

    public async Task<Guid> AddAsync(SpellWriteDto input, CancellationToken cancellationToken = default)
    {
        SpellWriteValidation.Validate(input);
        await operations.WaitAsync(cancellationToken);
        try
        {
            if (await repository.GetAllAsync<Spell>().IgnoreQueryFilters()
                .AnyAsync(x => x.SourceId == input.SourceId || x.Slug == input.Slug, cancellationToken))
                throw new SpellAlreadyExistsException();
            var entity = new Spell();
            Apply(entity, input);
            try { await repository.AddAsync(entity); }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException { SqlState: "23505" })
            { throw new SpellAlreadyExistsException(); }
            return entity.Id;
        }
        finally { operations.Release(); }
    }

    public async Task<bool> UpdateAsync(Guid id, SpellWriteDto input, CancellationToken cancellationToken = default)
    {
        SpellWriteValidation.Validate(input);
        await operations.WaitAsync(cancellationToken);
        try
        {
            // Include retired roles so reintroducing one reuses its unique row.
            var entity = await repository.GetAllAsync<Spell>().IgnoreQueryFilters()
                .Where(x => x.Id == id && !x.IsDeleted).Include(x => x.Components)
                .SingleOrDefaultAsync(cancellationToken);
            if (entity is null) return false;
            Apply(entity, input);
            await repository.UpdateAsync(entity);
            return true;
        }
        finally { operations.Release(); }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await operations.WaitAsync(cancellationToken);
        try
        {
            if (!await repository.GetAllAsync<Spell>().AnyAsync(x => x.Id == id, cancellationToken)) return false;
            await repository.DeleteAsync<Spell>(id);
            return true;
        }
        finally { operations.Release(); }
    }

    private static void Apply(Spell entity, SpellWriteDto input)
    {
        entity.SourceId = input.SourceId;
        entity.Slug = input.Slug;
        entity.Name = input.Name;
        entity.StorageFile = input.StorageFile;
        var existing = entity.Components.ToDictionary(x => x.Type, StringComparer.Ordinal);
        var now = DateTime.UtcNow;
        foreach (var old in existing.Values)
        {
            old.IsDeleted = true;
            old.DeletedDate = now;
            old.UpdatedDate = now;
        }
        for (var position = 0; position < input.Components.Count; position++)
        {
            var item = input.Components[position];
            if (!existing.TryGetValue(item.Type, out var component))
            {
                component = new SpellComponent { Type = item.Type, CreatedDate = now };
                entity.Components.Add(component);
            }
            component.Label = item.Label;
            component.Position = position;
            component.Value = item.Value.Clone();
            component.Html = item.Html;
            component.IsDeleted = false;
            component.DeletedDate = null;
            component.UpdatedDate = now;
        }
    }

}
