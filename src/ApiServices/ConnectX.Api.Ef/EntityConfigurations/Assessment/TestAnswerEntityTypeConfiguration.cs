using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class TestAnswerEntityTypeConfiguration : IEntityTypeConfiguration<TestAnswer>
    {
        public void Configure(EntityTypeBuilder<TestAnswer> configuration)
        {
            configuration.ConfigureAuditedEntity("TestAnswers");

            configuration.Property(o => o.PointsAwarded).HasPrecision(7, 2);

            configuration.HasIndex(o => new { o.TestAttemptId, o.TestQuestionId });
            configuration.HasIndex(o => o.SelectedOptionId);

            configuration.HasOne(o => o.TestAttempt)
                .WithMany(o => o.Answers)
                .HasForeignKey(o => o.TestAttemptId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.TestQuestion)
                .WithMany()
                .HasForeignKey(o => o.TestQuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.SelectedOption)
                .WithMany()
                .HasForeignKey(o => o.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
