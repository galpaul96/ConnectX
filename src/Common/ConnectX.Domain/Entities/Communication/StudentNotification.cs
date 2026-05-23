using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class StudentNotification : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? ModuleOfferingId { get; set; }
    public ModuleOffering? ModuleOffering { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public MessagePriority Priority { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
    public string? ActionUrl { get; set; }
}
