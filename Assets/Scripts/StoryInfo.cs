using System.Collections.Generic;

public class StoryInfo
{
	public string Name
	{
		get;
		private set;
	}

	public StoryType Type
	{
		get;
		private set;
	}

	public List<string> TrickIds
	{
		get;
		private set;
	}

	public UnlockInfo UnlockInfo
	{
		get;
		private set;
	}

	public string RewardTemplate
	{
		get;
		private set;
	}

	public string CutsceneStart
	{
		get; 
		private set;
	}

	public string CutsceneEnd
	{
		get; 
		private set;
	}

	public StoryInfo(string id, StoryType storyType, List<string> trickIds, UnlockInfo unlockInfo, string rewardTemplate, string cutsceneStart, string cutsceneEnd)
	{
		Name = id;
		Type = storyType;
		TrickIds = trickIds;
		UnlockInfo = unlockInfo;
		RewardTemplate = rewardTemplate;
		CutsceneStart = cutsceneStart;
		CutsceneEnd = cutsceneEnd;
	}
}
