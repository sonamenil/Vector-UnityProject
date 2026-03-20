namespace UI
{
	public class GadgetScreenPayloadData : CommonPayloadData
	{
		public bool BackToPrevious;

		public GadgetScreenPayloadData(bool backToPrevious)
		{
			BackToPrevious = backToPrevious;
		}
	}
}
