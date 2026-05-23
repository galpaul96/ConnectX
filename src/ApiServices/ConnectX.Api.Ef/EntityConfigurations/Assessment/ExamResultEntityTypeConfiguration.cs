using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class ExamResultEntityTypeConfiguration : IEntityTypeConfiguration<ExamResult>
    {
        public void Configure(EntityTypeBuilder<ExamResult> configuration)
        {
            configuration.ConfigureAuditedEntity("ExamResults");

            configuration.Property(o => o.Score).HasPrecision(7, 2);
            configuration.Property(o => o.Grade).HasMaxLength(32);

            configuration.HasIndex(o => o.StudentId);
            configuration.HasIndex(o => new { o.ExamId, o.StudentId, o.AttemptNumber }).IsUnique();
            configuration.HasIndex(o => new { o.StudentId, o.PublishedAt });

            configuration.HasOne(o => o.Exam)
                .WithMany(o => o.Results)
                .HasForeignKey(o => o.ExamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
