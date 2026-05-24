using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class UserNotification : Audit
{
    public Guid ExternalId { get; set; }
    public string RecipientUserId { get; set; } = string.Empty;
    public string? IssuerUserId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    public string? Topic { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool UserViewed { get; set; }
    public DateTimeOffset? EventStartsAt { get; set; }
    public DateTimeOffset? EventEndsAt { get; set; }
    public string? ActionUrl { get; set; }
    public string? SourceType { get; set; }
    public string? SourceId { get; set; }
}
