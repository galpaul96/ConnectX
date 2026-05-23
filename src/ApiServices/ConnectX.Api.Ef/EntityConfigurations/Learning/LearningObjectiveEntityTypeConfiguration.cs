using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class LearningObjectiveEntityTypeConfiguration : IEntityTypeConfiguration<LearningObjective>
    {
        public void Configure(EntityTypeBuilder<LearningObjective> configuration)
        {
            configuration.ConfigureAuditedEntity("LearningObjectives");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.BloomLevel).HasMaxLength(64);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.SortOrder });
            configuration.HasIndex(o => new { o.MeetingId, o.SortOrder });

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.LearningObjectives)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany(o => o.LearningObjectives)
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
