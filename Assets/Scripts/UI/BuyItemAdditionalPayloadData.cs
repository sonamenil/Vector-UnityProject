namespace UI
{
	public class BuyItemAdditionalPayloadData
	{
		public string Description;

		public bool MultiplePurchase;

		public BuyItemAdditionalPayloadData(string description, bool multiplePurchase)
		{
			Description = description;
			MultiplePurchase = multiplePurchase;
		}
	}
}
