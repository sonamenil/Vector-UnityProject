using DG.Tweening;
using UnityEngine;

public class TweenAnimator : MonoBehaviour
{
	protected Sequence _sequence;

	protected void InitSequence()
	{
		Stop();
		_sequence = DOTween.Sequence();
	}

	public virtual void Stop()
	{
		if ( _sequence != null )
		{
			TweenExtensions.Kill(_sequence, false);
		}
	}

	public void SetTimescale(float timeScale)
	{
		if (_sequence != null )
		{
			_sequence.timeScale = timeScale;
		}
	}
}
