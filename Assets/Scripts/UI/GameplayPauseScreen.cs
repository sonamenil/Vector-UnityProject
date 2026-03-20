namespace UI
{
    [View("GameplayPauseScreenView")]
    public class GameplayPauseScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public GameplayPauseScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction = () =>
			{
				ScreenManager.Show<LobbyScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
		}
	}
}
