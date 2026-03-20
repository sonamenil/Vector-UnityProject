using System.Collections.Generic;

namespace Nekki.Vector.Core.Animation.Events
{
	public class AnimationEventParam
	{
		public List<AnimationReaction> Reaction
		{
			get;
		}

		public List<AnimationSound> Sound
		{
			get;
		}

		public AnimationEventParam(List<AnimationReaction> reaction, List<AnimationSound> sound)
		{
			Reaction = reaction;
			Sound = sound;
		}
	}
}
