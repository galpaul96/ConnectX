using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class AcademicQuestion : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid ModuleOfferingId { get; set; }
    public ModuleOffering? ModuleOffering { get; set; }
    public Guid? AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    public Guid? PracticeExerciseId { get; set; }
    public PracticeExercise? PracticeExercise { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public QuestionStatus Status { get; set; }
    public DateTimeOffset AskedAt { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }
    public Guid? AcceptedAnswerId { get; set; }

    public List<AcademicQuestionReply> Replies { get; set; } = [];
}

