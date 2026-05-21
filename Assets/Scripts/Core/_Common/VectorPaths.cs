using UnityEngine;
using System.IO;

namespace Core._Common
{
    public class VectorPaths
    {
        public static string CurrentResources = Application.streamingAssetsPath;

        public static string XmlRoot = Path.Combine(CurrentResources, "XmlRoot");

        public static string XmlLevels = Path.Combine(XmlRoot, "Levels");

        public static string Models = Path.Combine(CurrentResources, "Models");

        public static string Animations = Path.Combine(CurrentResources, "Animations");

        public static string AnimationBinary = Path.Combine(Animations, "Data");

        public static string LevelsPrefab = Path.Combine("LevelContent", "Prefabs", "Levels");

        public static string Localization = Path.Combine(CurrentResources, "localization");

        public static string Commons = Path.Combine(CurrentResources, "commons");

        public static string Music = Path.Combine(CurrentResources, "music");

        public static string Sounds = Path.Combine(CurrentResources, "sounds");

        public static string Textures = Path.Combine(CurrentResources, "textures");

        public static string AnimatedTextures = Path.Combine(CurrentResources, "animatedtextures");
    }
}
