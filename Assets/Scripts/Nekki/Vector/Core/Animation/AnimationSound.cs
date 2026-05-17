using System.Collections.Generic;
using UnityEngine;

namespace Nekki.Vector.Core.Animation
{
	public class AnimationSound
	{
		private List<string> _names;

		public AnimationSound(string name, int type)
		{
			_names = new List<string>(name.Split('|'));
		}

		public void Play(float p_volume = 1f) //, float p_pitch = 1f)
		{
			float pitch = Random.Range(0.99f, 1.01f);
			var num = Random.Range(0, _names.Count);
			SoundsManager.Instance.PlaySoundsOnce(_names[num], p_volume, pitch);
		}
	}
}
