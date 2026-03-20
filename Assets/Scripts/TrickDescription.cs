using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrickDescription : MonoBehaviour
{
	[SerializeField]
	private Text _text;

	private RectTransform _cachedRectTransform;

	private Vector2 _defaultPosition;

	private Sequence _mainSequence;

	private void Awake()
	{
		_cachedRectTransform = _text.rectTransform;
		_defaultPosition = _cachedRectTransform.anchoredPosition;
		_text.gameObject.SetActive(false);
	}

	public void Show(string trickName, string score)
	{
		var id = LocalizationManager.Instance.GetTranslationByID(trickName);
		Show($"{id}\n+{score}");
	}

	public void ShowTrickNotBuy()
	{
		var texttoshow = LocalizationManager.Instance.GetTranslation("track_locked_txt");
		Show(texttoshow);
	}

	private void Show(string textToShow)
	{
		var text = _text;
		if (_mainSequence == null)
		{
			_mainSequence = Show(text, _cachedRectTransform, _defaultPosition, false, textToShow);
			TweenSettingsExtensions.OnKill(_mainSequence, new TweenCallback(() => _mainSequence = null));
			return;
		}
        text = Instantiate(text, _cachedRectTransform.parent);
		Show(text, text.rectTransform, _defaultPosition, true, textToShow);
	}

	private static Sequence Show(Text text, RectTransform rTransform, Vector2 defPos, bool isTemp, string textToShow)
	{
        text.color = Color.white;
		text.text = textToShow;
        text.gameObject.SetActive(true);
		rTransform.anchoredPosition = defPos;
		var s = DOTween.Sequence();
		s.AppendInterval(0.4f);
		var t = DOTweenModuleUI.DOFade(text, 0, 1.3000001f);
		s.Append(t);
		var s1 = DOTween.Sequence();
		var t1 = DOTweenModuleUI.DOAnchorPosY(rTransform, defPos.y + 150, 1.7f, false);
		s1.Append(t1);
		s1.Join(s);
		s1.AppendCallback(new TweenCallback(() => text.gameObject.SetActive(false)));
		s1.Play();
		return s1;
	}
}
