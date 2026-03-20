using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchButtonAnimator : TweenAnimator, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
    [SerializeField]
    protected RectTransform _scaleTransform;

    [SerializeField]
    protected CanvasGroup _alphaChangeImage;

    [SerializeField]
    protected bool _disableImageOnNormal = true;

    [SerializeField]
    protected float _normalAlpha;

    [SerializeField]
    protected float _pressedAlpha = 1;

    [SerializeField]
    protected float _normalScale = 1;

    [SerializeField]
    protected float _pressedScale = 1.2f;

    [SerializeField]
    protected float _transitionTime = 0.2f;

    [SerializeField]
    private bool _disabled;

    public Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();

    public bool Disabled
    {
        get
        {
            return _disabled;
        }
        set
        {
            _disabled = value;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_disabled)
        {
            return;
        }
        PressedTransition();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_disabled)
        {
            NormalTransition();
            return;
        }
        onClick.Invoke();
    }

    public void PressedTransition()
    {
        InitSequence();
        if (_scaleTransform != null)
        {
            var t = ShortcutExtensions.DOScale(_scaleTransform, _pressedScale, _transitionTime);
            TweenSettingsExtensions.Insert(_sequence, 0, t);
        }
        if (_alphaChangeImage == null)
        {
            TweenExtensions.Play(_sequence);
            return;
        }
        _alphaChangeImage.gameObject.SetActive(true);
        _alphaChangeImage.alpha = _normalAlpha;
        var t2 = DOTweenModuleUI.DOFade(_alphaChangeImage, _pressedAlpha, _transitionTime);
        TweenSettingsExtensions.Insert(_sequence, 0, t2);
        TweenExtensions.Play(_sequence);
    }

    public void NormalTransition()
    {
        InitSequence();
        if (_scaleTransform != null)
        {
            var t = ShortcutExtensions.DOScale(_scaleTransform, _normalScale, _transitionTime);
            TweenSettingsExtensions.Insert(_sequence, 0, t);
        }
        if (_alphaChangeImage != null)
        {
            var t2 = DOTweenModuleUI.DOFade(_alphaChangeImage, _normalAlpha, _transitionTime);
            TweenSettingsExtensions.Insert(_sequence, 0, t2);
            if (_disableImageOnNormal)
            {
                var callback = new TweenCallback(() => _alphaChangeImage.gameObject.SetActive(false));
                TweenSettingsExtensions.OnComplete(t2, callback);
            }
        }
        var callback2 = new TweenCallback(() => onClick.Invoke());
        TweenSettingsExtensions.AppendCallback(_sequence, callback2);
        TweenExtensions.Play(_sequence);
    }

    public void HighlightedTransition()
    {
    }
}
