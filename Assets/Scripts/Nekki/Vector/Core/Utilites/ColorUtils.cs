using System;
using UnityEngine;

namespace Nekki.Vector.Core.Utilites
{
	public static class ColorUtils
	{
		public static Color FromHex(string color, float opacity = 1f)
		{
            color = color.Replace("#", string.Empty);
            if (color.Length != 6 && color.Length != 8)
            {
                return new Color(0f, 0f, 0f, 1f);
            }
            int num = color.Length / 2;
            byte[] array = new byte[4]
            {
                0,
                0,
                0,
                (byte)(255f * opacity)
            };
            for (int i = 0; i < num; i++)
            {
                array[i] = Convert.ToByte(color.Substring(i * 2, 2), 16);
            }
            return new Color(array[0] / 255f, array[1] / 255f, array[2] / 255f, array[3] / 255f);
        }
	}
}
