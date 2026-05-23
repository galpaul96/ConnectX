using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class Assignment : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public AssignmentType AssignmentType { get; set; }
    public AssignmentStatus Status { get; set; }
    public DateTimeOffset? AvailableFrom { get; set; }
    public DateTimeOffset? DueAt { get; set; }
    public decimal? MaximumScore { get; set; }
    public decimal? WeightPercentage { get; set; }
    public bool IsPreparationRequired { get; set; }
    public bool AllowsResubmission { get; set; }
    public string? RubricUrl { get; set; }

    public List<AssignmentSubmission> Submissions { get; set; } = [];
}

