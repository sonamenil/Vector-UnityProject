//using System;
//using Unity.Notifications.Android;

//public class AndroidGameNotification : IGameNotification
//{
//	private AndroidNotification internalNotification;

//	public AndroidNotification InternalNotification => default(AndroidNotification);

//	public int? Id
//	{
//		get;
//		set;
//	}

//	public string Title
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public string Body
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public string Subtitle
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public string Data
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public string Group
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public int? BadgeNumber
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public bool ShouldAutoCancel
//	{
//		get
//		{
//			return false;
//		}
//		set
//		{
//		}
//	}

//	public DateTime? DeliveryTime
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public string DeliveredChannel
//	{
//		get;
//		set;
//	}

//	public bool Scheduled
//	{
//		get;
//		private set;
//	}

//	public string SmallIcon
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public string LargeIcon
//	{
//		get
//		{
//			return null;
//		}
//		set
//		{
//		}
//	}

//	public AndroidGameNotification()
//	{
//	}

//	internal AndroidGameNotification(AndroidNotification deliveredNotification, int deliveredId, string deliveredChannel)
//	{
//	}

//	internal void OnScheduled()
//	{
//	}
//}
