namespace ConnectX.Domain.Entities;

public enum AssignmentType
{
    Unknown = 0,
    Preparation = 1,
    Individual = 2,
    Group = 3,
    Portfolio = 4,
    Reflection = 5,
    CaseStudy = 6,
    Research = 7,
    Supplementary = 8
}

public enum AssignmentStatus
{
    Draft = 0,
    Published = 1,
    Closed = 2,
    Archived = 3
}

public enum SubmissionStatus
{
    Draft = 0,
    Submitted = 1,
    Returned = 2,
    Graded = 3,
    ResubmissionRequested = 4,
    Late = 5
}

public enum AssessmentType
{
    Unknown = 0,
    WrittenExam = 1,
    OralExam = 2,
    PracticalExam = 3,
    Assignment = 4,
    Portfolio = 5,
    OnlineTest = 6
}

public enum TestQuestionType
{
    Unknown = 0,
    SingleChoice = 1,
    MultipleChoice = 2,
    TrueFalse = 3,
    ShortAnswer = 4,
    Essay = 5
}

