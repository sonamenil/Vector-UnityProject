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
        get => _disabled;
        set => _disabled = value;
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
            var t = _scaleTransform.DOScale(_pressedScale, _transitionTime);
            _sequence.Insert(0, t);
        }
        if (_alphaChangeImage == null)
        {
            _sequence.Play();
            return;
        }
        _alphaChangeImage.gameObject.SetActive(true);
        _alphaChangeImage.alpha = _normalAlpha;
        var t2 = _alphaChangeImage.DOFade(_pressedAlpha, _transitionTime);
        _sequence.Insert(0, t2);
        _sequence.Play();
    }

    public void NormalTransition()
    {
        InitSequence();
        if (_scaleTransform != null)
        {
            var t = _scaleTransform.DOScale(_normalScale, _transitionTime);
            _sequence.Insert(0, t);
        }
        if (_alphaChangeImage != null)
        {
            var t2 = _alphaChangeImage.DOFade(_normalAlpha, _transitionTime);
            _sequence.Insert(0, t2);
            if (_disableImageOnNormal)
            {
                var callback = new TweenCallback(() => _alphaChangeImage.gameObject.SetActive(false));
                t2.OnComplete(callback);
            }
        }
        var callback2 = new TweenCallback(() => onClick.Invoke());
        _sequence.AppendCallback(callback2);
        _sequence.Play();
    }

    public void HighlightedTransition()
    {
    }
}
