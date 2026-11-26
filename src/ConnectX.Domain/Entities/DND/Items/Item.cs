namespace ConnectX.Domain.Entities.DND.Items;

public class Item : Audit
{
    public string SourceId { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string StorageFile { get; set; } = string.Empty;
    public ICollection<ItemComponent> Components { get; set; } = new List<ItemComponent>();
}
