using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class ResourceManager
{
    public static Dictionary<string, Sprite> textureCache = new Dictionary<string, Sprite>();

    public static Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

    public static void LoadAllTextures(string p_path, Vector2 pivot, float pixelsPerUnit)
    {
        foreach (var file in Directory.GetFiles(p_path))
        {
            LoadSpriteFromExternal(file, pivot, pixelsPerUnit);
        }
    }

    public static Sprite LoadSpriteFromExternal(string path, Vector2 pivot, float ppu)
    {
        if (textureCache.TryGetValue(path, out var cached))
        {
            return cached;
        }
        if (!File.Exists(path)) return null;

        byte[] bytes = File.ReadAllBytes(path);

        var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.LoadImage(bytes, markNonReadable: false);
        tex.wrapMode = TextureWrapMode.Clamp; 
        tex.filterMode = FilterMode.Trilinear;
        tex.Apply(false, false);

        var rect = new Rect(0, 0, tex.width, tex.height);
        var sprite = Sprite.Create(tex, rect, pivot, ppu, 0, SpriteMeshType.FullRect);
        textureCache[path] = sprite;
        return sprite;
    }


    public static byte[] GetBinary(string p_path)
    {
        if (p_path.StartsWith(Application.streamingAssetsPath))
        {
            if (!File.Exists(p_path))
            {
                return null;
            }
            FileStream fileStream = new FileStream(p_path, FileMode.Open, FileAccess.Read);
            byte[] array = new byte[fileStream.Length];
            fileStream.Read(array, 0, (int)fileStream.Length);
            fileStream.Close();
            return array;
        }
        char[] trimChars = { '\\', '/', };
        string path = p_path.TrimStart(trimChars);
        path = RemoveExtension(path);
        var obj = Resources.Load<TextAsset>(path);
        if (obj != null)
        {
            return obj.bytes;
        }
        return null;
    }

    public static string GetTextFromResources(string p_path)
    {
        char[] trimChars = { '\\', '/', };
        string path = p_path.TrimStart(trimChars);
        path = RemoveExtension(path);
        var obj = Resources.Load<TextAsset>(path);
        if (obj != null)
        {
            return obj.text;
        }
        return string.Empty;
    }

    private static string RemoveExtension(string p_path)
    {
        if (Path.HasExtension(p_path))
        {
            return Path.ChangeExtension(p_path, null);
        }
        return p_path;
    }

    public static bool FileExists(string p_path, out string existingPath, params string[] exts)
    {
        foreach (var ext in exts)
        {
            existingPath = p_path + ext;

            if (File.Exists(existingPath))
            {
                return true;
            }
        }

        existingPath = p_path;
        return false;
    }

    public static AudioClip GetAudioClipFromExternal(string p_fileName)
    {
        if (audioCache.ContainsKey(p_fileName))
        {
            return audioCache[p_fileName];
        }

        string[] possibleExts = { ".wav", ".mp3", ".ogg" };
        string resolvedPath = p_fileName;

        if (string.IsNullOrEmpty(Path.GetExtension(p_fileName)))
        {
            foreach (var ext in possibleExts)
            {
                string testPath = p_fileName + ext;
                if (File.Exists(testPath))
                {
                    resolvedPath = testPath;
                    break;
                }
            }
        }

        if (!File.Exists(resolvedPath))
        {
            return null;
        }

        AudioType audioType = AudioType.UNKNOWN;
        string extLower = Path.GetExtension(resolvedPath).ToLower();
        switch (extLower)
        {
            case ".wav": audioType = AudioType.WAV; break;
            case ".mp3": audioType = AudioType.MPEG; break;
            case ".ogg": audioType = AudioType.OGGVORBIS; break;
        }

        using (var www = UnityWebRequestMultimedia.GetAudioClip($"file:///{resolvedPath}", audioType))
        {
            www.SendWebRequest();

            while (!www.isDone && string.IsNullOrEmpty(www.error)) { }

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError($"Error loading audio clip from '{resolvedPath}': {www.error}");
                return null;
            }
            var audioClip = DownloadHandlerAudioClip.GetContent(www);
            audioCache.Add(p_fileName, audioClip);
            return audioClip;
        }
    }
}
