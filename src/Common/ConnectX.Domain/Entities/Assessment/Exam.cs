using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class Exam : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }

    public string Title { get; set; } = string.Empty;
    public AssessmentType AssessmentType { get; set; }
    public DateTimeOffset? ScheduledAt { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Location { get; set; }
    public string? OnlineExamUrl { get; set; }
    public DateTimeOffset? RegistrationDeadline { get; set; }
    public decimal? WeightPercentage { get; set; }
    public decimal? PassingScore { get; set; }
    public string? Instructions { get; set; }
    public DateTimeOffset? ResultsPublishedAt { get; set; }

    public List<ExamResult> Results { get; set; } = [];
}

