namespace ConnectX.Domain.Entities;

public enum ResourceType
{
    Unknown = 0,
    Article = 1,
    BookChapter = 2,
    Video = 3,
    Link = 4,
    SlideDeck = 5,
    Document = 6,
    InteractiveApplication = 7,
    PracticeTest = 8,
    BackgroundInformation = 9,
    Book = 10,
    JournalArticle = 11,
    StudyTip = 12
}

public enum DocumentType
{
    Unknown = 0,
    ExaminationRegulation = 1,
    StudyGuide = 2,
    Syllabus = 3,
    Slides = 4,
    Template = 5,
    Policy = 6,
    Form = 7
}

public enum ContentVisibility
{
    Private = 0,
    ModuleParticipants = 1,
    StudyProgramParticipants = 2,
    TeachersOnly = 3,
    Public = 4
}

public enum BibliographicReferenceType
{
    Unknown = 0,
    Book = 1,
    BookChapter = 2,
    JournalArticle = 3,
    WebArticle = 4,
    Report = 5,
    CaseLaw = 6,
    Standard = 7
}

