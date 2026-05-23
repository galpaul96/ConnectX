using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class ModuleEnrollment : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid ModuleOfferingId { get; set; }
    public ModuleOffering? ModuleOffering { get; set; }

    public EnrollmentStatus Status { get; set; }
    public DateOnly EnrolledOn { get; set; }
    public DateOnly? CompletedOn { get; set; }
    public decimal? FinalGrade { get; set; }
    public decimal? AttendancePercentage { get; set; }
    public string? CompletionRemarks { get; set; }
}

