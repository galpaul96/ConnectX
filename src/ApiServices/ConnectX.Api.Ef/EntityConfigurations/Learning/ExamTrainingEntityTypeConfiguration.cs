using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class ExamTrainingEntityTypeConfiguration : IEntityTypeConfiguration<ExamTraining>
    {
        public void Configure(EntityTypeBuilder<ExamTraining> configuration)
        {
            configuration.ConfigureAuditedEntity("ExamTrainings");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.PassingScore).HasPrecision(7, 2);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.OpensAt });
            configuration.HasIndex(o => o.ClosesAt);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany()
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasMany(o => o.PracticeExercises)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ExamTrainingPracticeExercises",
                    right => right.HasOne<PracticeExercise>()
                        .WithMany()
                        .HasForeignKey("PracticeExerciseId")
                        .OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<ExamTraining>()
                        .WithMany()
                        .HasForeignKey("ExamTrainingId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.ToTable("ExamTrainingPracticeExercises");
                        join.HasKey("ExamTrainingId", "PracticeExerciseId");
                        join.HasIndex("PracticeExerciseId");
                    });

            configuration.HasMany(o => o.PracticeTests)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ExamTrainingPracticeTests",
                    right => right.HasOne<OnlineTest>()
                        .WithMany()
                        .HasForeignKey("OnlineTestId")
                        .OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<ExamTraining>()
                        .WithMany()
                        .HasForeignKey("ExamTrainingId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.ToTable("ExamTrainingPracticeTests");
                        join.HasKey("ExamTrainingId", "OnlineTestId");
                        join.HasIndex("OnlineTestId");
                    });
        }
    }
}
