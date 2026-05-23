namespace ConnectX.Domain.Entities;

public enum AcademicLevel
{
    Unknown = 0,
    Certificate = 1,
    Associate = 2,
    Bachelor = 3,
    Master = 4,
    Professional = 5
}

public enum DeliveryMode
{
    Unknown = 0,
    InPerson = 1,
    Online = 2,
    Hybrid = 3,
    SelfPaced = 4
}

public enum EnrollmentStatus
{
    Draft = 0,
    Active = 1,
    Suspended = 2,
    Completed = 3,
    Withdrawn = 4
}

public enum MeetingFormat
{
    Unknown = 0,
    Lecture = 1,
    Workshop = 2,
    Seminar = 3,
    Lab = 4,
    Coaching = 5,
    Exam = 6
}

public enum AttendanceStatus
{
    Unknown = 0,
    Present = 1,
    Absent = 2,
    Late = 3,
    Excused = 4
}

