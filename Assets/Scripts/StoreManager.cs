using Core._Common;
using System.Collections.Generic;
using System.Xml;

public class StoreManager : AbstractManager<StoreManager>
{
	private Dictionary<StoreItemType, List<Item>> _items;

	private static string FilePath => "Shop_payed.xml";

	protected override void InitInternal()
	{
		_items = new Dictionary<StoreItemType, List<Item>>();
		_items[StoreItemType.Gear] = new List<Item>();
        _items[StoreItemType.Tricks] = new List<Item>();
        _items[StoreItemType.Gadgets] = new List<Item>();
		var doc = XmlUtils.OpenXMLDocument(VectorPaths.Commons, FilePath);
		foreach (XmlNode group in doc["Shop"])
		{
			switch (group.Attributes["Name"].Value)
			{
				case "TRICK":
					foreach (XmlNode trick in group)
					{
						var name = trick.Attributes["Name"].Value;
						int price = int.Parse(trick.Attributes["Price"].Value);
						var iconid = trick.Attributes["ShopImage"].Value;
						_items[StoreItemType.Tricks].Add(new Item(name, price, iconid, StoreItemType.Tricks));
					}
					break;
				case "GADGETS":
                    foreach (XmlNode gadget in group)
                    {
                        var name = gadget.Attributes["Name"].Value;
                        int price = int.Parse(gadget.Attributes["Price"].Value);
                        var iconid = gadget.Attributes["ShopImage"].Value;
                        _items[StoreItemType.Gadgets].Add(new Item(name, price, iconid, StoreItemType.Gadgets));
                    }
                    break;
                case "CLOTHING":
                    foreach (XmlNode gear in group)
                    {
                        var name = gear.Attributes["Name"].Value;
                        int price = int.Parse(gear.Attributes["Price"].Value);
                        var iconid = gear.Attributes["ShopImage"].Value;
                        _items[StoreItemType.Gear].Add(new Item(name, price, iconid, StoreItemType.Gear));
                    }
                    break;
            }
		}
    }

    public List<Item> GetItems(StoreItemType type)
	{
		return _items[type];
	}
}
