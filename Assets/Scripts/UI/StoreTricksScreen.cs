namespace UI
{
    [View("StoreTricksScreenView")]
    public class StoreTricksScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public Button BuyCoinsButton = new Button();

		public Button BuyButton = new Button();

		public Button ShowInfoButton = new Button();

		public StoreTricksScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction = () => 
			{
				ScreenManager.Show<StoreScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
			ShowInfoButton.PressedAction = () =>
			{
				ScreenManager.Popup<BuyItemPopup>();
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};

        }
	}
}
