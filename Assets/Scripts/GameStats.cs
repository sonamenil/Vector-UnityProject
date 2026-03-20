using Banzai.Json;
using Newtonsoft.Json;
using PlayerData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class GameStats : BaseUserHolder<GameStats>
{

    private Dictionary<string, Record> _trackStats = new Dictionary<string, Record>();

    protected override string JSONName => "GameStats";

    public override void ParseData()
    {
        if (_userjObject["TrackStats"] != null)
        {
            var val = JsonUtils.AsString(_userjObject["TrackStats"]);
            _trackStats = JsonConvert.DeserializeObject<Dictionary<string, Record>>(val);
        }
    }

    public override void SaveData()
    {
        var val = JsonConvert.SerializeObject(_trackStats);
        _userjObject["TrackStats"] = val;
        UserDataManager.Instance.SaveUserDate();
    }

    public void TrackStatsAdd(string storyName, Record record)
    {
        _trackStats.Add(storyName, record);
        SaveData();
    }

    public Record GetTrackStats(string currentStoryName)
    {
        if (_trackStats.ContainsKey(currentStoryName))
        {
            return _trackStats[currentStoryName];
        }
        return null;
    }

    public int GetStarsCount(string starsInfoStoryId)
    {
        var record = GetTrackStats(starsInfoStoryId);
        if (record != null)
        {
            return record.Stars;
        }
        return 0;
    }

    public int GetPointsCount(string starsInfoStoryId)
    {
        var trackStats = GetTrackStats(starsInfoStoryId);
        if (trackStats != null)
            return GetTrackStats(starsInfoStoryId).Points;
        return 0;
    }

    public void SetTrackStats(Dictionary<string, Record> trackStats)
    {
        _trackStats = trackStats;
    }

    public int GetLocationStars(LocationModeType locationModeType, string locationType)
    {
        var ids = LocationManager.Instance.GetLocationIds(locationModeType, locationType);
        int stars = 0;
        foreach (var id in ids)
        {
            if (_trackStats.ContainsKey(id))
                stars += _trackStats[id].Stars;
        }
        return stars;
    }

    public int GetLocationTrackCompleted(LocationModeType locationModeType, string locationType, uint stars)
    {
        var ids = LocationManager.Instance.GetLocationIds(locationModeType, locationType);
        return ids.Count(id =>
            _trackStats.TryGetValue(id, out var record) &&
            record.Stars >= stars);
    }


    public int GetLocationsTrackCompleted(LocationModeType locationModeType, uint stars)
    {
        int num = 0;
        foreach (var location in LocationManager.Instance.locations.Keys)
        {
            num += GetLocationTrackCompleted(locationModeType, location, stars);
        }
        return num;
    }

    public int GetAllTrackCompleted(uint stars)
    {
        return GetLocationsTrackCompleted(LocationModeType.Classic, stars) + GetLocationsTrackCompleted(LocationModeType.Hunter, stars);
    }

    public int GetLocationsStars(LocationModeType locationModeType)
    {
        int num = 0;
        foreach (var location in LocationManager.Instance.locations.Keys)
        {
            num += GetLocationStars(locationModeType, location);
        }
        return num;
    }

    public int GetAllStars()
    {
        return GetLocationsStars(LocationModeType.Classic) + GetLocationsStars(LocationModeType.Hunter);
    }

    public int CalcUnlockBonus()
    {
        foreach (var item in _trackStats)
        {
            var info = LocationManager.Instance.GetStoryInfo(item.Key);
            int val = RewardExtensions.GetReward(LocationManager.Instance.GetRewards(info.RewardTemplate), 0, 0);

            UnityEngine.Debug.Log(string.Format("Compensation [{0}]", val));

            return val;
        }
        return 0;
    }

    public void Migrate(PlayerDataComponent playerData)
    {
    }
}
