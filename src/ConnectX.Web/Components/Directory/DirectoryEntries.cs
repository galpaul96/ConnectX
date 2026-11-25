namespace ConnectX.Web.Components.Directory;

public static class DirectoryEntries
{
    public static IReadOnlyList<DirectoryEntry> Dnd { get; } =
    [
        new("Catalogs", "Browse rules, character options, spells, and equipment.", "/dnd/catalogs", "01"),
        new("Codex", "Organize characters, people, and quests for your campaigns.", "/dnd/codex", "02")
    ];

    public static IReadOnlyList<DirectoryEntry> Catalogs { get; } =
    [
        new("Classes", "Core class identities, features, and progression.", "/dnd/catalogs/classes", "01"),
        new("Subclasses", "Specializations and paths within each class.", "/dnd/catalogs/subclasses", "02"),
        new("Species", "Playable peoples, traits, and species options.", "/dnd/catalogs/species", "03"),
        new("Backgrounds", "Origins, proficiencies, and background features.", "/dnd/catalogs/backgrounds", "04"),
        new("Skills & Spells", "Browse spell details now; skill references can be added as that data is migrated.", "/dnd/catalogs/skills", "05"),
        new("Feats", "Character options, prerequisites, and benefits.", "/dnd/catalogs/feats", "06"),
        new("Stat Blocks", "Creatures, NPCs, and other rules-ready profiles.", "/dnd/catalogs/stat-blocks", "07"),
        new("Items", "Equipment, tools, treasure, and magic items.", "/dnd/catalogs/items", "08")
    ];

    public static IReadOnlyList<DirectoryEntry> Codex { get; } =
    [
        new("Characters", "Player characters, builds, and advancement.", "/dnd/codex/characters", "01"),
        new("NPCs", "People, relationships, and campaign knowledge.", "/dnd/codex/npcs", "02"),
        new("Quests", "Goals, leads, milestones, and outcomes.", "/dnd/codex/quests", "03")
    ];
}
