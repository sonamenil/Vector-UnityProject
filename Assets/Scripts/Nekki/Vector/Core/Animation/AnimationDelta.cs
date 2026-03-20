using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

namespace Nekki.Vector.Core.Animation
{
	public class AnimationDelta
	{
		private readonly int _corner;

		public Point Value
		{
			get;
		}

		public AnimationDeltaName Name
		{
			get;
		}

		public AnimationDeltaType Type
		{
			get;
		}

		public AnimationDelta(AnimationDeltaName name, AnimationDeltaType type, Point value, int corner)
		{
			Name = name;
			_corner = corner;
			Value = value;
			Type = type;
		}

		public int GetCorner(int sign)
		{
            if (sign == -1)
            {
                switch (_corner)
                {
                    case 0:
                        return 1;
                    case 1:
                        return 0;
                    case 2:
                        return 3;
                    case 3:
                        return 2;
                }
            }
            return _corner;
        }

		public bool IsInterval(double value)
		{
            return More(value, Value.X) && Less(value, Value.Y);
        }

		public static bool More(double target, float value)
		{
            return float.IsNaN(value) || target >= value;
        }

        public static bool Less(double target, float value)
		{
            return float.IsNaN(value) || target <= value;
        }
    }
}
