namespace ConnectX.Domain.Entities;

public enum CommunicationAudience
{
    Module = 0,
    StudyProgram = 1,
    Cohort = 2,
    IndividualStudent = 3,
    Teachers = 4
}

public enum AuthorRole
{
    Unknown = 0,
    Student = 1,
    Teacher = 2,
    NcoiStaff = 3,
    System = 4
}

public enum DiscussionStatus
{
    Draft = 0,
    Open = 1,
    Closed = 2,
    Archived = 3
}

public enum QuestionStatus
{
    Open = 0,
    Answered = 1,
    Resolved = 2,
    Closed = 3
}

public enum MessagePriority
{
    Low = 0,
    Normal = 1,
    High = 2,
    Urgent = 3
}

public enum SupportRequestCategory
{
    Unknown = 0,
    StudyInformation = 1,
    Enrollment = 2,
    PersonalDetails = 3,
    TechnicalSupport = 4,
    ExamInformation = 5,
    LearningMaterial = 6,
    Planning = 7
}

public enum SupportRequestStatus
{
    Open = 0,
    WaitingForStudent = 1,
    WaitingForConnectX = 2,
    Resolved = 3,
    Closed = 4
}

