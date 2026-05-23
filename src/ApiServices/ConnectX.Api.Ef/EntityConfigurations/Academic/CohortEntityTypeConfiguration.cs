using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Academic
{
    internal class CohortEntityTypeConfiguration : IEntityTypeConfiguration<Cohort>
    {
        public void Configure(EntityTypeBuilder<Cohort> configuration)
        {
            configuration.ConfigureAuditedEntity("Cohorts");

            configuration.Property(o => o.Name).HasMaxLength(256);
            configuration.Property(o => o.AcademicYear).HasMaxLength(32);
            configuration.Property(o => o.Location).HasMaxLength(256);

            configuration.HasIndex(o => new { o.StudyProgramId, o.AcademicYear });
            configuration.HasIndex(o => new { o.StudyProgramId, o.Name });

            configuration.HasOne(o => o.StudyProgram)
                .WithMany(o => o.Cohorts)
                .HasForeignKey(o => o.StudyProgramId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
