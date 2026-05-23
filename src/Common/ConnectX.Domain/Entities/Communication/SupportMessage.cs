using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class SupportMessage : Audit
{
    public Guid ExternalId { get; set; }
    public Guid SupportRequestId { get; set; }
    public SupportRequest? SupportRequest { get; set; }

    public Guid AuthorId { get; set; }
    public AuthorRole AuthorRole { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTimeOffset SentAt { get; set; }
    public bool IsInternalNote { get; set; }
}

