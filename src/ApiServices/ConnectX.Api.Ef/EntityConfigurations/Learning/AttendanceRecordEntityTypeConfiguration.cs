using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class AttendanceRecordEntityTypeConfiguration : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> configuration)
        {
            configuration.ConfigureAuditedEntity("AttendanceRecords");

            configuration.HasIndex(o => o.StudentId);
            configuration.HasIndex(o => new { o.MeetingId, o.StudentId }).IsUnique();
            configuration.HasIndex(o => new { o.MeetingId, o.Status });

            configuration.HasOne(o => o.Meeting)
                .WithMany(o => o.AttendanceRecords)
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
