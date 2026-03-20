using System.Collections.Generic;
using System.Linq;

namespace Nekki.Vector.Core.Animation
{
	public static class Animations
	{
		public static Dictionary<string, AnimationInfo> Animation
		{
			get;
		}

		static Animations()
		{
			Animation = new Dictionary<string, AnimationInfo>();
		}

		public static List<AnimationInfo> ToList()
		{
			return Animation.Values.ToList();
		}
	}
}
