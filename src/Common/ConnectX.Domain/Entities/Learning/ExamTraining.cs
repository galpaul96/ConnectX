using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class ExamTraining : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset? OpensAt { get; set; }
    public DateTimeOffset? ClosesAt { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public decimal? PassingScore { get; set; }
    public bool IsOptional { get; set; } = true;

    public List<PracticeExercise> PracticeExercises { get; set; } = [];
    public List<OnlineTest> PracticeTests { get; set; } = [];
}

