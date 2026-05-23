using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class DiscussionPost : Audit
{
    public Guid ExternalId { get; set; }
    public Guid DiscussionTopicId { get; set; }
    public DiscussionTopic? DiscussionTopic { get; set; }
    public Guid? ParentPostId { get; set; }
    public DiscussionPost? ParentPost { get; set; }

    public Guid AuthorId { get; set; }
    public AuthorRole AuthorRole { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTimeOffset PostedAt { get; set; }
    public DateTimeOffset? EditedAt { get; set; }
    public bool IsInstructorEndorsed { get; set; }

    public List<DiscussionPost> Replies { get; set; } = [];
}
