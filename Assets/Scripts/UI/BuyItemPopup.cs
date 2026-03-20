namespace UI
{
    [View("BuyItemPopupView")]
    public class BuyItemPopup : Screen
	{
		public Button BuyButton = new Button();

		public Button CancelButton = new Button();

		public Button BackgroundButton = new Button();

		public Item CurrentItem;

		public BuyItemPopup(ScreenManager screenManager)
			: base(screenManager)
		{
			BuyButton.PressedAction = () =>
			{
				ScreenManager.ClosePopup();
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
