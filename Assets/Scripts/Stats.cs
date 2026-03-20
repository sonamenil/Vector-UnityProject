using Banzai.Json;
using PlayerData;

public class Stats : BaseUserHolder<Stats>
{
    protected override string JSONName => "Stats";

    public float TotalRunningDistance
    {
        get;
        private set;
    }

    public int JumpsCount
    {
        get;
        private set;
    }

    public int SlidesCount
    {
        get;
        private set;
    }

    public int BonusCollected
    {
        get;
        private set;
    }

    public int CoinsCollected
    {
        get;
        private set;
    }

    public int TricksPerformed
    {
        get;
        private set;
    }

    public int Death
    {
        get;
        private set;
    }

    public int DeathByHunter
    {
        get;
        private set;
    }

    public int HuntersKilled
    {
        get;
        private set;
    }

    public int GlassBroken
    {
        get;
        private set;
    }

    public override void ParseData()
    {
        TotalRunningDistance = JsonUtils.GetFloat(_userjObject, "TotalRunningDistance");
        JumpsCount = JsonUtils.GetInt(_userjObject, "JumpsCount");
        SlidesCount = JsonUtils.GetInt(_userjObject, "SlidesCount");
        BonusCollected = JsonUtils.GetInt(_userjObject, "BonusCollected");
        CoinsCollected = JsonUtils.GetInt(_userjObject, "CoinsCollected");
        TricksPerformed = JsonUtils.GetInt(_userjObject, "TricksPerformed");
        Death = JsonUtils.GetInt(_userjObject, "Death");
        DeathByHunter = JsonUtils.GetInt(_userjObject, "DeathByHunter");
        HuntersKilled = JsonUtils.GetInt(_userjObject, "HuntersKilled");
        GlassBroken = JsonUtils.GetInt(_userjObject, "GlassBroken");
    }

    public void SetInitialData(float totalRunningDistance, int jumpsCount, int slidesCount, int bonusCollected, int coinsCollected, int tricksPerformed, int death, int deathByHunter, int huntersKilled, int glassBroken)
    {
        TotalRunningDistance = totalRunningDistance;
        JumpsCount = jumpsCount;
        SlidesCount = slidesCount;
        BonusCollected = bonusCollected;
        CoinsCollected = coinsCollected;
        TricksPerformed = tricksPerformed;
        Death = death;
        DeathByHunter = deathByHunter;
        HuntersKilled = huntersKilled;
        GlassBroken = glassBroken;
    }

    public void Add(ControllerStatistics controllerStatistics)
    {
        if (controllerStatistics == null)
        {
            return;
        }
        TotalRunningDistance += controllerStatistics.distance;
        JumpsCount += controllerStatistics.Jumps;
        SlidesCount += controllerStatistics.Slides;
        BonusCollected += controllerStatistics.bonusCollected;
        CoinsCollected += controllerStatistics.coinsCollected;
        TricksPerformed += controllerStatistics.trickCollected;
        Death += controllerStatistics.death;
        DeathByHunter += controllerStatistics.deathFromHunter;
        HuntersKilled += controllerStatistics.killedHunter;
        GlassBroken += controllerStatistics.brokenGlass;
        SaveData();
    }

    public override void SaveData()
    {
        _userjObject["TotalRunningDistance"] = TotalRunningDistance;
        _userjObject["JumpsCount"] = JumpsCount;
        _userjObject["SlidesCount"] = SlidesCount;
        _userjObject["BonusCollected"] = BonusCollected;
        _userjObject["CoinsCollected"] = CoinsCollected;
        _userjObject["TricksPerformed"] = TricksPerformed;
        _userjObject["Death"] = Death;
        _userjObject["DeathByHunter"] = DeathByHunter;
        _userjObject["HuntersKilled"] = HuntersKilled;
        _userjObject["GlassBroken"] = GlassBroken;
        UserDataManager.Instance.SaveUserDate();
    }

    public void Migrate(PlayerDataComponent playerData)
    {
        if (playerData == null)
        {
            return;
        }
        var stats = playerData.Stats;
        TotalRunningDistance = stats.TotalRunningDistance;
        JumpsCount = stats.JumpsCount;
        SlidesCount = stats.SlidesCount;
        BonusCollected = stats.BonusCollected;
        CoinsCollected = stats.CoinsCollected;
        TricksPerformed = stats.TricksPerformed;
        Death = stats.Death;
        DeathByHunter = stats.DeathByHunter;
        HuntersKilled = stats.HuntersKilled;
        GlassBroken = stats.GlassBroken;
        SaveData();
    }
}
