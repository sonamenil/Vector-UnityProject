using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Nekki.Vector.Core.Animation
{
	public class AnimationDeltas
	{
		private AnimationDeltaName _name;

		private readonly List<AnimationDelta> _deltas = new List<AnimationDelta>();

		public AnimationDeltas(XmlNode mainNode, AnimationDeltaType type)
		{
			foreach (XmlNode node in mainNode.ChildNodes)
			{
				if (GetDeltaTypeByString(XmlUtils.ParseString(node.Attributes["Type"], "")) == type)
				{
                    Point p_value = new Point(XmlUtils.ParseFloat(node.Attributes["Min"], float.NaN), XmlUtils.ParseFloat(node.Attributes["Max"], float.NaN));
                    int p_corner = XmlUtils.ParseInt(node.Attributes["Corner"], -1);
                    _deltas.Add(new AnimationDelta(GetDeltaNameByString(node.Name), type, p_value, p_corner));
                }
			}
		}

		public static AnimationDeltaName GetDeltaNameByString(string name)
		{
            switch (name)
            {
                case "Width":
                    return AnimationDeltaName.Width;
                case "Height":
                    return AnimationDeltaName.Height;
                case "DeltaX":
                    return AnimationDeltaName.DeltaX;
                case "DeltaY":
                    return AnimationDeltaName.DeltaY;
                case "VelosityX":
                    return AnimationDeltaName.VelocityX;
                case "VelosityY":
                    return AnimationDeltaName.VelocityY;
                default:
                    return AnimationDeltaName.Max;
            }
        }

		public static AnimationDeltaType GetDeltaTypeByString(string type)
		{
            switch (type)
            {
                case "H":
                    return AnimationDeltaType.Horizontal;
                case "V":
                    return AnimationDeltaType.Vertical;
                case "C":
                    return AnimationDeltaType.Collision;
                default:
                    return AnimationDeltaType.Max;
            }
        }

		public bool IsCheck(AnimationDeltaData deltaSort, int sign, Vector3d velocity)
		{
            if (_deltas.Count == 0)
            {
                return true;
            }
            if (deltaSort == null && _deltas.Count > 0)
            {
                return false;
            }
            foreach (AnimationDelta delta in _deltas)
            {
                if (!IsDeltaName(delta, deltaSort, sign, velocity))
                {
                    return false;
                }
            }
            return true;
        }

		public bool IsDeltaName(AnimationDelta delta, AnimationDeltaData deltaData, int sign, Vector3d velocity)
		{
            int corner = delta.GetCorner(sign);
            double deltaValue = deltaData.GetDeltaValue(delta.Name, corner, sign, velocity);
            return delta.IsInterval(deltaValue);
        }
	}
}
