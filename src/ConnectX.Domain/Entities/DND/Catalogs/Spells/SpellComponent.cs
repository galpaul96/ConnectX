using System.Text.Json;

namespace ConnectX.Domain.Entities.DND.Catalogs.Spells;

public class SpellComponent : Audit
{
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Position { get; set; }
    // One jsonb column for every JSON shape, including strings and scalars.
    // The JSON contents are never EF owned entities or navigations.
    public JsonElement Value { get; set; } = JsonSerializer.SerializeToElement<object?>(null);
    public string Html { get; set; } = string.Empty;
    public Guid SpellId { get; set; }
    public Spell Spell { get; set; } = null!;
}
