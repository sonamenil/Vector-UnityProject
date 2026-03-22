using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class LocationInfo
{
    public string Name;

    public string IconId;

    public UnlockInfo UnlockInfo;

    public LocationModeType LocationModeType;

    public string LocationType;

    public bool IsLocked;

    public Dictionary<StoryType, List<StoryInfo>> LocationInfos;

    [JsonIgnore]
    public List<StoryInfo> CurrentStoryModeStoryInfos => LocationInfos[UserDataManager.RuntimeInfo.StoryType];

    public List<StoryInfo> CurrentStoryInfos
    {
        get
        {
            return LocationInfos
                .SelectMany(kvp => kvp.Value)
                .ToList();
        }
    }

    public LocationInfo(string name, string iconId, UnlockInfo unlockInfo, LocationModeType locationModeType, string locationType, bool isLocked)
    {
        Name = name;
        IconId = iconId;
        UnlockInfo = unlockInfo;
        LocationModeType = locationModeType;
        LocationType = locationType;
        IsLocked = isLocked;
        LocationInfos = new Dictionary<StoryType, List<StoryInfo>>();
    }

    public void AddStories(StoryType type, List<StoryInfo> commonStories)
    {
        LocationInfos.Add(type, commonStories);
    }
}
