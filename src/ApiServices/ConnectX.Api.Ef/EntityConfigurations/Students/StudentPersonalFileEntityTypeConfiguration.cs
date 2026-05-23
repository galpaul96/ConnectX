using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Students
{
    internal class StudentPersonalFileEntityTypeConfiguration : IEntityTypeConfiguration<StudentPersonalFile>
    {
        public void Configure(EntityTypeBuilder<StudentPersonalFile> configuration)
        {
            configuration.ConfigureAuditedEntity("StudentPersonalFiles");

            configuration.Property(o => o.StudentNumber).HasMaxLength(64);
            configuration.Property(o => o.FirstName).HasMaxLength(128);
            configuration.Property(o => o.LastName).HasMaxLength(128);
            configuration.Property(o => o.Email).HasMaxLength(320);
            configuration.Property(o => o.PhoneNumber).HasMaxLength(64);
            configuration.Property(o => o.AddressLine1).HasMaxLength(256);
            configuration.Property(o => o.AddressLine2).HasMaxLength(256);
            configuration.Property(o => o.City).HasMaxLength(128);
            configuration.Property(o => o.PostalCode).HasMaxLength(32);
            configuration.Property(o => o.Country).HasMaxLength(128);
            configuration.Property(o => o.EmergencyContactName).HasMaxLength(256);
            configuration.Property(o => o.EmergencyContactPhone).HasMaxLength(64);

            configuration.HasIndex(o => o.StudentId).IsUnique();
            configuration.HasIndex(o => o.StudentNumber).IsUnique();
            configuration.HasIndex(o => o.Email);
            configuration.HasIndex(o => new { o.LastName, o.FirstName });
        }
    }
}
