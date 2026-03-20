using UnityEngine;

namespace UI
{
	public class BuyItemPayloadData
	{
		public Sprite Sprite;

		public string ItemId;

		public StoreItemType ItemType;

		public bool CanEquip;

		public BuyItemAdditionalPayloadData BuyItemAdditionalPayloadData;

		public int Index;

		public BuyItemPayloadData(Sprite sprite, string itemId, StoreItemType itemType, bool canEquip, BuyItemAdditionalPayloadData buyItemAdditionalPayloadData, int index)
		{
			Sprite = sprite;
			ItemId = itemId;
			ItemType = itemType;
			CanEquip = canEquip;
			BuyItemAdditionalPayloadData = buyItemAdditionalPayloadData;
			Index = index;
		}
	}
}
