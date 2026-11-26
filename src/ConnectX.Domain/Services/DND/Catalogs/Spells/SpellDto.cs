namespace ConnectX.Domain.Services.DND.Catalogs.Spells;

public sealed record SpellDto
{
    public Guid Id { get; init; }
    public string SourceId { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string StorageFile { get; init; } = string.Empty;
    public IReadOnlyList<SpellComponentDto> Components { get; init; } = [];
}

// Lightweight card data; descriptions and other components are loaded on demand.
public sealed record SpellSummaryDto(string Name, string? ImageUrl = null, string? Level = null, string? School = null);
public sealed record SpellPageEntry(Guid Id, SpellSummaryDto Spell);
public sealed record SpellPageDto(IReadOnlyList<SpellPageEntry> Items, int TotalCount, int PageNumber, int PageSize)
{
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
}

/// <summary>A full replacement, including the complete ordered component list.</summary>
public sealed record SpellWriteDto
{
    public required string SourceId { get; init; }
    public required string Slug { get; init; }
    public required string Name { get; init; }
    public required string StorageFile { get; init; }
    public required IReadOnlyList<SpellComponentDto> Components { get; init; }
}
