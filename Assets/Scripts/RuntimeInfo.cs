using System.Collections.Generic;
using UnityEngine;

public class RuntimeInfo
{
    public string CurentItemId;

    public StoreItemType CurentItemType;

    public string CurentLocationType;

    public StoryType StoryType;

    public LocationModeType LocationModeType;

    public int CurrentStory;

    public bool IsHunterMode;

    public bool IsWin;

    public int WinStars;

    public int LastSelectedGear = 1;

    public int LastSelectedTrick;

    public Dictionary<string, Dictionary<LocationModeType, Dictionary<StoryType, int>>> LastSelectedStories = new Dictionary<string, Dictionary<LocationModeType, Dictionary<StoryType, int>>>();

    public int GetLastIndex()
    {
        if (LastSelectedStories.ContainsKey(CurentLocationType))
        {
            if (LastSelectedStories[CurentLocationType].ContainsKey(LocationModeType))
            {
                if (LastSelectedStories[CurentLocationType][LocationModeType].ContainsKey(StoryType))
                {
                    return LastSelectedStories[CurentLocationType][LocationModeType][StoryType];
                }
            }
        }
        return 0;
    }

    public void SetLastIndex(int index)
    {
        if (!LastSelectedStories.ContainsKey(CurentLocationType))
        {
            LastSelectedStories.Add(CurentLocationType, new Dictionary<LocationModeType, Dictionary<StoryType, int>>());
        }
        if (!LastSelectedStories[CurentLocationType].ContainsKey(LocationModeType))
        {
            LastSelectedStories[CurentLocationType].Add(LocationModeType, new Dictionary<StoryType, int>());
        }
        if (LastSelectedStories[CurentLocationType][LocationModeType].ContainsKey(StoryType))
        {
            LastSelectedStories[CurentLocationType][LocationModeType][StoryType] = index;
        }
        else
        {
            LastSelectedStories[CurentLocationType][LocationModeType].Add(StoryType, index);
        }
    }
}
