using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class SupportRequestEntityTypeConfiguration : IEntityTypeConfiguration<SupportRequest>
    {
        public void Configure(EntityTypeBuilder<SupportRequest> configuration)
        {
            configuration.ConfigureAuditedEntity("SupportRequests");

            configuration.Property(o => o.ReferenceNumber).HasMaxLength(64);
            configuration.Property(o => o.Subject).HasMaxLength(256);

            configuration.HasIndex(o => o.ReferenceNumber).IsUnique();
            configuration.HasIndex(o => new { o.StudentId, o.Status });
            configuration.HasIndex(o => new { o.Status, o.Priority });
            configuration.HasIndex(o => o.ProgramModuleId);
            configuration.HasIndex(o => o.ModuleOfferingId);
            configuration.HasIndex(o => o.AssignedStaffId);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany()
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.ModuleOffering)
                .WithMany()
                .HasForeignKey(o => o.ModuleOfferingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
