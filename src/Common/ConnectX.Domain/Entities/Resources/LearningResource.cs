using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class LearningResource : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }
    public Guid? PublishedByTeacherId { get; set; }
    public Teacher? PublishedByTeacher { get; set; }
    public Guid? BibliographicReferenceId { get; set; }
    public BibliographicReference? BibliographicReference { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ResourceType ResourceType { get; set; }
    public ContentVisibility Visibility { get; set; }
    public string? Url { get; set; }
    public string? FileName { get; set; }
    public int SortOrder { get; set; }
    public bool IsRequired { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
}
