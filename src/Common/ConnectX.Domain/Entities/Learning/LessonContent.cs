using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class LessonContent : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }
    public Guid? PublishedByTeacherId { get; set; }
    public Teacher? PublishedByTeacher { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Body { get; set; }
    public int SortOrder { get; set; }
    public int? EstimatedStudyMinutes { get; set; }
    public DateTimeOffset? AvailableFrom { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public ContentVisibility Visibility { get; set; }
    public bool IsRequired { get; set; } = true;
}

