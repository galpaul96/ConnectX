using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class AcademicLocation : Audit
{
    public Guid ExternalId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public LocationType LocationType { get; set; }
    public string? Description { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? RoomNumber { get; set; }
    public string? BuildingName { get; set; }
    public string? ReceptionPhoneNumber { get; set; }
    public string? MapUrl { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsAccessible { get; set; }
    public bool IsActive { get; set; } = true;

    public List<LocationDirection> Directions { get; set; } = [];
    public List<Meeting> Meetings { get; set; } = [];
    public List<ModuleOffering> ModuleOfferings { get; set; } = [];
}

