public class Item
{
    public string Id;

    public int Price;

    public string IconId;

    public string Description;

    public StoreItemType ItemType;

    public Item(string id, int price, string iconId, StoreItemType itemType, string description = null)
    {
        Id = id;
        Price = price;
        IconId = iconId;
        ItemType = itemType;
        Description = description;
    }
}
