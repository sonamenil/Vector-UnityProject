using UnityEngine;

public class PopUPAnimation : TweenAnimator
{
	[SerializeField]
	public CanvasGroup _shade;

	[SerializeField]
	public RectTransform _plate;

	[SerializeField]
	public RectTransform _holderB;

	[SerializeField]
	public CanvasGroup _lblHead;

	[SerializeField]
	public CanvasGroup[] _text;

	public void PresentPopUp()
	{
	}
}
