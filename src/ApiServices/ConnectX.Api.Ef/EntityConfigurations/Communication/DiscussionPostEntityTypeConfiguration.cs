using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class DiscussionPostEntityTypeConfiguration : IEntityTypeConfiguration<DiscussionPost>
    {
        public void Configure(EntityTypeBuilder<DiscussionPost> configuration)
        {
            configuration.ConfigureAuditedEntity("DiscussionPosts");

            configuration.HasIndex(o => new { o.DiscussionTopicId, o.PostedAt });
            configuration.HasIndex(o => o.ParentPostId);
            configuration.HasIndex(o => new { o.AuthorId, o.AuthorRole });

            configuration.HasOne(o => o.DiscussionTopic)
                .WithMany(o => o.Posts)
                .HasForeignKey(o => o.DiscussionTopicId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.ParentPost)
                .WithMany(o => o.Replies)
                .HasForeignKey(o => o.ParentPostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
