using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Academic
{
    internal class TeacherEntityTypeConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> configuration)
        {
            configuration.ConfigureAuditedEntity("Teachers");

            configuration.Property(o => o.StaffNumber).HasMaxLength(64);
            configuration.Property(o => o.FirstName).HasMaxLength(128);
            configuration.Property(o => o.LastName).HasMaxLength(128);
            configuration.Property(o => o.DisplayName).HasMaxLength(256);
            configuration.Property(o => o.Email).HasMaxLength(320);
            configuration.Property(o => o.PhoneNumber).HasMaxLength(64);
            configuration.Property(o => o.ExpertiseArea).HasMaxLength(256);
            configuration.Property(o => o.OfficeLocation).HasMaxLength(256);
            configuration.Property(o => o.PreferredContactMethod).HasMaxLength(128);

            configuration.HasIndex(o => o.StaffNumber).IsUnique();
            configuration.HasIndex(o => o.Email);
            configuration.HasIndex(o => new { o.IsActive, o.LastName });
        }
    }
}
