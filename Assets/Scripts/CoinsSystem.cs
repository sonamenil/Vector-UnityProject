using System.Collections.Generic;
using System.Xml;

public class CoinsSystem
{
	public static CoinsSystemType CurrentType;

	private Dictionary<CoinsSystemType, CoinsSystemSettings> _settings = new Dictionary<CoinsSystemType, CoinsSystemSettings>();

	public CoinsSystem(XmlNode node)
	{
		foreach	(XmlNode node2 in node)
		{
			var coinSettings = new CoinsSystemSettings(node2);
			_settings[coinSettings.CoinsSystemType] = coinSettings;
		}
	}

	public CoinsSystem()
	{
		_settings[CoinsSystemType.CST_DEFAULT] = new CoinsSystemSettings();
	}

	public CoinsSettings GetCoinsSettings(CoinsSystemType coinsSystemType, CoinType coinType)
	{
		return _settings[coinsSystemType].GetCoinsSettings(coinType);
	}
}
