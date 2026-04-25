namespace UI
{
	public abstract class ScreenViewWithAlertPayload<TScreen> : ScreenView<TScreen, AlertPayloadData> where TScreen : Screen
	{
		public override void PreShow(AlertPayloadData payload)
		{
		}

		public override void PostShow(AlertPayloadData payload)
		{
		}

		public override void SetSelectedGO()
		{
		}
	}
}
