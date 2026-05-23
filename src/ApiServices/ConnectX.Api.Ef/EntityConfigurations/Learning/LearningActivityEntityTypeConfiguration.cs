using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class LearningActivityEntityTypeConfiguration : IEntityTypeConfiguration<LearningActivity>
    {
        public void Configure(EntityTypeBuilder<LearningActivity> configuration)
        {
            configuration.ConfigureAuditedEntity("LearningActivities");

            configuration.Property(o => o.Title).HasMaxLength(256);

            configuration.HasIndex(o => new { o.MeetingId, o.SortOrder });
            configuration.HasIndex(o => new { o.MeetingId, o.ActivityType });

            configuration.HasOne(o => o.Meeting)
                .WithMany(o => o.LearningActivities)
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
