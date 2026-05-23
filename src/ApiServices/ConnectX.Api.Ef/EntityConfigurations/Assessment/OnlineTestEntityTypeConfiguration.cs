using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Assessment
{
    internal class OnlineTestEntityTypeConfiguration : IEntityTypeConfiguration<OnlineTest>
    {
        public void Configure(EntityTypeBuilder<OnlineTest> configuration)
        {
            configuration.ConfigureAuditedEntity("OnlineTests");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.PassingScore).HasPrecision(7, 2);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.IsPracticeTest });
            configuration.HasIndex(o => new { o.MeetingId, o.OpensAt });
            configuration.HasIndex(o => o.ClosesAt);

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
