namespace UI
{
    [View("PostGameplayWinScreen")]
    public class PostGameplayWinScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public PostGameplayWinScreen(ScreenManager screenManager)
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
