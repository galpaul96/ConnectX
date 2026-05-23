using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class LearningActivity : Audit
{
    public Guid ExternalId { get; set; }
    public Guid MeetingId { get; set; }
    public Meeting? Meeting { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public LearningActivityType ActivityType { get; set; }
    public int SortOrder { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Instructions { get; set; }
    public bool IsRequired { get; set; } = true;
}

