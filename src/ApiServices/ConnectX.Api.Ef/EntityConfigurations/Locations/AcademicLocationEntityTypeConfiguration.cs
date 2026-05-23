using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Locations
{
    internal class AcademicLocationEntityTypeConfiguration : IEntityTypeConfiguration<AcademicLocation>
    {
        public void Configure(EntityTypeBuilder<AcademicLocation> configuration)
        {
            configuration.ConfigureAuditedEntity("AcademicLocations");

            configuration.Property(o => o.Code).HasMaxLength(64);
            configuration.Property(o => o.Name).HasMaxLength(256);
            configuration.Property(o => o.AddressLine1).HasMaxLength(256);
            configuration.Property(o => o.AddressLine2).HasMaxLength(256);
            configuration.Property(o => o.City).HasMaxLength(128);
            configuration.Property(o => o.PostalCode).HasMaxLength(32);
            configuration.Property(o => o.Country).HasMaxLength(128);
            configuration.Property(o => o.RoomNumber).HasMaxLength(64);
            configuration.Property(o => o.BuildingName).HasMaxLength(256);
            configuration.Property(o => o.ReceptionPhoneNumber).HasMaxLength(64);
            configuration.Property(o => o.MapUrl).HasMaxLength(1024);
            configuration.Property(o => o.Latitude).HasPrecision(9, 6);
            configuration.Property(o => o.Longitude).HasPrecision(9, 6);

            configuration.HasIndex(o => o.Code).IsUnique();
            configuration.HasIndex(o => new { o.IsActive, o.City });
            configuration.HasIndex(o => new { o.Latitude, o.Longitude });
        }
    }
}
