using System.Text.Json;
using System.Text.Json.Nodes;
using ConnectX.Domain.Entities.DND.Catalogs.Spells;
using ConnectX.Domain.Services.DND.Catalogs.Spells;
using ConnectX.Ef;
using ConnectX.Services.DND.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using Xunit;

namespace ConnectX.Spells.Tests;

public class SpellFlowTests
{
    [Fact]
    public void Relational_model_has_only_parent_and_component_tables_with_explicit_json_storage()
    {
        using var context = new EfContext(new DbContextOptionsBuilder<EfContext>()
            .UseNpgsql("Host=localhost;Database=model_only;Username=unused").Options);
        var model = context.Model;
        Assert.Equal(new[] { "ItemComponents", "Items", "SpellComponents", "Spells" }, model.GetEntityTypes().Select(x => x.GetTableName()).Order().ToArray());
        Assert.DoesNotContain(model.GetEntityTypes(), x => x.IsOwned());
        var component = model.FindEntityType(typeof(SpellComponent))!;
        Assert.Equal("jsonb", component.FindProperty(nameof(SpellComponent.Value))!.GetColumnType());
        Assert.Equal(typeof(JsonElement), component.FindProperty(nameof(SpellComponent.Value))!.ClrType);
        Assert.Equal("text", component.FindProperty(nameof(SpellComponent.Html))!.GetColumnType());
        Assert.Single(component.GetNavigations());
    }

