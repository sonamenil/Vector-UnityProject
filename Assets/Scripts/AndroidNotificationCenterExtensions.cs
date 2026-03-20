using UnityEngine;

public class AndroidNotificationCenterExtensions
{
	private static bool s_Initialized;

	private static AndroidJavaObject s_AndroidNotificationExtensions;

	public static bool Initialize()
	{
		return false;
	}

	public static bool AreNotificationsEnabled()
	{
		return false;
	}

	public static bool AreNotificationsEnabled(string channelId)
	{
		return false;
	}
}
