using System;
using Banzai.Json;
using Newtonsoft.Json;
using PlayerData;

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
        get => _gameRated;
        set
        {
            if (_gameRated != value)
            {
                _gameRated = value;
                SaveData();
            }
        }
    }

    public bool ShowRateGameNextTime
    {
        get => _showRateGameNextTime;
        set
        {
            if (_showRateGameNextTime != value)
            {
                _showRateGameNextTime = value;
                SaveData();
            }
        }
    }

    public override void ParseData()
    {
        SoundLevel = _userjObject.GetFloat("SoundLevel", 1);
        MusicLevel = _userjObject.GetFloat("MusicLevel", 1);
        LastUsedSetSoundValue = _userjObject.GetFloat("LastUsedSetSoundValue");
        LastUsedSetMusicValue = _userjObject.GetFloat("LastUsedSetMusicValue");
        Sound = _userjObject.GetBool("Sound", true);
        Music = _userjObject.GetBool("Music", true);
        GDPR = _userjObject.GetBool("GDPR", false);
        Locale = _userjObject.GetText("Locale");
        CurrentLocaleIndex = _userjObject.GetInt("CurrentLocaleIndex");
        _gameRated = _userjObject.GetBool("GameRated", false);
        _showRateGameNextTime = _userjObject.GetBool("ShowRateGameNextTime", false);
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
        if (!Music)
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
        if (!Sound)
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
        if (!status)
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
