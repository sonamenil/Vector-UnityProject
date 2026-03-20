using Nekki.Vector.Core.Frame;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nekki.Vector.Core.Animation
{
    public static class AnimationBinaryParser
    {
        private static readonly Dictionary<string, Provider> CachedBinary = new Dictionary<string, Provider>();

        public static void ClearCachedBinary()
        {
            CachedBinary.Clear();
        }

        public static Provider ParseFile(string fileName, bool useCache = true)
        {
            if (useCache)
            {
                if (CachedBinary.ContainsKey(fileName))
                {
                    return CachedBinary[fileName];
                }
            }
            var bytes = ResourceManager.GetBinary(fileName);
            if (bytes == null)
            {
                return new Provider(0);
            }
            var stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);

            int count = reader.ReadInt32();
            var provider = new Provider(count);

            for (int i = 0; i < count; i++)
            {
                reader.ReadByte();
                int num = reader.ReadInt32();
                Vector3[] array = new Vector3[num];

                for (int j = 0; j < num; j++)
                {
                    array[j].x = reader.ReadSingle();
                    array[j].y = 0f - reader.ReadSingle();
                    array[j].z = reader.ReadSingle();

                }
                provider.Add(array, i);
            }
            reader.Close();
            stream.Close();
            if (useCache)
            {
                CachedBinary.Add(fileName, provider);
            }
            return provider;
        }
    }
}
