using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Locations
{
    internal class LocationDirectionEntityTypeConfiguration : IEntityTypeConfiguration<LocationDirection>
    {
        public void Configure(EntityTypeBuilder<LocationDirection> configuration)
        {
            configuration.ConfigureAuditedEntity("LocationDirections");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.PublicTransportStop).HasMaxLength(256);
            configuration.Property(o => o.ExternalNavigationUrl).HasMaxLength(1024);

            configuration.HasIndex(o => new { o.AcademicLocationId, o.SortOrder });
            configuration.HasIndex(o => new { o.AcademicLocationId, o.TravelMode });

            configuration.HasOne(o => o.AcademicLocation)
                .WithMany(o => o.Directions)
                .HasForeignKey(o => o.AcademicLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
