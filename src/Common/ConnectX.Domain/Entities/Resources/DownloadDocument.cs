using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class DownloadDocument : Audit
{
    public Guid ExternalId { get; set; }
    public Guid? StudyProgramId { get; set; }
    public StudyProgram? StudyProgram { get; set; }
    public Guid? ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? PublishedByTeacherId { get; set; }
    public Teacher? PublishedByTeacher { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DocumentType DocumentType { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string? Version { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool IsMandatory { get; set; }
}

