using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class BibliographicReference : Audit
{
    public Guid ExternalId { get; set; }
    public Guid ProgramModuleId { get; set; }
    public ProgramModule? ProgramModule { get; set; }
    public Guid? MeetingId { get; set; }
    public Meeting? Meeting { get; set; }

    public BibliographicReferenceType ReferenceType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Authors { get; set; }
    public string? Editor { get; set; }
    public string? Publisher { get; set; }
    public string? JournalName { get; set; }
    public string? Edition { get; set; }
    public string? Volume { get; set; }
    public string? Issue { get; set; }
    public string? PageRange { get; set; }
    public int? PublicationYear { get; set; }
    public string? Isbn { get; set; }
    public string? Issn { get; set; }
    public string? Doi { get; set; }
    public string? Url { get; set; }
    public string? CitationText { get; set; }
    public bool IsRequiredReading { get; set; }
    public int SortOrder { get; set; }

    public List<LearningResource> LearningResources { get; set; } = [];
}

