namespace UI
{
    [View("SelectStoryScreen")]
    public class SelectStoryScreen : Screen
	{
		public Button BackToSelectLocationButton = new Button();

		public Button PlayButton = new Button();

		public Toggle StoryButton = new Toggle();

		public Toggle BonusButton = new Toggle();

		public Button SelectButton = new Button();

		public SelectStoryScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToSelectLocationButton.PressedAction = () =>
			{
				ScreenManager.Show<SelectLocationScreen>(true, false);
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            };
			SelectButton.PressedAction = () =>
			{
				ScreenManager.Show<PreGameplayScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
		}
	}
}
