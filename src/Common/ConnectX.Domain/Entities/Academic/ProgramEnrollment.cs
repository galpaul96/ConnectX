using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class ProgramEnrollment : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid StudyProgramId { get; set; }
    public StudyProgram? StudyProgram { get; set; }

    public string? StudentNumber { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateOnly EnrolledOn { get; set; }
    public DateOnly? ExpectedCompletionOn { get; set; }
    public DateOnly? CompletedOn { get; set; }
    public decimal? ProgressPercentage { get; set; }
    public string? AdvisorNotes { get; set; }
}

