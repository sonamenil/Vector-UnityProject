namespace UI
{
    [View("StoreGearScreenView")]
    public class StoreGearScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public Button BuyCoinsButton = new Button();

		public Button BuyButton = new Button();

		public Button ShowInfoButton = new Button();

		public StoreGearScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction = () =>
			{
				ScreenManager.Show<StoreScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
			BuyButton.PressedActionWithPayload = (gearId) =>
			{
				ScreenManager.Popup<BuyItemPopup>();
			};
			ShowInfoButton.PressedAction = () =>
			{
				ScreenManager.Popup<BuyItemPopup>();
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};

        }
	}
}
