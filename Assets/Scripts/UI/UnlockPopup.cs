namespace UI
{
	public abstract class UnlockPopup : Screen
	{
		public Button BuyButton = new Button();

		public Button CancelButton = new Button();

		public Button BackgroundButton = new Button();

		public UnlockPopup(ScreenManager screenManager)
			: base(screenManager)
		{
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
