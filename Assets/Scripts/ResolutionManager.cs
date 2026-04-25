using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResolutionManager : AbstractManager<ResolutionManager>
{
    private List<Resolution> _resolutions = new List<Resolution>();

    public Resolution CurrentResolution { get; private set; }
    public Action<Resolution> OnResolutionChanged;

    public FullScreenMode FullScreenMode;

    private const string FSKey = "FullScreen";
    private const string ResKey = "CurrentResolutionIndex";

    protected override void InitInternal()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CurrentResolution = Screen.currentResolution;
            FullScreenMode = FullScreenMode.FullScreenWindow;

            Screen.SetResolution(
                CurrentResolution.width,
                CurrentResolution.height,
                FullScreenMode,
                CurrentResolution.refreshRateRatio
            );

            OnResolutionChanged?.Invoke(CurrentResolution);
            return;
        }

        FullScreenMode = PlayerPrefs.HasKey(FSKey)
            ? (FullScreenMode)PlayerPrefs.GetInt(FSKey)
            : FullScreenMode.FullScreenWindow;

        Screen.fullScreenMode = FullScreenMode;

        RefreshRate currentRR = Screen.currentResolution.refreshRateRatio;

        bool SameRR(Resolution r) => AreSameRefreshRate(r.refreshRateRatio, currentRR);

        _resolutions = Screen.resolutions
            .Where(SameRR)
            .OrderBy(r => r.width)
            .ThenBy(r => r.height)
            .ToList();

        if (_resolutions.Count == 0)
        {
            _resolutions = Screen.resolutions
                .OrderBy(r => r.width)
                .ThenBy(r => r.height)
                .ToList();
        }

        int startIndex = LoadSavedIndex(_resolutions.Count);

        if (startIndex < 0)
        {
            startIndex = _resolutions.FindIndex(r =>
                r.width == Screen.currentResolution.width &&
                r.height == Screen.currentResolution.height &&
                AreSameRefreshRate(r.refreshRateRatio, Screen.currentResolution.refreshRateRatio)
            );

            if (startIndex < 0)
                startIndex = _resolutions.Count - 1;
        }

        ChangeResolution(startIndex);
    }

    public void ChangeFullScreenMode(int index)
    {
        FullScreenMode = (FullScreenMode)index;
        Screen.fullScreenMode = FullScreenMode;

        PlayerPrefs.SetInt(FSKey, (int)FullScreenMode);
        PlayerPrefs.Save();
    }

    public int GetNextFullScreenMode()
    {
        int currentIndex = (int)FullScreenMode;

        // Unity FullScreenMode enum currently has 4 values:
        // ExclusiveFullScreen, FullScreenWindow, MaximizedWindow, Windowed
        return (currentIndex + 1) % 4;
    }

    public string GetFullScreenModeName()
    {
        switch (FullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                return "Exclusive Fullscreen";

            case FullScreenMode.FullScreenWindow:
                return "Fullscreen Window";

            case FullScreenMode.Windowed:
                return "Windowed";

            case FullScreenMode.MaximizedWindow:
                return "Maximized Window";

            default:
                return "Fullscreen";
        }
    }

    public void ChangeResolution(int index)
    {
        if (_resolutions.Count == 0)
            return;

        index = Mathf.Clamp(index, 0, _resolutions.Count - 1);

        Resolution res = _resolutions[index];

        Screen.SetResolution(
            res.width,
            res.height,
            Screen.fullScreenMode,
            res.refreshRateRatio
        );

        CurrentResolution = res;

        PlayerPrefs.SetInt(ResKey, index);
        PlayerPrefs.Save();

        OnResolutionChanged?.Invoke(CurrentResolution);
    }

    public int GetNextResolution()
    {
        if (_resolutions.Count == 0)
            return 0;

        int currentIndex = _resolutions.FindIndex(r => AreSame(r, CurrentResolution));

        if (currentIndex < 0)
            currentIndex = 0;

        return (currentIndex + 1) % _resolutions.Count;
    }

    private static bool AreSame(Resolution a, Resolution b)
    {
        return a.width == b.width &&
               a.height == b.height &&
               AreSameRefreshRate(a.refreshRateRatio, b.refreshRateRatio);
    }

    private static bool AreSameRefreshRate(RefreshRate a, RefreshRate b)
    {
        // Unity 6 stores refresh rates as rational values, e.g. 60000 / 1001 = 59.94Hz.
        // This keeps your old +/- 1Hz tolerance behavior.
        return Mathf.Abs((float)a.value - (float)b.value) <= 1f;
    }

    private int LoadSavedIndex(int count)
    {
        if (!PlayerPrefs.HasKey(ResKey))
            return -1;

        int idx = PlayerPrefs.GetInt(ResKey);

        return idx >= 0 && idx < count ? idx : -1;
    }
}