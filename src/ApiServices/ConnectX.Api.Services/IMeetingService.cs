namespace ConnectX.Api.Services;

public interface IMeetingService
{
    Task<IReadOnlyList<StudentUpcomingEventModel>> GetAsync(Guid studentId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentUpcomingEventModel>> GetAsync(
        Guid studentId,
        DateTimeOffset startsAt,
        DateTimeOffset endsAt,
        CancellationToken cancellationToken = default);

    Task<StudentMeetingDetailModel?> GetAsync(
        Guid studentId,
        Guid meetingId,
        CancellationToken cancellationToken = default);
}

