namespace UI
{
	[View("BuyCoinsPopupView")]
	public class BuyCoinsPopup : Screen
	{
		public Button BuyButton = new Button();

		public Button CancelButton = new Button();

		public Button BackgroundButton = new Button();

		public Item CurrentItem;

		public BuyCoinsPopup(ScreenManager screenManager)
			: base(screenManager)
		{
			BuyButton.PressedAction = () =>
			{
				ScreenManager.ClosePopup();
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
			CancelButton.PressedAction = () =>
			{
				ScreenManager.ClosePopup();
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
			BackgroundButton.PressedAction = () =>
			{
				ScreenManager.ClosePopup();
			};

		}
	}
}
