using System;
using System.Collections.Generic;
using System.Linq;
using Banzai.Json;
using Newtonsoft.Json;
using PlayerData;

public class ShopData : BaseUserHolder<ShopData>
{
	private Dictionary<string, OwnedItem> _items = new Dictionary<string, OwnedItem>();

	protected override string JSONName => "ShopData";

	public event Action<ShopData> Updated;

	public override void ParseData()
	{
		if (_userjObject["Items"] == null)
		{
			return;
		}
		var list = JsonConvert.DeserializeObject<List<OwnedItem>>(JsonUtils.AsString(_userjObject["Items"]));
		foreach (var item in list )
		{
			_items[item.id] = item;
		}
	}

	public override void SaveData()
	{
		_userjObject["Items"] = JsonConvert.SerializeObject(_items.Values.ToList());
		UserDataManager.Instance.SaveUserDate();
		Updated?.Invoke(this);
	}

	public void Add(string itemId, int count, bool equip = true, bool saveDataNow = true)
	{
		if (!_items.ContainsKey(itemId))
		{
			_items[itemId] = new OwnedItem(itemId, count, equip);
		}
		else
		{
			_items[itemId].AddCount(count, equip);
		}
		SaveData();
	}

	public int GetCount(string itemId)
	{
		if (!_items.ContainsKey(itemId))
		{
			return 0;
		}
		return _items[itemId].count;
	}

	public bool Contains(string itemId)
	{
		return _items.ContainsKey(itemId);
	}

	public bool IsBought(string itemId)
	{
        if (!_items.ContainsKey(itemId))
        {
            return false;
        }
        return _items[itemId].count > 0;
    }

	public bool IsEquipped(string itemId)
	{
        if (!_items.ContainsKey(itemId))
        {
            return false;
        }
        if (_items[itemId].count == 0)
        {
            return false;
        }
        return _items[itemId].isEquip;
    }

	public bool IsEquippedGadget(string gadgetID = "GADGET_FORCEBLASTER")
	{
        if (!_items.ContainsKey(gadgetID))
        {
            return false;
        }
		if (_items[gadgetID].count == 0)
		{
			return false;
		}
        return _items[gadgetID].isEquip;
    }

	public bool IsEquippedGadgetNotEmpty(string gadgetID = "GADGET_FORCEBLASTER")
	{
		if (IsEquipped(gadgetID))
		{
			return GetCount(gadgetID) > 0;
		}
		return false;
	}

	public int GetGadgetCount(string gadgetID = "GADGET_FORCEBLASTER")
	{
        if (!_items.ContainsKey(gadgetID))
        {
            return 0;
        }
        return _items[gadgetID].count;
    }

	public void Equip(string itemId)
	{
		_items[itemId].SetEquip(true);
		SaveData();
	}

	public void Unequip(string itemId)
	{
        _items[itemId].SetEquip(false);
        SaveData();
    }

	public void Consume(string itemId)
	{
		_items[itemId].Spend(1);
		SaveData();
	}

	public void Migrate(PlayerDataComponent playerData)
	{
		if (playerData == null)
		{
			return;
		}
		var shopData = playerData.User.ShopData;
		foreach (var item in shopData.Items.Keys)
		{
			var ownedItem = shopData.Items[item];
			_items[item] = new OwnedItem(item, (int)ownedItem.Item1, ownedItem.Item2);
		}
	}
}
