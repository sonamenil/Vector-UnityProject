namespace UI
{
    [View("BasePopupView")]
    public class BasePopup : Screen
	{
		public Button CreditsButton = new Button();

		public Button CancelButton = new Button();

		public Button BackgroundButton = new Button();

		public BasePopup(ScreenManager screenManager)
			: base(screenManager)
		{
			CreditsButton.PressedAction = () =>
			{
				ScreenManager.Show<CreditsScreen>(true, false);
			};
			CancelButton.PressedAction = () =>
			{
				ScreenManager.ClosePopup();
			};
			BackgroundButton.PressedAction = () =>
			{
				ScreenManager.ClosePopup();
			};

		}
	}
}
