using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class PracticeExerciseEntityTypeConfiguration : IEntityTypeConfiguration<PracticeExercise>
    {
        public void Configure(EntityTypeBuilder<PracticeExercise> configuration)
        {
            configuration.ConfigureAuditedEntity("PracticeExercises");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.DifficultyLevel).HasMaxLength(64);
            configuration.Property(o => o.ResourceUrl).HasMaxLength(1024);
            configuration.Property(o => o.SolutionUrl).HasMaxLength(1024);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.IsOptional });
            configuration.HasIndex(o => o.MeetingId);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany()
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany()
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
