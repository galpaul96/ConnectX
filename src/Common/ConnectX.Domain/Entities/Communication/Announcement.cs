using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class Announcement : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ModuleOfferingId { get; set; }
    public ModuleOffering? ModuleOffering { get; set; }
    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public CommunicationAudience Audience { get; set; }
    public MessagePriority Priority { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool IsPinned { get; set; }
}

