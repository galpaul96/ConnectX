using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class SupportRequest : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? ModuleOfferingId { get; set; }
    public ModuleOffering? ModuleOffering { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SupportRequestCategory Category { get; set; }
    public SupportRequestStatus Status { get; set; }
    public MessagePriority Priority { get; set; }
    public DateTimeOffset SubmittedAt { get; set; }
    public DateTimeOffset? LastResponseAt { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }
    public Guid? AssignedStaffId { get; set; }

    public List<SupportMessage> Messages { get; set; } = [];
}