    [Fact]
    public async Task All_470_records_round_trip_with_ordered_lossless_components_and_compact_list_queries()
    {
        await using var database = await TestDatabase.CreateAsync();
        var service = database.Service;
        var sources = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "spell-catalog"), "*.json")
            .Where(x => !Path.GetFileName(x).StartsWith('_')).Order().ToList();
        Assert.Equal(470, sources.Count);
        var expected = new List<(Guid Id, SpellWriteDto Input)>();
        foreach (var source in sources)
        {
            var input = ReadSpell(source);
            expected.Add((await service.CreateAsync(input, cancellationToken: TestContext.Current.CancellationToken), input));
        }
        database.Context.ChangeTracker.Clear();
        database.Commands.Clear();
        var first = await service.GetPageAsync(1, 24, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(470, first.TotalCount);
        Assert.Equal(20, first.TotalPages);
        Assert.Equal(24, first.Items.Count);
        Assert.Equal(4, typeof(SpellSummaryDto).GetProperties().Length);
        Assert.Empty(database.Context.ChangeTracker.Entries());
        if (database.IsPostgres)
        {
            Assert.Contains(database.Commands, sql => sql.Contains("SpellComponents", StringComparison.Ordinal));
            Assert.DoesNotContain(database.Commands, sql => sql.Contains("\"Html\"", StringComparison.Ordinal));
            Assert.DoesNotContain(database.Commands, sql => sql.Contains("\"StorageFile\"", StringComparison.Ordinal));
            Assert.Contains(database.Commands, sql => sql.Contains("LIMIT", StringComparison.Ordinal));
            Assert.Contains(database.Commands, sql => sql.Contains("SELECT s.\"Id\", s.\"Name\"", StringComparison.Ordinal));
        }
        foreach (var entry in first.Items)
        {
            var input = expected.Single(x => x.Id == entry.Id).Input;
            var level = input.Components.First(x => x.Type == "level").Value.GetProperty("label").GetString();
            Assert.Equal(level, entry.Spell.Level);
            Assert.Equal(input.Components.First(x => x.Type == "school").Value.GetString(), entry.Spell.School);
            Assert.Equal(input.Components.First(x => x.Type == "imageUrl").Value.GetString(), entry.Spell.ImageUrl);
        }
        var last = await service.GetPageAsync(int.MaxValue, 24, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(20, last.PageNumber);
        Assert.Equal(14, last.Items.Count);
        Assert.Empty(first.Items.Select(x => x.Id).Intersect(last.Items.Select(x => x.Id)));
        Assert.Contains((await service.GetPageAsync(search: " FIREBALL ", cancellationToken: TestContext.Current.CancellationToken)).Items, x => x.Spell.Name == "Fireball");
        Assert.Empty((await service.GetPageAsync(search: "no-such-spell-7f6e2", cancellationToken: TestContext.Current.CancellationToken)).Items);
        foreach (var (id, input) in expected)
        {
            var detail = await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(detail);
            Assert.Equal(input.Name, detail.Name);
            Assert.Equal(input.SourceId, detail.SourceId);
            Assert.Equal(input.Slug, detail.Slug);
            Assert.Equal(input.StorageFile, detail.StorageFile);
            Assert.Equal(input.Components.Count, detail.Components.Count);
            for (var i = 0; i < input.Components.Count; i++)
            {
                var wanted = input.Components[i];
                var actual = detail.Components[i];
                Assert.Equal(i, actual.Position);
                Assert.Equal(wanted.Type, actual.Type);
                Assert.Equal(wanted.Label, actual.Label);
                Assert.Equal(wanted.Html, actual.Html);
                Assert.True(JsonNode.DeepEquals(JsonNode.Parse(wanted.Value.GetRawText()), JsonNode.Parse(actual.Value.GetRawText())), $"Changed {input.Slug}/{wanted.Type}");
            }
        }
    }

    [Fact]
    public async Task Crud_reconciles_removed_and_reintroduced_roles_and_preserves_all_json_kinds()
    {
        await using var database = await TestDatabase.CreateAsync();
        var service = database.Service;
        var values = new[] { "\"plain string\"", "123.5", "true", "false", "null", "[0,false,null,{\"nested\":[1,2]}]", "{\"material\":{\"cost\":0,\"consumed\":false}}" };
        var input = new SpellWriteDto
        {
            SourceId = "test-source", Slug = "test-spell", Name = "Test 🪶", StorageFile = "test-spell.json",
            Components = values.Select((v, i) => new SpellComponentDto
            {
                Type = $"extension-{i}", Label = $"Extension {i}", Value = JsonDocument.Parse(v).RootElement.Clone(), Html = new string('x', 7000)
            }).ToList()
        };
        var id = await service.CreateAsync(input, cancellationToken: TestContext.Current.CancellationToken);
        database.Context.ChangeTracker.Clear();
        var originalRows = await database.Context.Set<SpellComponent>().Where(x => x.SpellId == id).ToDictionaryAsync(x => x.Type, x => x.Id, cancellationToken: TestContext.Current.CancellationToken);
        var reduced = input with { Name = "Renamed", Components = input.Components.Take(1).ToList() };
        Assert.True(await service.UpdateAsync(id, reduced, cancellationToken: TestContext.Current.CancellationToken));
        database.Context.ChangeTracker.Clear();
        Assert.Single((await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken))!.Components);
        Assert.True(await service.UpdateAsync(id, input, cancellationToken: TestContext.Current.CancellationToken));
        database.Context.ChangeTracker.Clear();
        var restored = (await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken))!;
        Assert.Equal(values.Length, restored.Components.Count);
        Assert.Equal(input.Name, restored.Name);
        for (var i = 0; i < values.Length; i++)
            Assert.True(JsonNode.DeepEquals(JsonNode.Parse(values[i]), JsonNode.Parse(restored.Components[i].Value.GetRawText())));
        var restoredRows = await database.Context.Set<SpellComponent>().Where(x => x.SpellId == id).ToDictionaryAsync(x => x.Type, x => x.Id, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(originalRows.OrderBy(x => x.Key), restoredRows.OrderBy(x => x.Key));
        Assert.True(await service.DeleteAsync(id, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Null(await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(0, (await service.GetPageAsync(cancellationToken: TestContext.Current.CancellationToken)).TotalCount);
        Assert.False(await service.DeleteAsync(id, cancellationToken: TestContext.Current.CancellationToken));
        Assert.False(await service.UpdateAsync(id, input, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Null(await service.GetByIdAsync(Guid.NewGuid(), cancellationToken: TestContext.Current.CancellationToken));
        Assert.False(await service.DeleteAsync(Guid.NewGuid(), cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Rejects_invalid_writes_and_pages_and_observes_cancellation()
    {
        await using var database = await TestDatabase.CreateAsync();
        var service = database.Service;
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetPageAsync(0, cancellationToken: TestContext.Current.CancellationToken));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetPageAsync(pageSize: 101, cancellationToken: TestContext.Current.CancellationToken));
        using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.GetPageAsync(cancellationToken: cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.GetByIdAsync(Guid.NewGuid(), cancelled.Token));
        var input = ReadSpell(Path.Combine(AppContext.BaseDirectory, "spell-catalog", "fireball.json"));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(input with { Components = [input.Components[0], input.Components[0]] }, cancellationToken: TestContext.Current.CancellationToken));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(input with { Components = [new() { Type = "invalid", Label = "Invalid" }] }, cancellationToken: TestContext.Current.CancellationToken));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(input with { Name = " " }, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(0, (await service.GetPageAsync(cancellationToken: TestContext.Current.CancellationToken)).TotalCount);
    }

    internal static SpellWriteDto ReadSpell(string filename)
    {
        using var json = JsonDocument.Parse(File.ReadAllText(filename));
        var root = json.RootElement;
        return new()
        {
            SourceId = root.GetProperty("id").GetString()!, Slug = root.GetProperty("slug").GetString()!,
            Name = root.GetProperty("name").GetString()!, StorageFile = root.GetProperty("storageFile").GetString()!,
            Components = root.GetProperty("components").EnumerateArray().Select((c, i) => new SpellComponentDto
            {
                Type = c.GetProperty("type").GetString()!, Label = c.GetProperty("label").GetString()!,
                Value = c.GetProperty("value").Clone(), Html = c.GetProperty("html").GetString()!, Position = i
            }).ToList()
        };
    }
}

internal sealed class TestDatabase : IAsyncDisposable
{
    public EfContext Context { get; }
    public SpellService Service { get; }
    public bool IsPostgres { get; }
    public List<string> Commands { get; } = [];
    private TestDatabase()
    {
        var connection = Environment.GetEnvironmentVariable("CONNECTX_SPELL_TEST_POSTGRES");
        var options = new DbContextOptionsBuilder<EfContext>();
        IsPostgres = !string.IsNullOrEmpty(connection);
        if (IsPostgres)
        {
            // Always create a unique disposable database. Never migrate/drop the supplied database.
            var builder = new NpgsqlConnectionStringBuilder(connection) { Database = $"connectx_spell_test_{Guid.NewGuid():N}" };
            options.UseNpgsql(builder.ConnectionString).AddInterceptors(new CaptureCommands(Commands));
        }
        else options.UseInMemoryDatabase($"spell-test-{Guid.NewGuid():N}");
        Context = new EfContext(options.Options);
        Service = new SpellService(new Repository(Context));
    }
    public static async Task<TestDatabase> CreateAsync()
    {
        var result = new TestDatabase();
        if (result.IsPostgres) await result.Context.Database.MigrateAsync();
        else await result.Context.Database.EnsureCreatedAsync();
        return result;
    }
    public async ValueTask DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();
    }
    private sealed class CaptureCommands(List<string> commands) : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<System.Data.Common.DbDataReader>> ReaderExecutingAsync(
            System.Data.Common.DbCommand command, CommandEventData eventData,
            InterceptionResult<System.Data.Common.DbDataReader> result, CancellationToken cancellationToken = default)
        {
            commands.Add(command.CommandText);
            return ValueTask.FromResult(result);
        }
    }
}
