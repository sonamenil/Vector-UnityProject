using System.IO;
using Core._Common;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesLoader : MonoBehaviour
{
    public static Image LoadTrickImage(string trickIconId)
    {
        return Resources.Load<Image>(Path.Combine("Icons", "Tricks", trickIconId));
    }

    public static Image LoadGadgetImage(string gadgetIconId)
    {
        return Resources.Load<Image>(Path.Combine("Icons", "Gadgets", gadgetIconId));
    }

    public static Image LoadGearImage(string gearIconId)
    {
        return Resources.Load<Image>(Path.Combine("Icons", "Gear", gearIconId));
    }

    public static Sprite LoadGearSprite(string gearIconId)
    {
        return Resources.Load<Sprite>(Path.Combine("Icons", "Gear", gearIconId));
    }

    public static Sprite LoadItemSprite(string trickIconId)
    {
        if (ResourceManager.FileExists(
                Path.Combine(Application.streamingAssetsPath, "icons", "shop", trickIconId),
                out string path,
                ".png", ".jpg", ".jpeg"))
        {
            return ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 100);
        }

        return Resources.Load<Sprite>(Path.Combine("Icons", "Tricks", trickIconId));
    }

    public static Sprite LoadLocationSprite(string locationIconId)
    {
        if (ResourceManager.FileExists(
                Path.Combine(Application.streamingAssetsPath, "icons", "locations", locationIconId),
                out string path,
                ".png", ".jpg", ".jpeg"))
        {
            return ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 100);
        }

        return Resources.Load<Sprite>(Path.Combine("Icons", "Locations", locationIconId));
    }

    public static Sprite LoadStoriesSprite(string locationIconId)
    {
        if (ResourceManager.FileExists(
                Path.Combine(Application.streamingAssetsPath, "icons", "stories", locationIconId),
                out string path,
                ".png", ".jpg", ".jpeg"))
        {
            return ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 100);
        }

        return Resources.Load<Sprite>(Path.Combine("Icons", "Stories", locationIconId));
    }

    public static AudioClip LoadAudioClip(string audioClip)
    {
        var path = Path.Combine(VectorPaths.Sounds, audioClip);

        if (path.StartsWith(Application.streamingAssetsPath))
        {
            return ResourceManager.GetAudioClipFromExternal(path);
        }

        return Resources.Load<AudioClip>(path);
    }

    public static AudioClip LoadMusicClip(string musicClip)
    {
        var path = Path.Combine(VectorPaths.Music, musicClip);

        if (path.StartsWith(Application.streamingAssetsPath))
        {
            return ResourceManager.GetAudioClipFromExternal(path);
        }

        return Resources.Load<AudioClip>(path);
    }

    public static Image LoadImage(string id)
    {
        return Resources.Load<Image>(id);
    }

    public static T Load<T>(string id) where T : Object
    {
        return Resources.Load<T>(id);
    }
}