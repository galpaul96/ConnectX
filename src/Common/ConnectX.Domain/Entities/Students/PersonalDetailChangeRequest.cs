using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class PersonalDetailChangeRequest : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? StudentPersonalFileId { get; set; }
    public StudentPersonalFile? StudentPersonalFile { get; set; }

    public string FieldName { get; set; } = string.Empty;
    public string? CurrentValue { get; set; }
    public string? RequestedValue { get; set; }
    public string? Reason { get; set; }
    public ChangeRequestStatus Status { get; set; }
    public DateTimeOffset SubmittedAt { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }
    public Guid? ReviewedByStaffId { get; set; }
    public string? ReviewerNotes { get; set; }
}

