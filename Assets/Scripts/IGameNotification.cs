using System;

public interface IGameNotification
{
	int? Id
	{
		get;
		set;
	}

	string Title
	{
		get;
		set;
	}

	string Body
	{
		get;
		set;
	}

	string Subtitle
	{
		get;
		set;
	}

	string Data
	{
		get;
		set;
	}

	string Group
	{
		get;
		set;
	}

	int? BadgeNumber
	{
		get;
		set;
	}

	bool ShouldAutoCancel
	{
		get;
		set;
	}

	DateTime? DeliveryTime
	{
		get;
		set;
	}

	bool Scheduled
	{
		get;
	}

	string SmallIcon
	{
		get;
		set;
	}

	string LargeIcon
	{
		get;
		set;
	}
}
