using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class ExamEntityTypeConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> configuration)
        {
            configuration.ConfigureAuditedEntity("Exams");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.Location).HasMaxLength(256);
            configuration.Property(o => o.OnlineExamUrl).HasMaxLength(1024);
            configuration.Property(o => o.WeightPercentage).HasPrecision(5, 2);
            configuration.Property(o => o.PassingScore).HasPrecision(7, 2);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.ScheduledAt });
            configuration.HasIndex(o => o.ResultsPublishedAt);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.Exams)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
