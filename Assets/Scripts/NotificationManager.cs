using System;
using System.Collections.Generic;

public class NotificationManager : AbstractManager<NotificationManager>
{
	private const string ANDROID_CHANEL_ID = "game_channel";

	private static readonly TimeSpan MinimumNotificationTime;

	private bool inForeground;

	public IGameNotificationsPlatform Platform
	{
		get;
		private set;
	}

	public List<PendingNotification> PendingNotifications
	{
		get;
		private set;
	}

	public event Action<IGameNotification> NotificationReceived;

	protected override void InitInternal()
	{
	}

	private void OnNotificationReceived(IGameNotification deliveredNotification)
	{
	}

	public IGameNotification CreateNotification()
	{
		return null;
	}

	public PendingNotification ScheduleNotification(IGameNotification notification)
	{
		return null;
	}

	public void CancelAllNotifications()
	{
	}

	public void CancelNotification(int notificationId)
	{
	}

	private void OnForegrounding()
	{
	}

	protected void OnApplicationFocus(bool hasFocus)
	{
	}

	private void SchedulePendingNotification()
	{
	}
}
