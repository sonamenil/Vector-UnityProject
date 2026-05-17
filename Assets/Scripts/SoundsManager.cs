using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Core._Common;

public class SoundsManager : MonoBehaviour
{
    private static string PREFAB_PATH = Path.Combine("Other", "SoundsManager");

    [Header("Audio Sources")]
    public AudioSource AudioSourceMain;
    public AudioSource AudioSourceBackground;

    [Header("SFX Channel Pool")]
    [Min(1)] public int initialSfxChannels = 11;
    [Min(1)] public int maxSfxChannels = 16;
    public bool autoExpandPool = true;

    [Header("Defaults")]
    [Range(0f, 1f)] public float defaultSfxVolume = AudioLimits.MaxSoundVolume;
    [Range(0f, 1f)] public float defaultMusicVolume = AudioLimits.MaxMusicVolume;

    private static SoundsManager _instance;
    public static SoundsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<GameObject>(PREFAB_PATH);
                var obj = Instantiate(prefab);
                obj.name = "SoundsManager";
                DontDestroyOnLoad(obj);
                _instance = obj.GetComponent<SoundsManager>();
            }
            return _instance;
        }
    }

    private readonly List<AudioSource> _sfxSources = new List<AudioSource>();
    private Transform _sfxContainer;

    // ===================== GLOBAL AUDIO STATE =====================
    private bool _sfxEnabled = true;
    private bool _musicEnabled = true;
    private float _sfxLevel = 1f;
    private float _musicLevel = 1f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        BuildPool();
        Init();
    }

	public void SyncFromUserData()
	{
		var options = UserDataManager.Instance.Options;

		_musicLevel = options.MusicLevel;
		_sfxLevel = options.SoundLevel;

		_musicEnabled = options.Music;
		_sfxEnabled = options.Sound;

		ApplyToRuntimeAudio();
	}

    private void BuildPool()
    {
        _sfxContainer = new GameObject("SFX_Channels").transform;
        _sfxContainer.SetParent(transform, false);

        if (AudioSourceMain != null)
        {
            PrepareSfxSource(AudioSourceMain);
            _sfxSources.Add(AudioSourceMain);
        }

        int need = Mathf.Max(1, initialSfxChannels) - _sfxSources.Count;
        for (int i = 0; i < need; i++)
            _sfxSources.Add(CreateSfxSource());

        ApplyToRuntimeAudio();
    }

    private AudioSource CreateSfxSource()
    {
        var go = new GameObject($"SFX_Channel_{_sfxSources.Count}");
        go.transform.SetParent(_sfxContainer, false);
        var src = go.AddComponent<AudioSource>();
        PrepareSfxSource(src);
        return src;
    }

    private void PrepareSfxSource(AudioSource src)
    {
        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;
        src.dopplerLevel = 0f;
    }

    // =========================================================
    // STATE APPLY
    // =========================================================

    public void ApplyUserSettings(bool soundEnabled, float soundLevel, bool musicEnabled, float musicLevel)
    {
        _sfxEnabled = soundEnabled;
        _musicEnabled = musicEnabled;

        _sfxLevel = soundLevel;
        _musicLevel = musicLevel;

        ApplyToRuntimeAudio();
    }

    private void ApplyToRuntimeAudio()
    {
        if (AudioSourceBackground != null)
        {
            AudioSourceBackground.mute = !_musicEnabled;
            AudioSourceBackground.volume = _musicLevel;
        }

        foreach (var s in _sfxSources)
        {
            s.mute = !_sfxEnabled;
            s.volume = _sfxLevel;
        }
    }

    // =========================================================
    // PAN SYSTEM
    // =========================================================

    private float CalculateStereoPanFromScreen(Vector3 worldPos, Camera cam)
    {
        if (cam == null)
            return 0f;

        Vector3 vp = cam.WorldToViewportPoint(worldPos);

        if (vp.z < 0f)
            return 0f;

        float x = Mathf.Clamp01(vp.x);
        return (x * 2f) - 1f;
    }

    // =========================================================
    // SOURCE POOL
    // =========================================================

    private AudioSource GetFreeSfxSource()
    {
        var idle = _sfxSources.FirstOrDefault(s => !s.isPlaying);

        AudioSource candidate;

        if (idle != null)
        {
            candidate = idle;
        }
        else if (autoExpandPool && _sfxSources.Count < Mathf.Max(1, maxSfxChannels))
        {
            candidate = CreateSfxSource();
            _sfxSources.Add(candidate);
        }
        else
        {
            candidate = _sfxSources[0];

            float minRemaining = Remaining(candidate);
            for (int i = 1; i < _sfxSources.Count; i++)
            {
                float rem = Remaining(_sfxSources[i]);
                if (rem < minRemaining)
                {
                    minRemaining = rem;
                    candidate = _sfxSources[i];
                }
            }

            candidate.Stop();
        }

        // IMPORTANT: apply global state (NOT defaults!)
        candidate.panStereo = 0f;
        candidate.pitch = 1f;
        candidate.spatialBlend = 0f;
        candidate.volume = _sfxLevel;
        candidate.mute = !_sfxEnabled;

        return candidate;

        float Remaining(AudioSource s)
        {
            if (s.clip == null || !s.isPlaying) return 0f;
            return Mathf.Max(0f, s.clip.length - s.time);
        }
    }

    // =========================================================
    // GLOBAL CONTROLS
    // =========================================================

    public void SetMusicPause(bool state)
    {
        _musicEnabled = !state;

        if (AudioSourceBackground != null)
            AudioSourceBackground.mute = state;
    }

    public void SetSoundsPause(bool state)
    {
        _sfxEnabled = !state;

        foreach (var s in _sfxSources)
            s.mute = state;
    }

    public void SetSoundLevel(float value)
    {
        _sfxLevel = value;

        foreach (var s in _sfxSources)
            s.volume = value;
    }

    public void SetMusicLevel(float value)
    {
        _musicLevel = value;

        if (AudioSourceBackground != null)
            AudioSourceBackground.volume = value;
    }

    public void PauseAll(bool state)
    {
        if (state)
        {
            AudioSourceBackground?.Pause();
            foreach (var s in _sfxSources) s.Pause();
        }
        else
        {
            AudioSourceBackground?.UnPause();
            foreach (var s in _sfxSources) s.UnPause();
        }
    }

    // =========================================================
    // MUSIC
    // =========================================================

    public void StopBackground()
    {
        AudioSourceBackground?.Stop();
    }

    public void PlayBackground(MusicType musicType)
        => PlayBackground(musicType.ToString());

    public void PlayBackground(string musicName, bool loop = true)
    {
        if (AudioSourceBackground == null)
            return;

        var clip = ResourcesLoader.LoadMusicClip(musicName);
        if (clip == null)
        {
            Debug.LogError("Music clip not found: " + musicName);
            return;
        }

        AudioSourceBackground.clip = clip;
        AudioSourceBackground.loop = loop;

        AudioSourceBackground.volume = _musicLevel;
        AudioSourceBackground.mute = !_musicEnabled;

        if (!_musicEnabled)
            return;

        AudioSourceBackground.Play();
    }

    // =========================================================
    // SFX
    // =========================================================

    public void PlaySounds(params SoundType[] soundTypes)
    {
        string[] names = soundTypes.Select(s => s.ToString()).ToArray();
        PlaySounds(names);
    }

    public void PlaySounds(params string[] soundNames)
    {
        foreach (var name in soundNames)
        {
            var clip = ResourcesLoader.LoadAudioClip(name);
            if (clip == null)
            {
                Debug.LogError("Clip not found: " + name);
                continue;
            }

            var src = GetFreeSfxSource();
            src.pitch = 1f;
            src.spatialBlend = 0f;
            src.PlayOneShot(clip);
        }
    }

    public void PlaySoundsOnce(string soundName, float volume, float pitch = 1f, float panStereo = 0f)
    {
        var clip = ResourcesLoader.LoadAudioClip(soundName);

        if (clip == null)
        {
            Debug.LogError("Clip not found: " + soundName);
            return;
        }

        var src = GetFreeSfxSource();

        src.pitch = pitch;
        src.spatialBlend = 0f;
        src.panStereo = Mathf.Clamp(panStereo, -1f, 1f);

        src.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    public void PlaySoundAt(string soundName, Vector3 position,
        float volume = 1f, float pitch = 1f, float spatialBlend = 1f)
    {
        var clip = ResourcesLoader.LoadAudioClip(soundName);
        if (clip == null)
        {
            Debug.LogError("Clip not found: " + soundName);
            return;
        }

        var src = GetFreeSfxSource();

        src.transform.position = position;
        src.pitch = pitch;
        src.spatialBlend = Mathf.Clamp01(spatialBlend);
        src.minDistance = 1f;
        src.maxDistance = 25f;
        src.rolloffMode = AudioRolloffMode.Linear;

        src.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    // =========================================================
    // SEQUENTIAL
    // =========================================================

    public void PlaySoundsSequential(params string[] soundNames)
    {
        StartCoroutine(PlaySequentially(soundNames));
    }

    public void PlaySoundsSequential(params SoundType[] soundTypes)
    {
        string[] names = soundTypes.Select(s => s.ToString()).ToArray();
        StartCoroutine(PlaySequentially(names));
    }

    private IEnumerator PlaySequentially(params string[] soundNames)
    {
        var src = GetFreeSfxSource();
        src.pitch = 1f;
        src.spatialBlend = 0f;

        foreach (var name in soundNames)
        {
            var clip = ResourcesLoader.LoadAudioClip(name);
            if (clip == null)
                continue;

            src.clip = clip;
            src.Play();

            while (src != null && src.isPlaying)
                yield return null;
        }
    }

    // =========================================================
    // UTILS
    // =========================================================

    public void Init() { }

    public void StopAllSfx()
    {
        foreach (var s in _sfxSources)
            s.Stop();
    }

    public bool IsAnySfxPlaying()
    {
        return _sfxSources.Any(s => s.isPlaying);
    }
}