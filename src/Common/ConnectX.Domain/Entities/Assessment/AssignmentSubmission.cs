using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class AssignmentSubmission : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    public Guid? GradedByTeacherId { get; set; }
    public Teacher? GradedByTeacher { get; set; }

    public int AttemptNumber { get; set; } = 1;
    public SubmissionStatus Status { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public string? SubmissionText { get; set; }
    public string? FileUrl { get; set; }
    public decimal? Score { get; set; }
    public string? Grade { get; set; }
    public string? Feedback { get; set; }
    public DateTimeOffset? GradedAt { get; set; }
}

