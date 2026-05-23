using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class Teacher : Audit
{
    public Guid ExternalId { get; set; }
    public string StaffNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Biography { get; set; }
    public string? ExpertiseArea { get; set; }
    public string? OfficeLocation { get; set; }
    public string? PreferredContactMethod { get; set; }
    public bool IsFreelance { get; set; }
    public bool IsActive { get; set; } = true;

    public List<ProgramModule> CoordinatedModules { get; set; } = [];
    public List<ModuleOffering> ModuleOfferings { get; set; } = [];
}

