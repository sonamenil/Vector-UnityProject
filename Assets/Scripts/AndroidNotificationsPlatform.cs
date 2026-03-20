//using System;
//using System.Runtime.CompilerServices;
//using Unity.Notifications.Android;

//public class AndroidNotificationsPlatform : IGameNotificationsPlatform<AndroidGameNotification>, IGameNotificationsPlatform, IDisposable
//{
//	public string DefaultChannelId
//	{
//		get;
//		set;
//	}

//	public event Action<IGameNotification> NotificationReceived;

//	public void ScheduleNotification(AndroidGameNotification gameNotification)
//	{
//	}

//	public void ScheduleNotification(IGameNotification gameNotification)
//	{
//	}

//	public AndroidGameNotification CreateNotification()
//	{
//		return null;
//	}

//	IGameNotification IGameNotificationsPlatform.CreateNotification()
//	{
//		return null;
//	}

//	public void CancelNotification(int notificationId)
//	{
//	}

//	public void DismissNotification(int notificationId)
//	{
//	}

//	public void CancelAllScheduledNotifications()
//	{
//	}

//	public void DismissAllDisplayedNotifications()
//	{
//	}

//	IGameNotification IGameNotificationsPlatform.GetLastNotification()
//	{
//		return null;
//	}

//	public AndroidGameNotification GetLastNotification()
//	{
//		return null;
//	}

//	public void OnForeground()
//	{
//	}

//	public void OnBackground()
//	{
//	}

//	public void Dispose()
//	{
//	}

//	private void OnLocalNotificationReceived(AndroidNotificationIntentData data)
//	{
//	}
//}
