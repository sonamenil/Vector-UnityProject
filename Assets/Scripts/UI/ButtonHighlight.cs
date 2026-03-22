using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private Text HighlightText;

    [SerializeField] private Button _button;
    
    [SerializeField]
    private Color _highlightColor;
    
    private Color _defaultColor;

    private void Start()
    {
        if (HighlightText != null)
        {
            _defaultColor = HighlightText.color;
        }
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        HighlightText.color = _highlightColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HighlightText.color = _defaultColor;
    }
}
