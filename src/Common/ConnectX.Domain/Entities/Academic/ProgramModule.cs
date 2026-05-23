using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class ProgramModule : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudyProgramId { get; set; }
    public StudyProgram? StudyProgram { get; set; }
    public Guid? CoordinatorTeacherId { get; set; }
    public Teacher? CoordinatorTeacher { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SequenceNumber { get; set; }
    public decimal CreditValue { get; set; }
    public int EstimatedStudyHours { get; set; }
    public string? Prerequisites { get; set; }
    public bool IsMandatory { get; set; } = true;

    public List<LearningObjective> LearningObjectives { get; set; } = [];
    public List<ModuleOffering> Offerings { get; set; } = [];
    public List<LearningResource> Resources { get; set; } = [];
    public List<Assignment> Assignments { get; set; } = [];
    public List<Exam> Exams { get; set; } = [];
    public List<DownloadDocument> Documents { get; set; } = [];
    public List<LessonContent> LessonContents { get; set; } = [];
    public List<StudyTip> StudyTips { get; set; } = [];
    public List<BibliographicReference> BibliographicReferences { get; set; } = [];
}
