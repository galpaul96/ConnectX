namespace ConnectX.Domain.Services.Catalogs.Spells
{
    public class SpellDto
    {
        public string Name { get; set; }

        public ICollection<SpellComponentDto> Components { get; set; }
    }
}
