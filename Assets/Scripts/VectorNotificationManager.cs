using System;

public class VectorNotificationManager : AbstractManager<VectorNotificationManager>
{
	protected override void InitInternal()
	{
	}

	private void OnNotificationReceived(IGameNotification deliveredNotification)
	{
	}

	public void SendNotification(string title, string body, string data, DateTime deliveryTime, int? badgeNumber = default(int?))
	{
	}
}
