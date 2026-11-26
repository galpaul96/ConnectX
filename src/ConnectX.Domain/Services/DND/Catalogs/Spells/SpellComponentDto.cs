using System.Text.Json;

namespace ConnectX.Domain.Services.DND.Catalogs.Spells;

public sealed record SpellComponentDto
{
    public string Type { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public int Position { get; init; }
    public JsonElement Value { get; init; }
    public string Html { get; init; } = string.Empty;
}
