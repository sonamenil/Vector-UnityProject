using System.Xml;

public class ADConfig
{
	public int AR_MainChance
	{
		get;
		private set;
	}

	public int AR_InterstitialChance
	{
		get;
		private set;
	}

	public int AR_ToFull
	{
		get;
		private set;
	}

	public int IR_InterstitialChance
	{
		get;
		private set;
	}

	public int BR_BoostChance
	{
		get;
		private set;
	}

	public bool BR_Boost100AfterBoost
	{
		get;
		private set;
	}

	public void Parse(XmlNode node)
	{
	}
}
