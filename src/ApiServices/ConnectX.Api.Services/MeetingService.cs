using ConnectX.Api.Ef;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConnectX.Api.Services;

internal class MeetingService : IMeetingService
{
    private static readonly TimeSpan DefaultLookAhead = TimeSpan.FromDays(90);

    private readonly IRepository _repository;

    public MeetingService(IRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<StudentUpcomingEventModel>> GetAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var startsAt = DateTimeOffset.UtcNow;
        var endsAt = startsAt.Add(DefaultLookAhead);

        return GetAsync(studentId, startsAt, endsAt, cancellationToken);
    }

    public async Task<IReadOnlyList<StudentUpcomingEventModel>> GetAsync(
        Guid studentId,
        DateTimeOffset startsAt,
        DateTimeOffset endsAt,
        CancellationToken cancellationToken = default)
    {
        var enrollments = await GetActiveEnrollmentsAsync(studentId, cancellationToken);

        if (enrollments.Count == 0)
        {
            return [];
        }

        var moduleOfferingIds = enrollments.Select(x => x.ModuleOfferingId).Distinct().ToArray();
        var programModuleIds = enrollments.Select(x => x.ModuleOffering!.ProgramModuleId).Distinct().ToArray();

        var meetings = await GetMeetingEventsAsync(studentId, moduleOfferingIds, startsAt, endsAt, cancellationToken);
        var assignments = await GetAssignmentEventsAsync(programModuleIds, startsAt, endsAt, cancellationToken);
        var exams = await GetExamEventsAsync(studentId, programModuleIds, startsAt, endsAt, cancellationToken);

        return meetings
            .Concat(assignments)
            .Concat(exams)
            .OrderBy(x => x.StartsAt)
            .ThenBy(x => x.Title)
            .ToList();
    }

    public async Task<StudentMeetingDetailModel?> GetAsync(
        Guid studentId,
        Guid meetingId,
        CancellationToken cancellationToken = default)
    {
        var meeting = await _repository.GetAllAsync<Meeting>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ModuleOffering)
                .ThenInclude(x => x!.ProgramModule)
            .Include(x => x.ModuleOffering)
                .ThenInclude(x => x!.Teacher)
            .Include(x => x.AcademicLocation)
                .ThenInclude(x => x!.Directions)
            .Include(x => x.LearningObjectives)
            .Include(x => x.LearningActivities)
            .Include(x => x.PreparationAssignments)
            .Include(x => x.Resources)
                .ThenInclude(x => x.BibliographicReference)
            .Include(x => x.LessonContents)
            .Include(x => x.StudyTips)
            .Include(x => x.AttendanceRecords.Where(y => y.StudentId == studentId))
            .Where(x => x.Id == meetingId)
            .Where(x => x.ModuleOffering != null && x.ModuleOffering.Enrollments.Any(y => y.StudentId == studentId && y.Status == EnrollmentStatus.Active))
            .FirstOrDefaultAsync(cancellationToken);

