using UnityEngine;

namespace Nekki.Vector.Core.Utilites
{
	public static class Math
	{
		public static double Round(double Value, double Pow)
		{
            return (double)System.Math.Floor(Value * Pow + 0.5) / Pow;
        }

		public static float Round(float Value, float Pow)
		{
            return Mathf.Floor(Value * Pow + 0.5f) / Pow;
        }
	}
}
