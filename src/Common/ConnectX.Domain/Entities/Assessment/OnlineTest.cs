using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class OnlineTest : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public bool IsPracticeTest { get; set; }
    public DateTimeOffset? OpensAt { get; set; }
    public DateTimeOffset? ClosesAt { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public int? AttemptLimit { get; set; }
    public decimal? PassingScore { get; set; }

    public List<TestQuestion> Questions { get; set; } = [];
    public List<TestAttempt> Attempts { get; set; } = [];
}