        return meeting is null ? null : MapMeetingDetail(meeting);
    }

    private async Task<List<ModuleEnrollment>> GetActiveEnrollmentsAsync(Guid studentId, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync<ModuleEnrollment>()
            .AsNoTracking()
            .Include(x => x.ModuleOffering)
                .ThenInclude(x => x!.ProgramModule)
            .Where(x => x.StudentId == studentId && x.Status == EnrollmentStatus.Active)
            .ToListAsync(cancellationToken);
    }

    private async Task<List<StudentUpcomingEventModel>> GetMeetingEventsAsync(
        Guid studentId,
        IReadOnlyCollection<Guid> moduleOfferingIds,
        DateTimeOffset startsAt,
        DateTimeOffset endsAt,
        CancellationToken cancellationToken)
    {
        var meetings = await _repository.GetAllAsync<Meeting>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ModuleOffering)
                .ThenInclude(x => x!.ProgramModule)
            .Include(x => x.ModuleOffering)
                .ThenInclude(x => x!.Teacher)
            .Include(x => x.AcademicLocation)
            .Include(x => x.PreparationAssignments)
            .Include(x => x.AttendanceRecords.Where(y => y.StudentId == studentId))
            .Where(x => moduleOfferingIds.Contains(x.ModuleOfferingId))
            .Where(x => x.StartsAt >= startsAt && x.StartsAt <= endsAt)
            .ToListAsync(cancellationToken);

        return meetings.Select(MapMeetingEvent).ToList();
    }

    private async Task<List<StudentUpcomingEventModel>> GetAssignmentEventsAsync(
        IReadOnlyCollection<Guid> programModuleIds,
        DateTimeOffset startsAt,
        DateTimeOffset endsAt,
        CancellationToken cancellationToken)
    {
        var assignments = await _repository.GetAllAsync<Assignment>()
            .AsNoTracking()
            .Include(x => x.ProgramModule)
            .Include(x => x.Meeting)
            .Where(x => programModuleIds.Contains(x.ProgramModuleId))
            .Where(x => x.Status == AssignmentStatus.Published)
            .Where(x => x.DueAt.HasValue && x.DueAt.Value >= startsAt && x.DueAt.Value <= endsAt)
            .ToListAsync(cancellationToken);

        return assignments.Select(MapAssignmentEvent).ToList();
    }

    private async Task<List<StudentUpcomingEventModel>> GetExamEventsAsync(
        Guid studentId,
        IReadOnlyCollection<Guid> programModuleIds,
        DateTimeOffset startsAt,
        DateTimeOffset endsAt,
        CancellationToken cancellationToken)
    {
        var exams = await _repository.GetAllAsync<Exam>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ProgramModule)
            .Include(x => x.Results.Where(y => y.StudentId == studentId))
            .Where(x => programModuleIds.Contains(x.ProgramModuleId))
            .Where(x => x.ScheduledAt.HasValue && x.ScheduledAt.Value >= startsAt && x.ScheduledAt.Value <= endsAt)
            .ToListAsync(cancellationToken);

        return exams.Select(MapExamEvent).ToList();
    }

    private static StudentUpcomingEventModel MapMeetingEvent(Meeting meeting)
    {
        var offering = meeting.ModuleOffering;
        var module = offering?.ProgramModule;
        var teacher = offering?.Teacher;
        var attendance = meeting.AttendanceRecords.FirstOrDefault();

        return new StudentUpcomingEventModel
        {
            Id = meeting.Id,
            ExternalId = meeting.ExternalId,
            EventType = StudentUpcomingEventType.Meeting,
            StartsAt = meeting.StartsAt,
            EndsAt = meeting.EndsAt,
            Title = meeting.Title,
            Description = meeting.Description,
            ModuleId = module?.Id,
            ModuleCode = module?.Code,
            ModuleName = module?.Name,
            ModuleOfferingId = meeting.ModuleOfferingId,
            TeacherId = teacher?.Id,
            TeacherName = teacher?.DisplayName,
            TeacherEmail = teacher?.Email,
            LocationName = meeting.AcademicLocation?.Name ?? meeting.Location,
            LocationAddress = FormatAddress(meeting.AcademicLocation),
            OnlineUrl = meeting.OnlineMeetingUrl ?? offering?.OnlineClassroomUrl,
            MeetingFormat = meeting.Format,
            IsCancelled = meeting.IsCancelled,
            AttendanceStatus = attendance?.Status,
            RelatedItemCount = meeting.PreparationAssignments.Count
        };
    }

    private static StudentUpcomingEventModel MapAssignmentEvent(Assignment assignment)
    {
        var dueAt = assignment.DueAt!.Value;

        return new StudentUpcomingEventModel
        {
            Id = assignment.Id,
            ExternalId = assignment.ExternalId,
            EventType = StudentUpcomingEventType.Assignment,
            StartsAt = dueAt,
            EndsAt = dueAt,
            Title = assignment.Title,
            Description = assignment.Instructions,
            ModuleId = assignment.ProgramModuleId,
            ModuleCode = assignment.ProgramModule?.Code,
            ModuleName = assignment.ProgramModule?.Name,
            MeetingId = assignment.MeetingId,
            MeetingTitle = assignment.Meeting?.Title,
            AssignmentType = assignment.AssignmentType,
            AssignmentStatus = assignment.Status,
            MaximumScore = assignment.MaximumScore,
            WeightPercentage = assignment.WeightPercentage,
            IsRequired = assignment.IsPreparationRequired
        };
    }

    private static StudentUpcomingEventModel MapExamEvent(Exam exam)
    {
        var scheduledAt = exam.ScheduledAt!.Value;
        var result = exam.Results.FirstOrDefault();

        return new StudentUpcomingEventModel
        {
            Id = exam.Id,
            ExternalId = exam.ExternalId,
            EventType = StudentUpcomingEventType.Exam,
            StartsAt = scheduledAt,
            EndsAt = exam.DurationMinutes.HasValue ? scheduledAt.AddMinutes(exam.DurationMinutes.Value) : scheduledAt,
            Title = exam.Title,
            Description = exam.Instructions,
            ModuleId = exam.ProgramModuleId,
            ModuleCode = exam.ProgramModule?.Code,
            ModuleName = exam.ProgramModule?.Name,
            LocationName = exam.Location,
            OnlineUrl = exam.OnlineExamUrl,
            AssessmentType = exam.AssessmentType,
            MaximumScore = exam.PassingScore,
            WeightPercentage = exam.WeightPercentage,
            ResultGrade = result?.Grade,
            ResultScore = result?.Score,
            ResultPassed = result?.Passed
        };
    }

    private static StudentMeetingDetailModel MapMeetingDetail(Meeting meeting)
    {
        var summary = MapMeetingEvent(meeting);

        return new StudentMeetingDetailModel
        {
            Summary = summary,
            PreparationInstructions = meeting.PreparationInstructions,
            CancellationReason = meeting.CancellationReason,
            LearningObjectives = meeting.LearningObjectives
                .OrderBy(x => x.SortOrder)
                .Select(x => new LearningObjectiveModel(x.Id, x.ExternalId, x.Title, x.Description, x.BloomLevel, x.IsAssessed))
                .ToList(),
            LearningActivities = meeting.LearningActivities
                .OrderBy(x => x.SortOrder)
                .Select(x => new LearningActivityModel(x.Id, x.ExternalId, x.Title, x.Description, x.ActivityType, x.DurationMinutes, x.Instructions, x.IsRequired))
                .ToList(),
            PreparationAssignments = meeting.PreparationAssignments
                .OrderBy(x => x.DueAt)
                .Select(x => new AssignmentSummaryModel(x.Id, x.ExternalId, x.Title, x.AssignmentType, x.Status, x.DueAt, x.MaximumScore, x.WeightPercentage))
                .ToList(),
            Resources = meeting.Resources
                .OrderBy(x => x.SortOrder)
                .Select(x => new ResourceSummaryModel(
                    x.Id,
                    x.ExternalId,
                    x.Title,
                    x.Description,
                    x.ResourceType,
                    x.Url,
                    x.FileName,
                    x.IsRequired,
                    x.BibliographicReference?.CitationText))
                .ToList(),
            LessonContents = meeting.LessonContents
                .OrderBy(x => x.SortOrder)
                .Select(x => new LessonContentSummaryModel(x.Id, x.ExternalId, x.Title, x.Summary, x.EstimatedStudyMinutes, x.IsRequired))
                .ToList(),
            StudyTips = meeting.StudyTips
                .OrderBy(x => x.SortOrder)
                .Select(x => new StudyTipSummaryModel(x.Id, x.ExternalId, x.Title, x.Body, x.Category, x.IsHighlighted))
                .ToList(),
            Directions = meeting.AcademicLocation?.Directions
                .OrderBy(x => x.SortOrder)
                .Select(x => new LocationDirectionModel(x.TravelMode, x.Title, x.Instructions, x.PublicTransportStop, x.ParkingInstructions, x.ExternalNavigationUrl))
                .ToList() ?? []
        };
    }

    private static string? FormatAddress(AcademicLocation? location)
    {
        if (location is null)
        {
            return null;
        }

        var parts = new[]
        {
            location.AddressLine1,
            location.AddressLine2,
            location.PostalCode,
            location.City,
            location.Country
        };

        var address = string.Join(", ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));

        return string.IsNullOrWhiteSpace(address) ? null : address;
    }
}

