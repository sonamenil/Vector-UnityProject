using System;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    public ScrollRect _scrollRect;

    [SerializeField]
    public RectTransform _content;

    [SerializeField]
    private int _childCount;

    [SerializeField]
    private float _scrollWidth;

    [SerializeField]
    private float _contentDeltaWidth;

    [SerializeField]
    private float _contentWidth;

    [SerializeField]
    private float _spacing;

    [SerializeField]
    private float _duration = 0.5f;

    private int _currentIndex;

    [SerializeField]
    private int _currentDragIndex;

    public int StartIndex;

    public int EndIndex;

    public float MinVelocity;

    public Action<int> SnapEvent;

    private Vector2 _lastDelta;

    [SerializeField]
    private int _radius;

    [SerializeField]
    private float _scale;

    private Tweener _mainTweener;
    
    private static PlayerInputActions  _actions;

    [SerializeField]
    private bool _disabled;

    public ScrollSnapItem CurrentItem => _content.GetChild(_currentIndex).GetComponent<ScrollSnapItem>();

    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            CurrentItem.Deselect();
            _currentIndex = value;
            CurrentItem.Select();
            SnapEvent?.Invoke(value);
        }
    }

    private void Awake()
    {
        if (_actions == null)
            _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _actions.Enable();
        
        _actions.UI.Left.performed += ScrollToLeft;
        _actions.UI.Right.performed += ScrollToRight;
    }

    private void OnDisable()
    {
        _actions.UI.Left.performed -= ScrollToLeft;
        _actions.UI.Right.performed -= ScrollToRight;
        _actions.Disable();
    }

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _content = _scrollRect.content;
        _childCount = _content.childCount;
        _scrollWidth = ((RectTransform)_scrollRect.transform).rect.width;
        _contentDeltaWidth = _content.sizeDelta.x;
        _contentWidth = (_content.transform.GetChild(0).transform as RectTransform).rect.width;
        _spacing = Math.Abs(_content.GetComponent<HorizontalLayoutGroup>().spacing);
        _content.ForceUpdateRectTransforms();
    }

    public void Recalculate()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _content = _scrollRect.content;
        _childCount = _content.childCount;
        _scrollWidth = ((RectTransform)_scrollRect.transform).rect.width;
        _contentDeltaWidth = _content.sizeDelta.x;
        _contentWidth = (_content.transform.GetChild(0).transform as RectTransform).rect.width;
        _spacing = Math.Abs(_content.GetComponent<HorizontalLayoutGroup>().spacing);
        _content.ForceUpdateRectTransforms();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_mainTweener != null)
        {
            _mainTweener.Kill();
        }
    }

    private int GetCurrentIndex()
    {
        return Mathf.RoundToInt(GetCenterPos() / (_contentWidth + _spacing));
    }

    private int GetIndex(float pos)
    {
        return Mathf.RoundToInt(pos / (_contentWidth + _spacing));
    }

    private float GetCenterPos()
    {
        return _contentDeltaWidth * _scrollRect.horizontalNormalizedPosition + _scrollWidth * 0.5f - _contentWidth * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData != null)
        {
            _lastDelta = eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int index = GetCurrentIndex();
        var magnitude = _scrollRect.velocity.magnitude;
        if (MinVelocity <= magnitude)
        {
            _scrollRect.velocity = Vector2.zero;
            var num = Mathf.Sign(_lastDelta.x);
            var num2 = Mathf.Sqrt(magnitude / 1000);
            int num3 = (int)Mathf.Clamp(index - num2 * num, StartIndex, EndIndex);
            Snap(num3, num2 * 0.35f, false);
        }
        else
        {
            Snap(Mathf.Clamp(index, StartIndex, EndIndex), _duration, false);
        }
    }

    private void Update()
    {
        if (_disabled)
            return;
        
        var index = GetCurrentIndex();
        if (index != _currentIndex)
        {
            if (_content.transform.childCount > 0)
            {
                CurrentIndex = Mathf.Clamp(index, StartIndex, EndIndex);
            }
        }
        Scale(_radius, _scale);
    }

    public void ScrollToLeft(InputAction.CallbackContext ctx = default)
    {
        if (_disabled)
            return;
        
        int target = Mathf.Clamp(_currentIndex - 1, StartIndex, EndIndex);
        Snap(target, _duration, false);
    }

    public void ScrollToRight(InputAction.CallbackContext ctx = default)
    {
        if (_disabled)
            return;
        
        int target = Mathf.Clamp(_currentIndex + 1, StartIndex, EndIndex);
        Snap(target, _duration, false);
    }

    private void Scale(int radius, float scale)
    {
        var center = GetCenterPos();
        foreach (Transform child in _content.transform)
        {
            var transform = child.GetComponent<RectTransform>();
            float num = (_contentWidth + _spacing) * radius;
            num = Mathf.Clamp((num - Mathf.Abs(transform.anchoredPosition.x - center)) / num, 0, 1);
            var snapItem = child.GetComponent<ScrollSnapItem>();
            if (snapItem != null)
            {
                var vector = Vector3.one * (num * scale + 1);
                snapItem.Content.localScale = vector;
            }
        }
    }

    public void Snap(int index, bool instant)
    {
        Snap(index, _duration, instant);
    }

    private void Snap(int index, float duration, bool instant)
    {

        float val = ((index + 0.5f) * (_contentWidth + _spacing) - _scrollWidth * 0.5f - _spacing * 0.5f) / _contentDeltaWidth;

        if (instant)
        {
            _scrollRect.horizontalNormalizedPosition = val;
            CurrentIndex = index;
            //SnapEvent?.Invoke(index);
            return;
        }

        _mainTweener?.Kill();
        _mainTweener = TweenToNormalized(val, duration);


    }
    
    [ContextMenu("Disable")]
    public void Disable()
    {
        _disabled = true;
        if (_childCount > 0 && CurrentItem != null)
        {
            CurrentItem.Deselect();
            CurrentItem.Content.localScale = Vector3.one;
        }
    }
    
    [ContextMenu("Enable")]
    public void Enable()
    {
        _disabled = false;
        if (_childCount > 0 && CurrentItem != null)
        {
            CurrentItem.Select();
            Scale(_radius, _scale);
        }

    }

    private Tweener TweenToNormalized(float normalized, float duration)
    {
        var getter = new DOGetter<float>(() => _scrollRect.horizontalNormalizedPosition);
        var setter = new DOSetter<float>(x => _scrollRect.horizontalNormalizedPosition = x);
        _mainTweener = DOTween.To(getter, setter, normalized, duration).SetEase(Ease.OutQuint);
        return _mainTweener;
    }
}
