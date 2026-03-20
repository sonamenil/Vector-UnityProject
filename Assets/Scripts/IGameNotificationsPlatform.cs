using System;

public interface IGameNotificationsPlatform
{
	event Action<IGameNotification> NotificationReceived;

	IGameNotification CreateNotification();

	void ScheduleNotification(IGameNotification gameNotification);

	void CancelNotification(int notificationId);

	void DismissNotification(int notificationId);

	void CancelAllScheduledNotifications();

	void DismissAllDisplayedNotifications();

	IGameNotification GetLastNotification();

	void OnForeground();

	void OnBackground();
}
public interface IGameNotificationsPlatform<TNotificationType> : IGameNotificationsPlatform where TNotificationType : IGameNotification
{
	new TNotificationType CreateNotification();

	void ScheduleNotification(TNotificationType notification);

	new TNotificationType GetLastNotification();
}
