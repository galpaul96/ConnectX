using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class AcademicQuestionEntityTypeConfiguration : IEntityTypeConfiguration<AcademicQuestion>
    {
        public void Configure(EntityTypeBuilder<AcademicQuestion> configuration)
        {
            configuration.ConfigureAuditedEntity("AcademicQuestions");

            configuration.Property(o => o.Title).HasMaxLength(256);

            configuration.HasIndex(o => o.StudentId);
            configuration.HasIndex(o => new { o.ModuleOfferingId, o.Status });
            configuration.HasIndex(o => o.AssignmentId);
            configuration.HasIndex(o => o.PracticeExerciseId);
            configuration.HasIndex(o => o.AcceptedAnswerId);

            configuration.HasOne(o => o.ModuleOffering)
                .WithMany()
                .HasForeignKey(o => o.ModuleOfferingId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Assignment)
                .WithMany()
                .HasForeignKey(o => o.AssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.PracticeExercise)
                .WithMany()
                .HasForeignKey(o => o.PracticeExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne<AcademicQuestionReply>()
                .WithMany()
                .HasForeignKey(o => o.AcceptedAnswerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
