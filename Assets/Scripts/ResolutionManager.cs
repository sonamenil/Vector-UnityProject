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

    const string FSKey = "FullScreen";
    const string ResKey = "CurrentResolutionIndex";

    protected override void InitInternal()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CurrentResolution = Screen.currentResolution;
            FullScreenMode =  FullScreenMode.FullScreenWindow;
            //OnResolutionChanged.Invoke(CurrentResolution);

            Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, FullScreenMode, CurrentResolution.refreshRate);
            return;
        }
        if (!PlayerPrefs.HasKey(FSKey))
        {
            FullScreenMode =  FullScreenMode.FullScreenWindow;
        }
        else
        {
            FullScreenMode = (FullScreenMode)PlayerPrefs.GetInt(FSKey);
        }
        Screen.fullScreenMode =  FullScreenMode;
        var currentRR = Screen.currentResolution.refreshRate;
        bool SameRR(Resolution r) => r.refreshRate == currentRR || r.refreshRate + 1 == currentRR || r.refreshRate == currentRR + 1;

        _resolutions = Screen.resolutions
            .Where(SameRR)
            .OrderBy(r => r.width).ThenBy(r => r.height)
            .ToList();

        if (_resolutions.Count == 0)
        {
            _resolutions = Screen.resolutions.OrderBy(r => r.width).ThenBy(r => r.height).ToList();
        }

        int startIndex = LoadSavedIndex(_resolutions.Count);

        if (startIndex < 0)
        {
            startIndex = _resolutions.FindIndex(r =>
                r.width == Screen.currentResolution.width &&
                r.height == Screen.currentResolution.height
                && r.refreshRate == Screen.currentResolution.refreshRate
            );

            if (startIndex < 0) startIndex = _resolutions.Count - 1;
        }

        ChangeResolution(startIndex);
    }

    public void ChangeFullScreenMode(int index)
    {
        FullScreenMode = (FullScreenMode)index;
        Screen.fullScreenMode = FullScreenMode;
        PlayerPrefs.SetInt(FSKey, (int)FullScreenMode);
    }

    public int GetNextFullScreenMode()
    {
        int currentIndex = (int)FullScreenMode;
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
        if (_resolutions.Count == 0) return;

        index = Mathf.Clamp(index, 0, _resolutions.Count - 1);
        var res = _resolutions[index];

        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRate);

        CurrentResolution = res;
        PlayerPrefs.SetInt(ResKey, index);
        PlayerPrefs.Save();
        OnResolutionChanged?.Invoke(CurrentResolution);
    }

    public int GetNextResolution()
    {
        if (_resolutions.Count == 0) return 0;
        int currentIndex = _resolutions.FindIndex(r => AreSame(r, CurrentResolution));
        if (currentIndex < 0) currentIndex = 0;
        return (currentIndex + 1) % _resolutions.Count;
    }

    private static bool AreSame(Resolution a, Resolution b)
    {
        return a.width == b.width && a.height == b.height && (a.refreshRate == b.refreshRate || a.refreshRate + 1 == b.refreshRate || a.refreshRate == b.refreshRate + 1);
    }

    private int LoadSavedIndex(int count)
    {
        if (!PlayerPrefs.HasKey(ResKey)) return -1;
        int idx = PlayerPrefs.GetInt(ResKey);
        return idx >= 0 && idx < count ? idx : -1;
    }
}
