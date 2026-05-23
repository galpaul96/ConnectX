namespace ConnectX.Domain.Entities;

public enum ChangeRequestStatus
{
    Submitted = 0,
    InReview = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}

public enum PlatformAccessDeviceType
{
    Unknown = 0,
    Desktop = 1,
    Laptop = 2,
    Tablet = 3,
    Mobile = 4
}

