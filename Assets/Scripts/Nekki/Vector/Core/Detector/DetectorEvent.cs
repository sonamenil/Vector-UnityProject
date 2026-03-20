using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Node;
using System.Runtime.InteropServices;
using static Nekki.Vector.Core.Node.NodeName;

namespace Nekki.Vector.Core.Detector
{
	public class DetectorEvent
	{
		public enum DetectorEventType
		{
			None,
			On,
			Off
		}

		public DetectorLine Detector
		{
			get;
		}

		public DetectorEventType Type
		{
			get;
		}

		public int Side
		{
			get;
		}

		public QuadRunner Platform => Detector.Platform;

		public ModelNode Node => Detector.Node;

		public bool IsVertical => Detector.Type == DetectorLine.DetectorType.Vertical;

		public bool IsHorizontal => Detector.Type == DetectorLine.DetectorType.Horizontal;

		public DetectorEvent(DetectorLine detector, DetectorEventType type, int side)
		{
            Detector = detector;
            Type = type;
            Side = side;
        }

		public void DeltaPosition(Vector3d velocity)
		{
            if (Type == DetectorEventType.On)
            {
                Detector.DeltaPosition(Platform.DeltaEdge(Detector.Position, Side).Add(velocity));
            }
        }
	}
}
