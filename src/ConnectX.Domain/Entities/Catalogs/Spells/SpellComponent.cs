namespace ConnectX.Domain.Entities.Catalogs.Spells
{
    public class SpellComponent : Audit
    {
        public string Label { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Html { get; set; }

        public Guid SpellId { get; set; }
        public virtual Spell Spell { get; set; }
    }
}
