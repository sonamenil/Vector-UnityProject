using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class StarView : MonoBehaviour
	{
		[SerializeField]
		private GameObject Empty;

		[SerializeField]
		private GameObject Full;

		[SerializeField]
		private Image _starLight;

		[SerializeField]
		private Text _startext;

		public void Disable()
		{
			Empty.SetActive(true);
			Full.SetActive(false);
		}

		public void Enable()
		{
            Empty.SetActive(false);
            Full.SetActive(true);
        }

		public Sequence EnableWithAnimation(int coins)
		{
			Empty.SetActive(true);
			_starLight.gameObject.SetActive(false);
			_starLight.color = Color.white;
			_starLight.transform.localScale = Vector3.one;
			_startext.gameObject.SetActive(false);
			_startext.color = Color.white;
			_startext.rectTransform.anchoredPosition = new Vector2();
			_startext.gameObject.SetActive(false);
			var s = DOTween.Sequence();
			s.AppendInterval(0.2f);
			if (coins > 0)
			{
				_startext.text = string.Format("+{0}", coins);
				var s1 = DOTween.Sequence();
				s1.AppendCallback(() =>
				{
					_startext.gameObject.SetActive(true);
				});
				s1.Append(DOTweenModuleUI.DOAnchorPosY(_startext.rectTransform, 200, 2, false));
				s1.Join(DOTweenModuleUI.DOFade(_startext, 0, 2));
				s1.Pause();
				s.AppendCallback(() =>
				{
					s1.Play();
				});
			}
			s.AppendCallback(() => 
			{
				Full.gameObject.SetActive(true);
			});
			s.Append(ShortcutExtensions.DOScale(Full.transform, new Vector3(2, 2, 1), 0.12f));
			s.AppendCallback(() =>
			{
				_starLight.gameObject.SetActive(true);
			});
			s.Append(ShortcutExtensions.DOScale(Full.transform, new Vector3(1, 1, 1), 0.12f));
            s.Append(ShortcutExtensions.DOScale(_starLight.transform, new Vector3(3, 3, 3), 0.2f));
			s.Append(DOTweenModuleUI.DOFade(_starLight, 0, 0.2f));
			return s;
        }
    }
}
