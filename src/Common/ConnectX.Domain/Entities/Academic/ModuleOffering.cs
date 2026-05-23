using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class ModuleOffering : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? CohortId { get; set; }
    public Cohort? Cohort { get; set; }
    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    public Guid? AcademicLocationId { get; set; }
    public AcademicLocation? AcademicLocation { get; set; }

    public string AcademicYear { get; set; } = string.Empty;
    public string Term { get; set; } = string.Empty;
    public DeliveryMode DeliveryMode { get; set; }
    public DateOnly StartsOn { get; set; }
    public DateOnly? EndsOn { get; set; }
    public string? Location { get; set; }
    public string? OnlineClassroomUrl { get; set; }
    public int? MaximumParticipants { get; set; }

    public List<Meeting> Meetings { get; set; } = [];
    public List<ModuleEnrollment> Enrollments { get; set; } = [];
    public List<DiscussionTopic> DiscussionTopics { get; set; } = [];
    public List<Announcement> Announcements { get; set; } = [];
}
