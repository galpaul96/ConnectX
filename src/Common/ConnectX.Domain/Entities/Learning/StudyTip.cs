using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class StudyTip : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }
    public Guid? PublishedByTeacherId { get; set; }
    public Teacher? PublishedByTeacher { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public StudyTipCategory Category { get; set; }
    public int SortOrder { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public bool IsHighlighted { get; set; }
}