public enum StudentUpcomingEventType
{
    Meeting = 0,
    Assignment = 1,
    Exam = 2
}

public class StudentUpcomingEventModel
{
    public Guid Id { get; init; }
    public Guid ExternalId { get; init; }
    public StudentUpcomingEventType EventType { get; init; }
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

public class StudentMeetingDetailModel
{
    public StudentUpcomingEventModel Summary { get; init; } = new();
    public string? PreparationInstructions { get; init; }
    public string? CancellationReason { get; init; }
    public IReadOnlyList<LearningObjectiveModel> LearningObjectives { get; init; } = [];
    public IReadOnlyList<LearningActivityModel> LearningActivities { get; init; } = [];
    public IReadOnlyList<AssignmentSummaryModel> PreparationAssignments { get; init; } = [];
    public IReadOnlyList<ResourceSummaryModel> Resources { get; init; } = [];
    public IReadOnlyList<LessonContentSummaryModel> LessonContents { get; init; } = [];
    public IReadOnlyList<StudyTipSummaryModel> StudyTips { get; init; } = [];
    public IReadOnlyList<LocationDirectionModel> Directions { get; init; } = [];
}

public record LearningObjectiveModel(
    Guid Id,
    Guid ExternalId,
    string Title,
    string Description,
    string? BloomLevel,
    bool IsAssessed);

public record LearningActivityModel(
    Guid Id,
    Guid ExternalId,
    string Title,
    string? Description,
    LearningActivityType ActivityType,
    int? DurationMinutes,
    string? Instructions,
    bool IsRequired);

public record AssignmentSummaryModel(
    Guid Id,
    Guid ExternalId,
    string Title,
    AssignmentType AssignmentType,
    AssignmentStatus Status,
    DateTimeOffset? DueAt,
    decimal? MaximumScore,
    decimal? WeightPercentage);

public record ResourceSummaryModel(
    Guid Id,
    Guid ExternalId,
    string Title,
    string? Description,
    ResourceType ResourceType,
    string? Url,
    string? FileName,
    bool IsRequired,
    string? CitationText);

public record LessonContentSummaryModel(
    Guid Id,
    Guid ExternalId,
    string Title,
    string? Summary,
    int? EstimatedStudyMinutes,
    bool IsRequired);

public record StudyTipSummaryModel(
    Guid Id,
    Guid ExternalId,
    string Title,
    string Body,
    StudyTipCategory Category,
    bool IsHighlighted);

public record LocationDirectionModel(
    TravelMode TravelMode,
    string Title,
    string Instructions,
    string? PublicTransportStop,
    string? ParkingInstructions,
    string? ExternalNavigationUrl);
