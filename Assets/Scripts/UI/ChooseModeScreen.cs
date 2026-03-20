namespace UI
{
    [View("ChooseModeScreen")]
    public class ChooseModeScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public ChooseModeScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction += () =>
			{
				ScreenManager.Show<LobbyScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
		}
	}
}
