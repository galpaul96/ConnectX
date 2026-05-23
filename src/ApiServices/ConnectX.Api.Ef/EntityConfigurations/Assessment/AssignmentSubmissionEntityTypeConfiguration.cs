using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class AssignmentSubmissionEntityTypeConfiguration : IEntityTypeConfiguration<AssignmentSubmission>
    {
        public void Configure(EntityTypeBuilder<AssignmentSubmission> configuration)
        {
            configuration.ConfigureAuditedEntity("AssignmentSubmissions");

            configuration.Property(o => o.FileUrl).HasMaxLength(1024);
            configuration.Property(o => o.Grade).HasMaxLength(32);
            configuration.Property(o => o.Score).HasPrecision(7, 2);

            configuration.HasIndex(o => o.StudentId);
            configuration.HasIndex(o => o.GradedByTeacherId);
            configuration.HasIndex(o => new { o.AssignmentId, o.StudentId, o.AttemptNumber }).IsUnique();
            configuration.HasIndex(o => new { o.AssignmentId, o.Status });

            configuration.HasOne(o => o.Assignment)
                .WithMany(o => o.Submissions)
                .HasForeignKey(o => o.AssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.GradedByTeacher)
                .WithMany()
                .HasForeignKey(o => o.GradedByTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
