using System.Collections.Generic;
using System.Xml;

public class MarmaladeMigration
{
	private static string _filename;

	private static string _folder;

	private XmlDocument _document;

	public static void Clear()
	{
	}

	public static bool HasOldData()
	{
		return false;
	}

	public void GetOptions(Options options)
	{
	}

	public void GetMainData(MainData mainData)
	{
	}

	public void GetGameStats(GameStats gameStats)
	{
	}

	public void GetShopData(ShopData shopData)
	{
	}

	public void GetStats(Stats stats)
	{
	}

	public Dictionary<string, string> GetMisc()
	{
		return null;
	}
}
