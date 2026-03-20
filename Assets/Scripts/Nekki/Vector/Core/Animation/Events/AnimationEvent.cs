using System.Collections.Generic;

namespace Nekki.Vector.Core.Animation.Events
{
	public class AnimationEvent
	{
		public AnimationEventParam Param
		{
			get;
		}

		public List<AnimationReaction> Reaction => Param.Reaction;

		public List<AnimationSound> Sound => Param.Sound;

		public AnimationEvent(AnimationEventParam param)
		{
			Param = param;
		}
	}
}
