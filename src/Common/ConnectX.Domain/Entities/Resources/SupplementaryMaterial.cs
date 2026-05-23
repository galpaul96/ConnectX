using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class SupplementaryMaterial : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }
    public Guid? PublishedByTeacherId { get; set; }
    public Teacher? PublishedByTeacher { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ResourceType ResourceType { get; set; }
    public string? FileUrl { get; set; }
    public string? ExternalUrl { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public bool IsHighlighted { get; set; }
}

