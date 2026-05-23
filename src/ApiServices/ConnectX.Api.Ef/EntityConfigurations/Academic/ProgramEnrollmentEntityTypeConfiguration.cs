using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Academic
{
    internal class ProgramEnrollmentEntityTypeConfiguration : IEntityTypeConfiguration<ProgramEnrollment>
    {
        public void Configure(EntityTypeBuilder<ProgramEnrollment> configuration)
        {
            configuration.ConfigureAuditedEntity("ProgramEnrollments");

            configuration.Property(o => o.StudentNumber).HasMaxLength(64);
            configuration.Property(o => o.ProgressPercentage).HasPrecision(5, 2);

            configuration.HasIndex(o => o.StudentId);
            configuration.HasIndex(o => new { o.StudentId, o.StudyProgramId }).IsUnique();
            configuration.HasIndex(o => new { o.StudyProgramId, o.Status });

            configuration.HasOne(o => o.StudyProgram)
                .WithMany(o => o.Enrollments)
                .HasForeignKey(o => o.StudyProgramId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
