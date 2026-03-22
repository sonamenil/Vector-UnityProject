using System;

namespace UI
{
    [View("YesNoPopupView")]
    public class YesNoPopup : Screen
	{
		public Button YesButton = new Button();

		public Button NoButton = new Button();

		public Button BackgroundButton = new Button();

		public event Action<string> CaptionChanged;

		public YesNoPopup(ScreenManager screenManager)
			: base(screenManager)
		{
			YesButton.PressedAction = () => ScreenManager.ClosePopup();
			NoButton.PressedAction = () =>
			{
				YesButton.PressedAction = null;
				ScreenManager.ClosePopup();
			};
            BackgroundButton.PressedAction = () =>
            {
	            YesButton.PressedAction = null;
	            ScreenManager.ClosePopup();
            };
        }

		public void SetCaption(string caption)
		{
			CaptionChanged?.Invoke(caption);
		}
	}
}
