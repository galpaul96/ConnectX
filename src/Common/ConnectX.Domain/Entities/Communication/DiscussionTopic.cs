using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class DiscussionTopic : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ModuleOfferingId { get; set; }
    public ModuleOffering? ModuleOffering { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }

    public Guid CreatedById { get; set; }
    public AuthorRole CreatedByRole { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Prompt { get; set; }
    public CommunicationAudience Audience { get; set; }
    public DiscussionStatus Status { get; set; }
    public bool IsPinned { get; set; }
    public DateTimeOffset OpenedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }

    public List<DiscussionPost> Posts { get; set; } = [];
}

