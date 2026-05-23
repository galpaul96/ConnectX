using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class TestAttemptEntityTypeConfiguration : IEntityTypeConfiguration<TestAttempt>
    {
        public void Configure(EntityTypeBuilder<TestAttempt> configuration)
        {
            configuration.ConfigureAuditedEntity("TestAttempts");

            configuration.Property(o => o.Score).HasPrecision(7, 2);

            configuration.HasIndex(o => o.StudentId);
            configuration.HasIndex(o => new { o.OnlineTestId, o.StudentId, o.AttemptNumber }).IsUnique();
            configuration.HasIndex(o => new { o.StudentId, o.StartedAt });

            configuration.HasOne(o => o.OnlineTest)
                .WithMany(o => o.Attempts)
                .HasForeignKey(o => o.OnlineTestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
