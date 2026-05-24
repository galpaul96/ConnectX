namespace ConnectX.Web.Components.User;

public sealed class NotificationUpdateDispatcher
{
    public event EventHandler<NotificationCreatedEventArgs>? NotificationCreated;

    public void NotifyCreated(NotificationCreatedEventArgs args)
    {
        NotificationCreated?.Invoke(this, args);
    }
}

public sealed record NotificationCreatedEventArgs(
    string RecipientUserId,
    UserNotificationSummary Notification);
