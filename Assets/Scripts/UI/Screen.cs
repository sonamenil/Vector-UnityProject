namespace UI
{
	public abstract class Screen
	{
		public ScreenManager ScreenManager;

		public Screen(ScreenManager screenManager)
		{
			ScreenManager = screenManager;
		}
	}
}
