using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class Meeting : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ModuleOfferingId { get; set; }
    public ModuleOffering? ModuleOffering { get; set; }
    public Guid? AcademicLocationId { get; set; }
    public AcademicLocation? AcademicLocation { get; set; }

    public int SequenceNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset StartsAt { get; set; }
    public DateTimeOffset EndsAt { get; set; }
    public MeetingFormat Format { get; set; }
    public string? Location { get; set; }
    public string? OnlineMeetingUrl { get; set; }
    public string? PreparationInstructions { get; set; }
    public string? CancellationReason { get; set; }
    public bool IsCancelled { get; set; }

    public List<LearningObjective> LearningObjectives { get; set; } = [];
    public List<LearningActivity> LearningActivities { get; set; } = [];
    public List<Assignment> PreparationAssignments { get; set; } = [];
    public List<LearningResource> Resources { get; set; } = [];
    public List<LessonContent> LessonContents { get; set; } = [];
    public List<StudyTip> StudyTips { get; set; } = [];
    public List<AttendanceRecord> AttendanceRecords { get; set; } = [];
}
