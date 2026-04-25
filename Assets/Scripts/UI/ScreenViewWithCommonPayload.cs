namespace UI
{
	public abstract class ScreenViewWithCommonPayload<TScreen> : ScreenView<TScreen, CommonPayloadData> where TScreen : Screen
	{
		public override void PreShow(CommonPayloadData payload)
		{

		}

		public override void PostShow(CommonPayloadData payload)
		{
		}

		public override void SetSelectedGO()
		{
		}

		public override void Back()
		{
		}
	}
}
