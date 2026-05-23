using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class PlatformAccessLog : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }

    public DateTimeOffset AccessedAt { get; set; }
    public PlatformAccessDeviceType DeviceType { get; set; }
    public string? DeviceName { get; set; }
    public string? BrowserName { get; set; }
    public string? OperatingSystem { get; set; }
    public string? IpAddress { get; set; }
    public string? Country { get; set; }
    public bool WasSuccessful { get; set; }
    public string? FailureReason { get; set; }
}
