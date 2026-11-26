namespace ConnectX.Domain.Services.DND.Catalogs.Items;

public sealed record ItemDto
{
    public Guid Id { get; init; }
    public string SourceId { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string StorageFile { get; init; } = string.Empty;
    public IReadOnlyList<ItemComponentDto> Components { get; init; } = [];
}

// Lightweight card data; descriptions and other components are loaded on demand.
public sealed record ItemSummaryDto(string Name, string? ImageUrl = null, string? ItemType = null, string? Cost = null);
public sealed record ItemPageEntry(Guid Id, ItemSummaryDto Item);
public sealed record ItemPageDto(IReadOnlyList<ItemPageEntry> Items, int TotalCount, int PageNumber, int PageSize)
{
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
}

/// <summary>A full replacement, including the complete ordered component list.</summary>
public sealed record ItemWriteDto
{
    public required string SourceId { get; init; }
    public required string Slug { get; init; }
    public required string Name { get; init; }
    public required string StorageFile { get; init; }
    public required IReadOnlyList<ItemComponentDto> Components { get; init; }
}
