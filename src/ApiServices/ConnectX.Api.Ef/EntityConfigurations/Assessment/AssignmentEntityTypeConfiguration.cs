using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class AssignmentEntityTypeConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> configuration)
        {
            configuration.ConfigureAuditedEntity("Assignments");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.RubricUrl).HasMaxLength(1024);
            configuration.Property(o => o.MaximumScore).HasPrecision(7, 2);
            configuration.Property(o => o.WeightPercentage).HasPrecision(5, 2);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.Status });
            configuration.HasIndex(o => new { o.MeetingId, o.DueAt });
            configuration.HasIndex(o => o.DueAt);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.Assignments)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany(o => o.PreparationAssignments)
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
