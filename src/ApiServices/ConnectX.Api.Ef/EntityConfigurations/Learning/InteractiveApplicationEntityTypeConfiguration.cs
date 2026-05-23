using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class InteractiveApplicationEntityTypeConfiguration : IEntityTypeConfiguration<InteractiveApplication>
    {
        public void Configure(EntityTypeBuilder<InteractiveApplication> configuration)
        {
            configuration.ConfigureAuditedEntity("InteractiveApplications");

            configuration.Property(o => o.Name).HasMaxLength(256);
            configuration.Property(o => o.LaunchUrl).HasMaxLength(1024);
            configuration.Property(o => o.ProviderName).HasMaxLength(256);

            configuration.HasIndex(o => o.ProgramModuleId);
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
