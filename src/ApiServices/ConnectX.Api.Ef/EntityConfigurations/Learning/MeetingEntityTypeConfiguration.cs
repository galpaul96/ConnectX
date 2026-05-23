using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class MeetingEntityTypeConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> configuration)
        {
            configuration.ConfigureAuditedEntity("Meetings");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.Location).HasMaxLength(256);
            configuration.Property(o => o.OnlineMeetingUrl).HasMaxLength(1024);

            configuration.HasIndex(o => new { o.ModuleOfferingId, o.StartsAt });
            configuration.HasIndex(o => new { o.ModuleOfferingId, o.SequenceNumber }).IsUnique();
            configuration.HasIndex(o => o.AcademicLocationId);

            configuration.HasOne(o => o.ModuleOffering)
                .WithMany(o => o.Meetings)
                .HasForeignKey(o => o.ModuleOfferingId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.AcademicLocation)
                .WithMany(o => o.Meetings)
                .HasForeignKey(o => o.AcademicLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
