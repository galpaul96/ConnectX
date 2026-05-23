using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class InteractiveApplication : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string LaunchUrl { get; set; } = string.Empty;
    public string? ProviderName { get; set; }
    public bool OpensInNewWindow { get; set; }
    public bool RequiresAuthentication { get; set; }
    public bool TracksProgress { get; set; }
}

