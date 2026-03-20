namespace UI
{
    [View("PreGameplayScreen")]
    public class PreGameplayScreen : Screen
	{
		public Button BackToSelectStoryButton = new Button();

		public PreGameplayScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToSelectStoryButton.PressedAction = () =>
			{
				ScreenManager.Show<SelectStoryScreen>(true, false);
			};
		}
	}
}
