public class StarsLocationUnlockInfo
{
	public int Stars;

	public string StoryId;

	public LocationLocator LocationLocator;

	public static StarsLocationUnlockInfo Default
	{
		get
		{
			return new StarsLocationUnlockInfo(0, "DOWNTOWN_STORY_01", null);
		}
	}

	public StarsLocationUnlockInfo(int stars, string storyId, LocationLocator locationLocator)
	{
		Stars = stars;
		StoryId = storyId;
		LocationLocator = locationLocator;
	}
}
