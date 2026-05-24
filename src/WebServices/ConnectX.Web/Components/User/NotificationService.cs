using ConnectX.Domain.Entities;
using ConnectX.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace ConnectX.Web.Components.User;

public sealed class NotificationService(
    IServiceScopeFactory serviceScopeFactory,
    NotificationUpdateDispatcher updateDispatcher)
{
    private const int MaxPageSize = 50;
    private const int MaxPreviewSize = 5;
    private const int UpcomingWindowDays = 14;

    public async Task<NotificationHeaderSnapshot> GetHeaderSnapshotForUserAsync(string userId, DateTimeOffset now)
    {
        var startsAtLowerBound = now.ToUniversalTime();
        var startsAtUpperBound = startsAtLowerBound.AddDays(UpcomingWindowDays);
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var latestReceived = await dbContext.UserNotifications
            .AsNoTracking()
            .Where(notification => notification.RecipientUserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(MaxPreviewSize)
            .Select(notification => new UserNotificationSummary(
                notification.Id,
                notification.ExternalId,
                notification.RecipientUserId,
                notification.Title,
                notification.Body,
                notification.Priority,
                notification.Topic,
                notification.CreatedAt,
                notification.UserViewed,
                notification.EventStartsAt,
                notification.EventEndsAt,
                notification.ActionUrl,
                notification.SourceType,
                notification.SourceId))
            .ToListAsync();

        var upcoming = await dbContext.UserNotifications
            .AsNoTracking()
            .Where(notification =>
                notification.RecipientUserId == userId &&
                notification.EventStartsAt.HasValue &&
                notification.EventStartsAt >= startsAtLowerBound &&
                notification.EventStartsAt <= startsAtUpperBound)
            .OrderBy(notification => notification.EventStartsAt)
            .ThenByDescending(notification => notification.CreatedAt)
            .Take(MaxPreviewSize)
            .Select(notification => new UserNotificationSummary(
                notification.Id,
                notification.ExternalId,
                notification.RecipientUserId,
                notification.Title,
                notification.Body,
                notification.Priority,
                notification.Topic,
                notification.CreatedAt,
                notification.UserViewed,
                notification.EventStartsAt,
                notification.EventEndsAt,
                notification.ActionUrl,
                notification.SourceType,
                notification.SourceId))
            .ToListAsync();

        var unviewedCount = await dbContext.UserNotifications
            .AsNoTracking()
            .CountAsync(notification =>
                notification.RecipientUserId == userId &&
                !notification.UserViewed);

        return new NotificationHeaderSnapshot(latestReceived, upcoming, unviewedCount);
    }

    public async Task<IReadOnlyList<UserNotificationSummary>> GetLatestForUserAsync(string userId, int max = MaxPageSize)
    {
        var safeMax = Math.Clamp(max, 1, MaxPageSize);
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await dbContext.UserNotifications
            .AsNoTracking()
            .Where(notification => notification.RecipientUserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(safeMax)
            .Select(notification => new UserNotificationSummary(
                notification.Id,
                notification.ExternalId,
                notification.RecipientUserId,
                notification.Title,
                notification.Body,
                notification.Priority,
                notification.Topic,
                notification.CreatedAt,
                notification.UserViewed,
                notification.EventStartsAt,
                notification.EventEndsAt,
                notification.ActionUrl,
                notification.SourceType,
                notification.SourceId))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserNotificationSummary>> GetLatestReceivedForUserAsync(string userId, int max = MaxPreviewSize)
    {
        var safeMax = Math.Clamp(max, 1, MaxPreviewSize);
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await dbContext.UserNotifications
            .AsNoTracking()
            .Where(notification => notification.RecipientUserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(safeMax)
            .Select(notification => new UserNotificationSummary(
                notification.Id,
                notification.ExternalId,
                notification.RecipientUserId,
                notification.Title,
                notification.Body,
                notification.Priority,
                notification.Topic,
                notification.CreatedAt,
                notification.UserViewed,
                notification.EventStartsAt,
                notification.EventEndsAt,
                notification.ActionUrl,
                notification.SourceType,
                notification.SourceId))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserNotificationSummary>> GetClosestUpcomingForUserAsync(
        string userId,
        DateTimeOffset now,
        int max = MaxPreviewSize)
    {
        var safeMax = Math.Clamp(max, 1, MaxPreviewSize);
        var startsAtLowerBound = now.ToUniversalTime();
        var startsAtUpperBound = startsAtLowerBound.AddDays(UpcomingWindowDays);
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await dbContext.UserNotifications
            .AsNoTracking()
            .Where(notification =>
                notification.RecipientUserId == userId &&
                notification.EventStartsAt.HasValue &&
                notification.EventStartsAt >= startsAtLowerBound &&
                notification.EventStartsAt <= startsAtUpperBound)
            .OrderBy(notification => notification.EventStartsAt)
            .ThenByDescending(notification => notification.CreatedAt)
            .Take(safeMax)
            .Select(notification => new UserNotificationSummary(
                notification.Id,
                notification.ExternalId,
                notification.RecipientUserId,
                notification.Title,
                notification.Body,
                notification.Priority,
                notification.Topic,
                notification.CreatedAt,
                notification.UserViewed,
                notification.EventStartsAt,
                notification.EventEndsAt,
                notification.ActionUrl,
                notification.SourceType,
                notification.SourceId))
            .ToListAsync();
    }

    public async Task<int> GetUnviewedCountForUserAsync(string userId)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await dbContext.UserNotifications
            .AsNoTracking()
            .CountAsync(notification =>
                notification.RecipientUserId == userId &&
                !notification.UserViewed);
    }

    public async Task MarkViewedAsync(string userId, IReadOnlyCollection<Guid> notificationIds)
    {
        if (notificationIds.Count == 0)
        {
            return;
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var notifications = await dbContext.UserNotifications
            .Where(notification =>
                notification.RecipientUserId == userId &&
                !notification.UserViewed &&
                notificationIds.Contains(notification.Id))
            .ToListAsync();

        if (notifications.Count == 0)
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;
        foreach (var notification in notifications)
        {
            notification.UserViewed = true;
            notification.UpdatedDate = now.UtcDateTime;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<NotificationOperationResult> CreateForUserAsync(
        string recipientUserId,
        NotificationCreateRequest request,
        string? issuedByUserId)
    {
        if (string.IsNullOrWhiteSpace(recipientUserId))
        {
            return NotificationOperationResult.Failure("Select a user before sending a notification.");
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var recipientExists = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Id == recipientUserId);

        if (!recipientExists)
        {
            return NotificationOperationResult.Failure("Recipient user was not found.");
        }

        var title = NormalizeRequired(request.Title);
        if (title is null)
        {
            return NotificationOperationResult.Failure("Notification title is required.");
        }

        var body = NormalizeRequired(request.Body);
        if (body is null)
        {
            return NotificationOperationResult.Failure("Notification body is required.");
        }

        var eventStartsAt = request.EventStartsAt?.ToUniversalTime();
        var eventEndsAt = request.EventEndsAt?.ToUniversalTime();

        if (eventStartsAt.HasValue &&
            eventEndsAt.HasValue &&
            eventEndsAt.Value < eventStartsAt.Value)
        {
            return NotificationOperationResult.Failure("Event end must be after the event start.");
        }

        var actionUrl = NormalizeOptional(request.ActionUrl);
        if (actionUrl is not null && !IsAllowedActionUrl(actionUrl))
        {
            return NotificationOperationResult.Failure("Action URL must be relative or use HTTP/HTTPS.");
        }

        var now = DateTimeOffset.UtcNow;
        var notification = new UserNotification
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            RecipientUserId = recipientUserId,
            IssuerUserId = NormalizeOptional(issuedByUserId),
            Title = title,
            Body = body,
            Priority = request.Priority,
            Topic = NormalizeOptional(request.Topic),
            CreatedAt = now,
            UserViewed = false,
            EventStartsAt = eventStartsAt,
            EventEndsAt = eventEndsAt,
            ActionUrl = actionUrl,
            SourceType = NormalizeOptional(request.SourceType) ?? "AdminManual",
            SourceId = NormalizeOptional(request.SourceId),
            CreatedDate = now.UtcDateTime,
            UpdatedDate = now.UtcDateTime,
            IsDeleted = false
        };

        dbContext.UserNotifications.Add(notification);
        await dbContext.SaveChangesAsync();

        var summary = ToSummary(notification);
        updateDispatcher.NotifyCreated(new NotificationCreatedEventArgs(recipientUserId, summary));

        return NotificationOperationResult.Success("Notification sent.", summary);
    }

    private static UserNotificationSummary ToSummary(UserNotification notification)
    {
        return new UserNotificationSummary(
            notification.Id,
            notification.ExternalId,
            notification.RecipientUserId,
            notification.Title,
            notification.Body,
            notification.Priority,
            notification.Topic,
            notification.CreatedAt,
            notification.UserViewed,
            notification.EventStartsAt,
            notification.EventEndsAt,
            notification.ActionUrl,
            notification.SourceType,
            notification.SourceId);
    }

    private static string? NormalizeRequired(string? value)
    {
        var normalizedValue = value?.Trim();
        return string.IsNullOrWhiteSpace(normalizedValue) ? null : normalizedValue;
    }

    private static string? NormalizeOptional(string? value)
    {
        var normalizedValue = value?.Trim();
        return string.IsNullOrWhiteSpace(normalizedValue) ? null : normalizedValue;
    }

    private static bool IsAllowedActionUrl(string value)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out var absoluteUri))
        {
            return string.Equals(absoluteUri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(absoluteUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
        }

        return Uri.TryCreate(value, UriKind.Relative, out _) &&
            !value.StartsWith("//", StringComparison.Ordinal) &&
            !value.StartsWith('\\');
    }
}

public sealed record NotificationCreateRequest(
    string? Title,
    string? Body,
    MessagePriority Priority,
    string? Topic,
    DateTimeOffset? EventStartsAt,
    DateTimeOffset? EventEndsAt,
    string? ActionUrl,
    string? SourceType,
    string? SourceId);

public sealed record UserNotificationSummary(
    Guid Id,
    Guid ExternalId,
    string RecipientUserId,
    string Title,
    string Body,
    MessagePriority Priority,
    string? Topic,
    DateTimeOffset CreatedAt,
    bool UserViewed,
    DateTimeOffset? EventStartsAt,
    DateTimeOffset? EventEndsAt,
    string? ActionUrl,
    string? SourceType,
    string? SourceId);

public sealed record NotificationHeaderSnapshot(
    IReadOnlyList<UserNotificationSummary> LatestReceived,
    IReadOnlyList<UserNotificationSummary> Upcoming,
    int UnviewedCount);

public sealed record NotificationOperationResult(
    bool Succeeded,
    string Message,
    UserNotificationSummary? Notification)
{
    public static NotificationOperationResult Success(string message, UserNotificationSummary notification)
    {
        return new NotificationOperationResult(true, message, notification);
    }

    public static NotificationOperationResult Failure(string message)
    {
        return new NotificationOperationResult(false, message, null);
    }
}
