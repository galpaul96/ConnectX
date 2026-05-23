using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class AnnouncementEntityTypeConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> configuration)
        {
            configuration.ConfigureAuditedEntity("Announcements");

            configuration.Property(o => o.Title).HasMaxLength(256);

            configuration.HasIndex(o => new { o.ModuleOfferingId, o.PublishedAt });
            configuration.HasIndex(o => new { o.ModuleOfferingId, o.IsPinned });
            configuration.HasIndex(o => o.TeacherId);
            configuration.HasIndex(o => o.ExpiresAt);

            configuration.HasOne(o => o.ModuleOffering)
                .WithMany(o => o.Announcements)
                .HasForeignKey(o => o.ModuleOfferingId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Teacher)
                .WithMany()
                .HasForeignKey(o => o.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
