using System;

public class Record
{
	public int Points;

	public int Stars;

	public TimeSpan TimeSpan;

	public Record()
	{

	}

	public Record(PlayerData.Record oldRecord)
	{
		Points = oldRecord.Points;
		Stars = oldRecord.Stars;
		TimeSpan = oldRecord.TimeSpan;
	}
}
