using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class AttendanceRecord : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid MeetingId { get; set; }
    public Meeting? Meeting { get; set; }

    public AttendanceStatus Status { get; set; }
    public DateTimeOffset? CheckedInAt { get; set; }
    public DateTimeOffset? CheckedOutAt { get; set; }
    public string? Notes { get; set; }
}

