namespace ConnectX.Domain.Entities.Catalogs.Spells;

public class Spell : Audit
{
    public string Name { get; set; }

    public ICollection<SpellComponent> Components { get; set; }
}
