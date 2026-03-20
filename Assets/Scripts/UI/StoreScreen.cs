namespace UI
{
    [View("StoreScreen")]
    public class StoreScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public Button GadgetsButton = new Button();

		public Button TricksButton = new Button();

		public Button GearButton = new Button();

		public Button CoinsButton = new Button();

		public Button BuyCoinsButton = new Button();

		public StoreScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction = () =>
			{
				ScreenManager.Show<LobbyScreen>(true, false);
			};
			GadgetsButton.PressedAction = () => {
				ScreenManager.Show<StoreGadgetsScreen, GadgetScreenPayloadData>(new GadgetScreenPayloadData(false), true, false);
			};
            TricksButton.PressedAction = () => {
				ScreenManager.Show<StoreTricksScreen>(true, false);
			};
            GearButton.PressedAction = () => {
                ScreenManager.Show<StoreGearScreen>(true, false);
            };
        }
    }
}
