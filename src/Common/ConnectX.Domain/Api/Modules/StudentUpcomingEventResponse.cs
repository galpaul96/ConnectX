using ConnectX.Domain.Entities;

namespace ConnectX.Domain.Api.Modules;

public enum StudentUpcomingEventKind
{
    Meeting = 0,
    Assignment = 1,
    Exam = 2
}

public class StudentUpcomingEventResponse
{
    public Guid Id { get; init; }
    public Guid ExternalId { get; init; }
    public StudentUpcomingEventKind EventType { get; init; }
    public DateTimeOffset StartsAt { get; init; }
    public DateTimeOffset? EndsAt { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? ModuleId { get; init; }
    public string? ModuleCode { get; init; }
    public string? ModuleName { get; init; }
    public Guid? ModuleOfferingId { get; init; }
    public Guid? MeetingId { get; init; }
    public string? MeetingTitle { get; init; }
    public Guid? TeacherId { get; init; }
    public string? TeacherName { get; init; }
    public string? TeacherEmail { get; init; }
    public string? LocationName { get; init; }
    public string? LocationAddress { get; init; }
    public string? OnlineUrl { get; init; }
    public MeetingFormat? MeetingFormat { get; init; }
    public AssessmentType? AssessmentType { get; init; }
    public AssignmentType? AssignmentType { get; init; }
    public AssignmentStatus? AssignmentStatus { get; init; }
    public AttendanceStatus? AttendanceStatus { get; init; }
    public bool IsCancelled { get; init; }
    public bool IsRequired { get; init; }
    public decimal? MaximumScore { get; init; }
    public decimal? WeightPercentage { get; init; }
    public string? ResultGrade { get; init; }
    public decimal? ResultScore { get; init; }
    public bool? ResultPassed { get; init; }
    public int RelatedItemCount { get; init; }
}
