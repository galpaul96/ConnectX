using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Academic
{
    internal class ModuleEnrollmentEntityTypeConfiguration : IEntityTypeConfiguration<ModuleEnrollment>
    {
        public void Configure(EntityTypeBuilder<ModuleEnrollment> configuration)
        {
            configuration.ConfigureAuditedEntity("ModuleEnrollments");

            configuration.Property(o => o.FinalGrade).HasPrecision(5, 2);
            configuration.Property(o => o.AttendancePercentage).HasPrecision(5, 2);

            configuration.HasIndex(o => o.StudentId);
            configuration.HasIndex(o => new { o.StudentId, o.ModuleOfferingId }).IsUnique();
            configuration.HasIndex(o => new { o.ModuleOfferingId, o.Status });

            configuration.HasOne(o => o.ModuleOffering)
                .WithMany(o => o.Enrollments)
                .HasForeignKey(o => o.ModuleOfferingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
