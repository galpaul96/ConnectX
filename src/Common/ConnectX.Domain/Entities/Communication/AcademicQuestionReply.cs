using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class AcademicQuestionReply : Audit
{
    public Guid ExternalId { get; set; }
    public Guid AcademicQuestionId { get; set; }
    public AcademicQuestion? AcademicQuestion { get; set; }

    public Guid AuthorId { get; set; }
    public AuthorRole AuthorRole { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTimeOffset PostedAt { get; set; }
    public bool IsAcceptedAnswer { get; set; }
}

