using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Students
{
    internal class PlatformAccessLogEntityTypeConfiguration : IEntityTypeConfiguration<PlatformAccessLog>
    {
        public void Configure(EntityTypeBuilder<PlatformAccessLog> configuration)
        {
            configuration.ConfigureAuditedEntity("PlatformAccessLogs");

            configuration.Property(o => o.DeviceName).HasMaxLength(256);
            configuration.Property(o => o.BrowserName).HasMaxLength(128);
            configuration.Property(o => o.OperatingSystem).HasMaxLength(128);
            configuration.Property(o => o.IpAddress).HasMaxLength(64);
            configuration.Property(o => o.Country).HasMaxLength(128);

            configuration.HasIndex(o => new { o.StudentId, o.AccessedAt });
            configuration.HasIndex(o => new { o.StudentId, o.WasSuccessful });
        }
    }
}
