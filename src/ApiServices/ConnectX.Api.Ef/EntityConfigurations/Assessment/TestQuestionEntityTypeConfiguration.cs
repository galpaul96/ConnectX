using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class TestQuestionEntityTypeConfiguration : IEntityTypeConfiguration<TestQuestion>
    {
        public void Configure(EntityTypeBuilder<TestQuestion> configuration)
        {
            configuration.ConfigureAuditedEntity("TestQuestions");

            configuration.Property(o => o.Points).HasPrecision(7, 2);

            configuration.HasIndex(o => new { o.OnlineTestId, o.SortOrder });

            configuration.HasOne(o => o.OnlineTest)
                .WithMany(o => o.Questions)
                .HasForeignKey(o => o.OnlineTestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
