using System.Text.Json;

namespace ConnectX.Domain.Entities.DND.Items;

public class ItemComponent : Audit
{
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Position { get; set; }
    // One jsonb column for every JSON shape, including strings and scalars.
    // The JSON contents are never EF owned entities or navigations.
    public JsonElement Value { get; set; } = JsonSerializer.SerializeToElement<object?>(null);
    public string Html { get; set; } = string.Empty;
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = null!;
}
