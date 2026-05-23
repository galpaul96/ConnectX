using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class StudentNotificationEntityTypeConfiguration : IEntityTypeConfiguration<StudentNotification>
    {
        public void Configure(EntityTypeBuilder<StudentNotification> configuration)
        {
            configuration.ConfigureAuditedEntity("StudentNotifications");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.ActionUrl).HasMaxLength(1024);

            configuration.HasIndex(o => new { o.StudentId, o.ReadAt });
            configuration.HasIndex(o => new { o.StudentId, o.CreatedAt });
            configuration.HasIndex(o => o.ModuleOfferingId);

            configuration.HasOne(o => o.ModuleOffering)
                .WithMany()
                .HasForeignKey(o => o.ModuleOfferingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
