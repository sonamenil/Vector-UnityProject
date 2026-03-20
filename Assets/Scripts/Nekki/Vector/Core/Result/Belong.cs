using Nekki.Vector.Core.Detector;
using Nekki.Vector.Core.Location;

namespace Nekki.Vector.Core.Result
{
	public class Belong
	{
		private QuadRunner _Platform;

		private DetectorLine _Detector;

		public QuadRunner Platform => _Platform;

		public DetectorLine Detector => _Detector;

		public Belong(QuadRunner platform, DetectorLine detector)
		{
			_Platform = platform;
			_Detector = detector;
		}
	}
}
