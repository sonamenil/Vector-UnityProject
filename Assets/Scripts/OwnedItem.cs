using Newtonsoft.Json;
using System;

[Serializable]
public class OwnedItem
{
	[JsonProperty]
	public string id
	{
		get;
		private set;
	}

	[JsonProperty]
	public int count
	{
		get;
		private set;
	}

	[JsonProperty]
	public bool isEquip
	{
		get;
		private set;
	}

	[JsonConstructor]
	public OwnedItem(string id, int count, bool isEquip)
	{
		this.id = id;
		this.count = count;
		this.isEquip = isEquip;
	}

	public void AddCount(int countvalue, bool equip)
	{
		isEquip = equip;
		count += countvalue;
	}

	public void Spend(int value)
	{
		count -= value;
	}

	public void SetEquip(bool value)
	{
		isEquip = value;
	}
}
