namespace UI
{
	public class BuyLocationPayloadData
	{
		public string LocationId;

		public int Price;

		public BuyLocationPayloadData(string locationId, int price)
		{
			LocationId = locationId;
			Price = price;
		}
	}
}
