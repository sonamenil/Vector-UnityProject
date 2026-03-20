using Banzai.Json;
using Newtonsoft.Json;
using PlayerData;
using System;
using UnityEngine;

public class Options : BaseUserHolder<Options>
{
    public float SoundLevel = 1;

    public float MusicLevel = 1;

    public float LastUsedSetSoundValue = 1;

    public float LastUsedSetMusicValue = 1;

    public bool Sound = true;

    public bool Music = true;

    public bool GDPR;

    public string Locale;

    public int CurrentLocaleIndex;

    private bool _gameRated;

    private bool _showRateGameNextTime;

    [JsonIgnore]
    public Action<float> ToggleMusicEvent;

    [JsonIgnore]
    public Action<float> ToggleSoundEvent;

    protected override string JSONName => "Options";

    public bool GameRated
    {
        get
        {
            return _gameRated;
        }
        set
        {
            if ((_gameRated != false) != value)
            {
                _gameRated = value;
                SaveData();
            }
        }
    }

    public bool ShowRateGameNextTime
    {
        get
        {
            return _showRateGameNextTime;
        }
        set
        {
            if ((_showRateGameNextTime != false) != value)
            {
                _showRateGameNextTime = value;
                SaveData();
            }
        }
    }

    public override void ParseData()
    {
        SoundLevel = JsonUtils.GetFloat(_userjObject, "SoundLevel", 1);
        MusicLevel = JsonUtils.GetFloat(_userjObject, "MusicLevel", 1);
        LastUsedSetSoundValue = JsonUtils.GetFloat(_userjObject, "LastUsedSetSoundValue");
        LastUsedSetMusicValue = JsonUtils.GetFloat(_userjObject, "LastUsedSetMusicValue");
        Sound = JsonUtils.GetBool(_userjObject, "Sound", true);
        Music = JsonUtils.GetBool(_userjObject, "Music", true);
        GDPR = JsonUtils.GetBool(_userjObject, "GDPR", false);
        Locale = JsonUtils.GetText(_userjObject, "Locale");
        CurrentLocaleIndex = JsonUtils.GetInt(_userjObject, "CurrentLocaleIndex");
        _gameRated = JsonUtils.GetBool(_userjObject, "GameRated", false);
        _showRateGameNextTime = JsonUtils.GetBool(_userjObject, "ShowRateGameNextTime", false);
    }

    public override void SaveData()
    {
        _userjObject["SoundLevel"] = SoundLevel;
        _userjObject["MusicLevel"] = MusicLevel;
        _userjObject["LastUsedSetSoundValue"] = LastUsedSetSoundValue;
        _userjObject["LastUsedSetMusicValue"] = LastUsedSetMusicValue;
        _userjObject["Sound"] = Sound;
        _userjObject["Music"] = Music;
        _userjObject["GDPR"] = GDPR;
        _userjObject["Locale"] = Locale;
        _userjObject["CurrentLocaleIndex"] = CurrentLocaleIndex;
        _userjObject["GameRated"] = _gameRated;
        _userjObject["ShowRateGameNextTime"] = _showRateGameNextTime;
        UserDataManager.Instance.SaveUserDate();
    }

    public void Update()
    {
        SoundsManager.Instance.SetSoundLevel(SoundLevel);
        SoundsManager.Instance.SetMusicLevel(MusicLevel);
        LocalizationManager.Instance.ChangeLocale(Locale);
        SaveData();
    }

    public void ToggleMusic()
    {
        MusicLevel = 0;
        if (Music == false)
        {
            MusicLevel = LastUsedSetMusicValue;
        }
        Music = !Music;
        SoundsManager.Instance.SetMusicPause(!Music);
        ToggleMusicEvent?.Invoke(MusicLevel);
        Update();
        SaveData();
    }

    public void ToggleSound()
    {
        SoundLevel = 0;
        if (Sound == false)
        {
            SoundLevel = LastUsedSetSoundValue;
        }
        Sound = !Sound;
        SoundsManager.Instance.SetSoundsPause(!Sound);
        ToggleSoundEvent?.Invoke(SoundLevel);
        Update();
        SaveData();
    }

    public void ToggleValue(ref bool status, ref float value, float userSetValue)
    {
        value = 0;
        if (status == false)
        {
            value = userSetValue;
        }
        status = !status;
    }

    public void Migrate(PlayerDataComponent playerData)
    {
        if (playerData == null)
        {
            return;
        }
        var options = playerData.Options;
        SoundLevel = options.SoundLevel;
        MusicLevel = options.MusicLevel;
        LastUsedSetSoundValue = options.LastUsedSetSoundValue;
        LastUsedSetMusicValue = options.LastUsedSetMusicValue;
        Sound = options.Sound;
        Music = options.Music;
        GDPR = options.GDPR;
        Locale = options.Locale;
        CurrentLocaleIndex = options.CurrentLocaleIndex;
        SaveData();
    }
}
