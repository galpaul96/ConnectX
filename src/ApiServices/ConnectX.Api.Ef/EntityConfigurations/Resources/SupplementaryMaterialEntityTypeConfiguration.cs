using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Resources
{
    internal class SupplementaryMaterialEntityTypeConfiguration : IEntityTypeConfiguration<SupplementaryMaterial>
    {
        public void Configure(EntityTypeBuilder<SupplementaryMaterial> configuration)
        {
            configuration.ConfigureAuditedEntity("SupplementaryMaterials");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.FileUrl).HasMaxLength(1024);
            configuration.Property(o => o.ExternalUrl).HasMaxLength(1024);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.PublishedAt });
            configuration.HasIndex(o => new { o.MeetingId, o.PublishedAt });
            configuration.HasIndex(o => o.PublishedByTeacherId);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany()
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany()
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.PublishedByTeacher)
                .WithMany()
                .HasForeignKey(o => o.PublishedByTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
