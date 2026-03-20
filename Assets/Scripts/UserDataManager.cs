using Newtonsoft.Json.Linq;
using PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserDataManager : AbstractManager<UserDataManager>
{
    public static int VERSION = 1;

    public static string USER_DATA_FOLDER = Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Path.Combine(Directory.GetParent(Application.dataPath).FullName, "saves");

    public static string USER_DATA_FILE_NAME = USER_DATA_FOLDER + "/UserData.data";

    private List<BaseUserHolder> _allHolder = new List<BaseUserHolder>();

    public MainData MainData;

    public Stats Stats;

    public Options Options;

    public GameStats GameStats;

    public ShopData ShopData;

    public OldData OldData;

    private JObject _userJsonRoot;

    public static RuntimeInfo RuntimeInfo = new RuntimeInfo();

    public LocationInfo CurrentBalanceLocation
    {
        get
        {
            return AbstractManager<LocationManager>.Instance.GetLocationInfo(RuntimeInfo.CurentLocationType, RuntimeInfo.LocationModeType);
        }
    }

    public event Action UnlockEvent
    {
        add
        {
            UnlockEvent += value;
        }
        remove
        {
            UnlockEvent -= value;
        }
    }

    protected override void InitInternal()
    {
        if (!Directory.Exists(USER_DATA_FOLDER))
        {
            Directory.CreateDirectory(USER_DATA_FOLDER);
        }

        _userJsonRoot = LoadDataFromFile();
        MainData = MainData.Create(_userJsonRoot);
        Stats = Stats.Create(_userJsonRoot);
        Options = Options.Create(_userJsonRoot);
        GameStats = GameStats.Create(_userJsonRoot);
        ShopData = ShopData.Create(_userJsonRoot);
        OldData = OldData.Create(_userJsonRoot);

        _allHolder.Add(MainData);
        _allHolder.Add(Stats);
        _allHolder.Add(Options);
        _allHolder.Add(GameStats);
        _allHolder.Add(ShopData);
        _allHolder.Add(OldData);

        Options.Update();
        MigrateFromMarmalade();
        MigrateFromBadFormat();
        InitDataFromScratch();
        SetClassicMode();

        var newLocale = Options.Locale;
        if (newLocale == null)
        {
            newLocale = LocalizationManager.Instance.GetSystem();
        }

        LocalizationManager.Instance.ChangeLocale(newLocale);

    }

    private void CheckVersion()
    {
    }

    private static JObject LoadDataFromFile()
    {
        string content = string.Empty;
        if (File.Exists(USER_DATA_FILE_NAME))
        {
            content = FileUtils.ReadAllText(USER_DATA_FILE_NAME, true);
        }
        if (string.IsNullOrEmpty(content))
        {
            var jObject = new JObject();
            jObject["V"] = VERSION;
            return jObject;
        }
        return JObject.Parse(content);
    }

    private void InitDataFromScratch()
    {
        ShopData.Add("TRICK_JUMPTUMBLE", 1);
        ShopData.Add("TRICK_SPINNINGVAULT", 1);
    }

    private void MigrateFromMarmalade()
    {
        //NOT NECESSARY
    }

    private void MigrateFromBadFormat()
    {
        if (PlayerDataStorage.HasData())
        {
            var data = new PlayerDataComponent();
            data.Init();

            MainData.Migrate(data);
            Stats.Migrate(data);
            Options.Migrate(data);
            GameStats.Migrate(data);
            ShopData.Migrate(data);
            OldData.Migrate(data);

            PlayerDataStorage.Clear();
        }
    }

    public void SetClassicMode()
    {
        RuntimeInfo.LocationModeType = LocationModeType.Classic;
    }

    public void SetHunterMode()
    {
        RuntimeInfo.LocationModeType = LocationModeType.Hunter;
    }

    public void SaveUserDate()
    {
        FileUtils.WriteAllText(USER_DATA_FILE_NAME, _userJsonRoot.ToString(), true);
    }
}
