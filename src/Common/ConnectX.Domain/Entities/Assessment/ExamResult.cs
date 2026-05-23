using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class ExamResult : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid ExamId { get; set; }
    public Exam? Exam { get; set; }

    public int AttemptNumber { get; set; } = 1;
    public decimal? Score { get; set; }
    public string? Grade { get; set; }
    public bool Passed { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public string? Feedback { get; set; }
}

