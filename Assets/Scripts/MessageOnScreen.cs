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

        var fade = _text.DOFade(1, 0.5f);
        _sequence.Append(fade);
        _sequence.AppendInterval(timeInFrame / 60);

        var fade1 = _text.DOFade(0, 0.5f);
        _sequence.Append(fade1);

        var action = new TweenCallback(() => _text.gameObject.SetActive(false));
        _sequence.OnKill(action);
        _sequence.Play();
    }

    private void Reset()
    {
        _sequence.Kill();
        _text.gameObject.SetActive(false);
    }
}
