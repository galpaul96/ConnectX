namespace ConnectX.Domain.Entities.DND.Catalogs.Spells;

public class Spell : Audit
{
    public string SourceId { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string StorageFile { get; set; } = string.Empty;
    public ICollection<SpellComponent> Components { get; set; } = new List<SpellComponent>();
}
