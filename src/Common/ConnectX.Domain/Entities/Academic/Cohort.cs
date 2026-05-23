using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class Cohort : Audit
{
    public Guid ExternalId { get; set; }
    public Guid StudyProgramId { get; set; }
    public StudyProgram? StudyProgram { get; set; }

    public string Name { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public DateOnly StartsOn { get; set; }
    public DateOnly? EndsOn { get; set; }
    public string? Location { get; set; }
    public DeliveryMode DeliveryMode { get; set; }
    public int? Capacity { get; set; }

    public List<ModuleOffering> ModuleOfferings { get; set; } = [];
}

