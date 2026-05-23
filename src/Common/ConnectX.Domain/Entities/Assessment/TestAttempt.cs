using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class TestAttempt : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid OnlineTestId { get; set; }
    public OnlineTest? OnlineTest { get; set; }

    public int AttemptNumber { get; set; } = 1;
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public decimal? Score { get; set; }
    public bool? Passed { get; set; }

    public List<TestAnswer> Answers { get; set; } = [];
}

