using System.IO;
using Core._Common;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesLoader : MonoBehaviour
{
    public static Image LoadTrickImage(string trickIconId)
    {
        return Resources.Load<Image>("Icons/Tricks/" + trickIconId);
    }

    public static Image LoadGadgetImage(string gadgetIconId)
    {
        return Resources.Load<Image>("Icons/Gadgets/" + gadgetIconId);
    }

    public static Image LoadGearImage(string gearIconId)
    {
        return Resources.Load<Image>("Icons/Gear" + gearIconId);
    }

    public static Sprite LoadGearSprite(string gearIconId)
    {
        return Resources.Load<Sprite>("Icons/Gear" + gearIconId);
    }

    public static Sprite LoadItemSprite(string trickIconId)
    {
        if (ResourceManager.FileExists(Application.streamingAssetsPath + "/icons/shop/" + trickIconId, out string path, ".png", ".jpg", ".jpeg"))
        {
            return ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 100);
        }
        return Resources.Load<Sprite>("Icons/Tricks/" + trickIconId);
    }

    public static Sprite LoadLocationSprite(string locationIconId)
    {
        if (ResourceManager.FileExists(Application.streamingAssetsPath + "/icons/locations/" + locationIconId, out string path, ".png", ".jpg", ".jpeg"))
        {
            return ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 100);
        }
        return Resources.Load<Sprite>("Icons/Locations/" + locationIconId);
    }

    public static Sprite LoadStoriesSprite(string locationIconId)
    {
        if (ResourceManager.FileExists(Application.streamingAssetsPath + "/icons/stories/" + locationIconId, out string path, ".png", ".jpg", ".jpeg"))
        {
            return ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 100);
        }
        return Resources.Load<Sprite>("Icons/Stories/" + locationIconId);
    }

    public static AudioClip LoadAudioClip(string audioClip)
    {
        var path = VectorPaths.Sounds + "/" + audioClip;
        if (path.StartsWith(Application.streamingAssetsPath))
        {
            return ResourceManager.GetAudioClipFromExternal(path);
        }
        return Resources.Load<AudioClip>(path);
    }

    public static AudioClip LoadMusicClip(string musicClip)
    {
        var path = VectorPaths.Music + "/" + musicClip;
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
