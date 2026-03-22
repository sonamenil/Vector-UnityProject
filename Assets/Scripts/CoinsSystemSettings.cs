using System.Collections.Generic;
using System.Xml;

public class CoinsSystemSettings
{
	private CoinsSystemType _type = CoinsSystemType.CST_DEFAULT;

	private Dictionary<CoinType, CoinsSettings> _settings = new Dictionary<CoinType, CoinsSettings>();

	public CoinsSystemType CoinsSystemType => _type;

	public CoinsSystemSettings(XmlNode node)
	{
		if (node.Attributes["Name"].Value == "CoinsAfterVideo")
		{
			_type = CoinsSystemType.CST_AFTER_VIDEO;
		}
		if (node.Attributes["Name"].Value == "CoinsAfterPush")
		{
			_type = CoinsSystemType.CST_AFTER_PUSH;
		}

		foreach (XmlNode node2 in node.ChildNodes)
		{
			CoinType coinType = CoinType.CT_DEFAULT;
			if (node2.Attributes["Type"].Value == "Gold")
			{
				coinType = CoinType.CT_GOLD;
			}
            if (node2.Attributes["Type"].Value == "Ruby")
            {
                coinType = CoinType.CT_BRONZE;
            }
			_settings[coinType] = new CoinsSettings(node2);
        }
	}

	public CoinsSystemSettings()
	{
		_settings[CoinType.CT_DEFAULT] = new CoinsSettings(1, 10, -1);
		_settings[CoinType.CT_GOLD] = new CoinsSettings(0.33f, 100, 1);
		_settings[CoinType.CT_BRONZE] = new CoinsSettings(0.01f, 300, 1);
	}

	public CoinsSettings GetCoinsSettings(CoinType coinType)
	{
		return _settings[coinType];
	}
}
