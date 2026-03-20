using System;

public class ATTManager
{
	private delegate void ATTrackingManagerAuthorizationStatusDelegate(int status);

	private static ATTrackingManagerAuthorizationStatus _status;

	private static Action<ATTrackingManagerAuthorizationStatus> _onStatusRecived;

	public static void GetATTStatus(Action<ATTrackingManagerAuthorizationStatus> callback)
	{
	}

	private static void RequestATTAuthorization()
	{
	}

	private static void OnATTrackingAuthorizationRequested(int status)
	{
	}
}
