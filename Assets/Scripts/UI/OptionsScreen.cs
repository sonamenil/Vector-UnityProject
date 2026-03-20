namespace UI
{
    [View("OptionsScreenView")]
    public class OptionsScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public Button LanguageButton = new Button();

		public Button RestorePurchasesButton = new Button();

		public OptionsScreen(ScreenManager screenManager)
			: base(screenManager)
		{
		}
	}
}
