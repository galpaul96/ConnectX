using ConnectX.Domain.Entities;

namespace ConnectX.Domain.Api.Modules;

public class StudentMeetingDetailResponse
{
    public StudentUpcomingEventResponse Summary { get; init; } = new();
    public string? PreparationInstructions { get; init; }
    public string? CancellationReason { get; init; }
    public IReadOnlyList<LearningObjectiveResponse> LearningObjectives { get; init; } = [];
    public IReadOnlyList<LearningActivityResponse> LearningActivities { get; init; } = [];
    public IReadOnlyList<AssignmentSummaryResponse> PreparationAssignments { get; init; } = [];
    public IReadOnlyList<ResourceSummaryResponse> Resources { get; init; } = [];
    public IReadOnlyList<LessonContentSummaryResponse> LessonContents { get; init; } = [];
    public IReadOnlyList<StudyTipSummaryResponse> StudyTips { get; init; } = [];
    public IReadOnlyList<LocationDirectionResponse> Directions { get; init; } = [];
}

public record LearningObjectiveResponse(
    Guid Id,
    Guid ExternalId,
    string Title,
    string Description,
    string? BloomLevel,
    bool IsAssessed);

public record LearningActivityResponse(
    Guid Id,
    Guid ExternalId,
    string Title,
    string? Description,
    LearningActivityType ActivityType,
    int? DurationMinutes,
    string? Instructions,
    bool IsRequired);

public record AssignmentSummaryResponse(
    Guid Id,
    Guid ExternalId,
    string Title,
    AssignmentType AssignmentType,
    AssignmentStatus Status,
    DateTimeOffset? DueAt,
    decimal? MaximumScore,
    decimal? WeightPercentage);

public record ResourceSummaryResponse(
    Guid Id,
    Guid ExternalId,
    string Title,
    string? Description,
    ResourceType ResourceType,
    string? Url,
    string? FileName,
    bool IsRequired,
    string? CitationText);

public record LessonContentSummaryResponse(
    Guid Id,
    Guid ExternalId,
    string Title,
    string? Summary,
    int? EstimatedStudyMinutes,
    bool IsRequired);

public record StudyTipSummaryResponse(
    Guid Id,
    Guid ExternalId,
    string Title,
    string Body,
    StudyTipCategory Category,
    bool IsHighlighted);

public record LocationDirectionResponse(
    TravelMode TravelMode,
    string Title,
    string Instructions,
    string? PublicTransportStop,
    string? ParkingInstructions,
    string? ExternalNavigationUrl);
