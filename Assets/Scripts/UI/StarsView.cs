using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class StarsView : MonoBehaviour
	{
		public List<StarView> StarViews;

		public void Set(int number)
		{
			foreach (var star in StarViews)
			{
				star.Disable();
			}
			for (int i = 0; i < number; i++)
			{
				StarViews[i].Enable();
			}
		}

		public Sequence SetWithAnimation(int starRecord, int[] coins)
		{
            foreach (var star in StarViews)
            {
                star.Disable();
            }

			var s = DOTween.Sequence();
			s.AppendInterval(0.5f);
            for (int i = 0; i < starRecord; i++)
            {
				var t = StarViews[i].EnableWithAnimation(coins[i]);
				s.Append(t);
            }
            s.Play();
            return s;
        }
	}
}
