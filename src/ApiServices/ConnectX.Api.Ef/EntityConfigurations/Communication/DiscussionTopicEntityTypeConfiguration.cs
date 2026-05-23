using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class DiscussionTopicEntityTypeConfiguration : IEntityTypeConfiguration<DiscussionTopic>
    {
        public void Configure(EntityTypeBuilder<DiscussionTopic> configuration)
        {
            configuration.ConfigureAuditedEntity("DiscussionTopics");

            configuration.Property(o => o.Title).HasMaxLength(256);

            configuration.HasIndex(o => new { o.ModuleOfferingId, o.Status, o.IsPinned });
            configuration.HasIndex(o => new { o.ModuleOfferingId, o.OpenedAt });
            configuration.HasIndex(o => o.MeetingId);
            configuration.HasIndex(o => new { o.CreatedById, o.CreatedByRole });

            configuration.HasOne(o => o.ModuleOffering)
                .WithMany(o => o.DiscussionTopics)
                .HasForeignKey(o => o.ModuleOfferingId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany()
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
