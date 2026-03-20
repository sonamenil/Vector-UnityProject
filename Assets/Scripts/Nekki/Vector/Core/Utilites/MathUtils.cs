using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Nekki.Vector.Core.Utilites
{
	public static class MathUtils
	{
		public static float Round(float value, float pow)
		{
            return Mathf.Floor(value * pow + 0.5f) / pow;
        }

		public static float AngleBetweenPoints(Vector2 a, Vector2 b)
		{
            float x = b.x - a.x;
            float y = b.y - a.y;
            return Mathf.Atan2(y, x) * 57.29578f;
        }

		public static float NormalizeAngle(float angle)
		{
            return angle % 360f + (float)((angle < 0f) ? 360 : 0);
        }

        public static bool LineRectIntersection(Vector2 lineStartPoint, Vector2 lineEndPoint, Rect rectangle, ref Vector2 result)
		{
            Vector2 vector = ((!(lineStartPoint.x <= lineEndPoint.x)) ? lineEndPoint : lineStartPoint);
            Vector2 vector2 = ((!(lineStartPoint.x <= lineEndPoint.x)) ? lineStartPoint : lineEndPoint);
            Vector2 vector3 = ((!(lineStartPoint.y <= lineEndPoint.y)) ? lineEndPoint : lineStartPoint);
            Vector2 vector4 = ((!(lineStartPoint.y <= lineEndPoint.y)) ? lineStartPoint : lineEndPoint);
            float xMax = rectangle.xMax;
            float xMin = rectangle.xMin;
            float yMax = rectangle.yMax;
            float yMin = rectangle.yMin;
            if (vector.x <= xMax && xMax <= vector2.x)
            {
                float num = (vector2.y - vector.y) / (vector2.x - vector.x);
                float num2 = (xMax - vector.x) * num + vector.y;
                if (vector3.y <= num2 && num2 <= vector4.y && yMin <= num2 && num2 <= yMax)
                {
                    result = new Vector2(xMax, num2);
                    return true;
                }
            }
            if (vector.x <= xMin && xMin <= vector2.x)
            {
                float num3 = (vector2.y - vector.y) / (vector2.x - vector.x);
                float num4 = (xMin - vector.x) * num3 + vector.y;
                if (vector3.y <= num4 && num4 <= vector4.y && yMin <= num4 && num4 <= yMax)
                {
                    result = new Vector2(xMin, num4);
                    return true;
                }
            }
            if (vector3.y <= yMax && yMax <= vector4.y)
            {
                float num5 = (vector4.x - vector3.x) / (vector4.y - vector3.y);
                float num6 = (yMax - vector3.y) * num5 + vector3.x;
                if (vector.x <= num6 && num6 <= vector2.x && xMin <= num6 && num6 <= xMax)
                {
                    result = new Vector2(num6, yMax);
                    return true;
                }
            }
            if (vector3.y <= yMin && yMin <= vector4.y)
            {
                float num7 = (vector4.x - vector3.x) / (vector4.y - vector3.y);
                float num8 = (yMin - vector3.y) * num7 + vector3.x;
                if (vector.x <= num8 && num8 <= vector2.x && xMin <= num8 && num8 <= xMax)
                {
                    result = new Vector2(num8, yMin);
                    return true;
                }
            }
            return false;
        }
	}
}
