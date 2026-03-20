using DG.Tweening;
using Nekki.Vector.Core.Trigger.Actions;
using UnityEngine;
using UnityEngine.UI;

public class MessageOnScreen : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private Sequence _sequence;

    private void Awake()
    {
        _text.gameObject.SetActive(false);
    }

    public void Show(string text, int timeInFrame, Color color, TA_MessageOnScreen.Animation appearAnimation, TA_MessageOnScreen.Animation disappearAnimation)
    {
        _text.gameObject.SetActive(true);
        _text.text = LocalizationManager.Instance.GetTranslation(text);
        _text.color = color;

        _sequence = DOTween.Sequence();

        var fade = DOTweenModuleUI.DOFade(_text, 1, 0.5f);
        TweenSettingsExtensions.Append(_sequence, fade);
        TweenSettingsExtensions.AppendInterval(_sequence, timeInFrame / 60);

        var fade1 = DOTweenModuleUI.DOFade(_text, 0, 0.5f);
        TweenSettingsExtensions.Append(_sequence, fade1);

        var action = new TweenCallback(() => _text.gameObject.SetActive(false));
        TweenSettingsExtensions.OnKill(_sequence, action);
        TweenExtensions.Play(_sequence);
    }

    private void Reset()
    {
        TweenExtensions.Kill(_sequence, false);
        _text.gameObject.SetActive(false);
    }
}
