namespace UI
{
    [View("StoreGadgetsScreenView")]
    public class StoreGadgetsScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public Button BuyCoinsButton = new Button();

		public Button BuyButton = new Button();

		public Button ShowInfoButton = new Button();

		public StoreGadgetsScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction = () => {
				ScreenManager.Show<StoreScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
		}
	}
}