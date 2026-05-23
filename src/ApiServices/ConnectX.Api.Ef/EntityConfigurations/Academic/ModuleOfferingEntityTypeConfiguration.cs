using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Academic
{
    internal class ModuleOfferingEntityTypeConfiguration : IEntityTypeConfiguration<ModuleOffering>
    {
        public void Configure(EntityTypeBuilder<ModuleOffering> configuration)
        {
            configuration.ConfigureAuditedEntity("ModuleOfferings");

            configuration.Property(o => o.AcademicYear).HasMaxLength(32);
            configuration.Property(o => o.Term).HasMaxLength(64);
            configuration.Property(o => o.Location).HasMaxLength(256);
            configuration.Property(o => o.OnlineClassroomUrl).HasMaxLength(1024);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.AcademicYear, o.Term });
            configuration.HasIndex(o => new { o.CohortId, o.StartsOn });
            configuration.HasIndex(o => new { o.TeacherId, o.StartsOn });
            configuration.HasIndex(o => o.AcademicLocationId);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.Offerings)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Cohort)
                .WithMany(o => o.ModuleOfferings)
                .HasForeignKey(o => o.CohortId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Teacher)
                .WithMany(o => o.ModuleOfferings)
                .HasForeignKey(o => o.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.AcademicLocation)
                .WithMany(o => o.ModuleOfferings)
                .HasForeignKey(o => o.AcademicLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
