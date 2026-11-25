namespace ConnectX.Web.Components.Pages.DND.Catalogs.Skills.Components;

/// <summary>
/// Temporary UI fixture. Replace this provider with a JSON-backed catalog service later.
/// Names and descriptions are illustrative and are not loaded from src/data.
/// </summary>
public static class SpellCatalogPreviewData
{
    public static IReadOnlyList<SpellCatalogEntry> Entries { get; } =
    [
        Create("aether-spark", "Aether Spark", 0, "Cantrip", "Evocation", "1 Action", "60 ft.", "Instantaneous", "A bright mote streaks toward a creature and bursts in a brief flare.", ["Sorcerer", "Wizard"], ["Damage"], damage: "1d8 force", attackSave: "Ranged spell attack"),
        Create("briar-thread", "Briar Thread", 0, "Cantrip", "Transmutation", "1 Action", "30 ft.", "Instantaneous", "A thorned strand lashes out and briefly catches at a target's clothing.", ["Druid", "Ranger"], ["Control"], damage: "1d6 piercing", attackSave: "DEX save"),
        Create("veiled-whisper", "Veiled Whisper", 0, "Cantrip", "Illusion", "1 Action", "Self", "1 Round", "A faint illusion disguises a short phrase as a nearby sound.", ["Bard", "Wizard"], ["Social"], components: "V, S"),
        Create("cinder-lance", "Cinder Lance", 1, "1st", "Evocation", "1 Action", "90 ft.", "Instantaneous", "A lance of ember-light strikes one creature and leaves sparks in its wake.", ["Sorcerer", "Wizard"], ["Damage"], damage: "2d8 fire", attackSave: "Ranged spell attack", components: "V, S"),
        Create("lantern-ward", "Lantern Ward", 1, "1st", "Abjuration", "1 Minute", "Touch", "8 Hours", "A willing creature gains a soft ward that brightens when danger draws near.", ["Cleric", "Wizard"], ["Warding"], ritual: true, components: "V, S, M", material: "A brass wick or a pinch of amber dust"),
        Create("mending-current", "Mending Current", 1, "1st", "Transmutation", "1 Action", "Touch", "Instantaneous", "A gentle current knits a small tear in an object or draws scattered pieces together.", ["Artificer", "Druid"], ["Utility"]),
        Create("shatterwind", "Shatterwind", 2, "2nd", "Evocation", "1 Action", "Self (15-foot cone)", "Instantaneous", "A compressed gust rolls outward, rattling loose objects and striking creatures.", ["Bard", "Sorcerer", "Wizard"], ["Damage", "Control"], concentration: false, damage: "3d6 thunder", attackSave: "CON save"),
        Create("stillwater-aegis", "Stillwater Aegis", 2, "2nd", "Abjuration", "1 Bonus Action", "Self", "1 Minute", "A glassy ring of water turns aside the next force that would move you.", ["Cleric", "Druid"], ["Warding"], concentration: true),
        Create("mirror-step", "Mirror Step", 2, "2nd", "Illusion", "1 Bonus Action", "Self", "Instantaneous", "You trade places with a fleeting reflection visible nearby.", ["Bard", "Sorcerer", "Wizard"], ["Movement"]),
        Create("stormglass-call", "Stormglass Call", 3, "3rd", "Evocation", "1 Action", "120 ft.", "Instantaneous", "A suspended shard cracks with thunder and releases a focused bolt of lightning.", ["Druid", "Sorcerer", "Wizard"], ["Damage"], damage: "6d6 lightning", attackSave: "DEX save"),
        Create("thornwake", "Thornwake", 3, "3rd", "Conjuration", "1 Action", "90 ft.", "1 Minute", "A line of thorny growth surges across the ground and hinders passage.", ["Druid", "Ranger"], ["Control"], concentration: true),
        Create("quiet-archive", "Quiet Archive", 3, "3rd", "Divination", "10 Minutes", "Self", "1 Hour", "You recall a fragment of lore connected to a place, object, or name.", ["Bard", "Cleric", "Wizard"], ["Information"], ritual: true),
        Create("crown-of-embers", "Crown of Embers", 4, "4th", "Evocation", "1 Action", "Self", "1 Minute", "A crown of flame circles your head and lashes at nearby threats.", ["Sorcerer", "Wizard"], ["Damage"], concentration: true, damage: "4d8 fire"),
        Create("moonlit-passage", "Moonlit Passage", 4, "4th", "Conjuration", "1 Action", "60 ft.", "Instantaneous", "A silver doorway opens between two places you can see.", ["Druid", "Ranger", "Sorcerer"], ["Movement"], components: "V, S, M", material: "A silver thread worth 5 gp"),
        Create("winters-grasp", "Winter's Grasp", 5, "5th", "Necromancy", "1 Action", "60 ft.", "Instantaneous", "A pale chill clings to a creature and drains warmth from its limbs.", ["Sorcerer", "Warlock", "Wizard"], ["Damage", "Debuff"], damage: "5d8 cold", attackSave: "CON save"),
        Create("waking-dream", "Waking Dream", 5, "5th", "Enchantment", "1 Action", "90 ft.", "1 Minute", "You wrap a creature's senses in a vivid dream while it remains awake.", ["Bard", "Warlock", "Wizard"], ["Control"], concentration: true, attackSave: "WIS save"),
        Create("starfall-mantle", "Starfall Mantle", 7, "7th", "Transmutation", "1 Action", "Self", "10 Minutes", "Starlight gathers around you, carrying you above the ground.", ["Druid", "Sorcerer", "Wizard"], ["Movement", "Warding"], concentration: true),
        Create("worldfold", "Worldfold", 9, "9th", "Conjuration", "1 Action", "Special", "Instantaneous", "You fold a remembered route into a single step across great distance.", ["Bard", "Sorcerer", "Wizard"], ["Movement"], components: "V, S, M", material: "A map marked with the destination")
    ];

    private static SpellCatalogEntry Create(
        string id,
        string name,
        int level,
        string levelLabel,
        string school,
        string castingTime,
        string rangeArea,
        string duration,
        string description,
        IReadOnlyList<string> availableFor,
        IReadOnlyList<string> tags,
        bool ritual = false,
        bool concentration = false,
        string? components = "V, S",
        string? material = null,
        string? damage = null,
        string? attackSave = null) => new()
        {
            Id = id,
            Name = name,
            LevelValue = level,
            LevelLabel = levelLabel,
            School = school,
            CastingTime = castingTime,
            CastingTimeNote = ritual ? "You can cast this spell as a ritual." : null,
            RangeArea = rangeArea,
            Components = components,
            MaterialDetail = material,
            Duration = concentration ? $"Concentration, up to {duration}" : duration,
            AttackSave = attackSave,
            DamageEffect = damage,
            DamageTypes = damage is null ? [] : [damage.Split(' ').Last()],
            Description = [description],
            SpellTags = tags,
            AvailableFor = availableFor,
            Source = "Preview entry · not loaded from catalog files",
            IsRitual = ritual,
            IsConcentration = concentration
        };
}
