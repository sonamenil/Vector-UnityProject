namespace UI
{
    [View("StatsScreen")]
    public class StatsScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public Button BuyCoinsButton = new Button();

		public StatsScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction = () =>
			{
				ScreenManager.Show<LobbyScreen>(true, false);
			};
			BuyCoinsButton.PressedAction = () =>
			{
				ScreenManager.Show<BuyCoinsScreen>(true, false);
			};
		}
	}
}
