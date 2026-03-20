namespace UI
{
    [View("SelectLocationScreen")]
    public class SelectLocationScreen : Screen
	{
		public Button BackToChooseModeButton = new Button();

		public Button ContinueButton = new Button();

		public Button SelectButton = new Button();

		public Button BuyCoinsButton = new Button();

		public SelectLocationScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToChooseModeButton.PressedAction = () =>
			{
				ScreenManager.Show<ChooseModeScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
			ContinueButton.PressedAction = () =>
			{
				ScreenManager.Show<SelectStoryScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
            SelectButton.PressedAction = () =>
            {
                ScreenManager.Show<SelectStoryScreen>(true, false);
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            };
        }
	}
}
