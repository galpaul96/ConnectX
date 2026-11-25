namespace ConnectX.Web.Components.Pages.DND.Catalogs.Skills.Components;

/// <summary>
/// UI-facing spell model. The future JSON adapter should map the spell catalog's
/// component keys into these optional fields; absent values are intentionally omitted by the UI.
/// </summary>
public sealed record SpellCatalogEntry
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required int LevelValue { get; init; }
    public required string LevelLabel { get; init; }
    public string? School { get; init; }
    public string? CastingTime { get; init; }
    public string? CastingTimeNote { get; init; }
    public string? RangeArea { get; init; }
    public string? Components { get; init; }
    public string? MaterialDetail { get; init; }
    public string? Duration { get; init; }
    public string? AttackSave { get; init; }
    public string? DamageEffect { get; init; }
    public IReadOnlyList<string> DamageTypes { get; init; } = [];
    public IReadOnlyList<string> Description { get; init; } = [];
    public IReadOnlyList<string> SpellTags { get; init; } = [];
    public IReadOnlyList<string> AvailableFor { get; init; } = [];
    public string? Source { get; init; }
    public string? SourceUrl { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsRitual { get; init; }
    public bool IsConcentration { get; init; }

    public string SearchableDescription => string.Join(' ', Description);
}
