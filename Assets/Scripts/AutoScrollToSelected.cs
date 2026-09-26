using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoScrollToSelected : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    [Header("Scrolling")]
    [SerializeField] private float scrollSpeed = 12f;
    [SerializeField] private float padding = 15f;

    private RectTransform viewport;
    private RectTransform content;

    private GameObject lastSelected;

    private Vector2 targetPosition;
    private bool isScrolling;

    private void Awake()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();

        viewport = scrollRect.viewport;
        content = scrollRect.content;

        targetPosition = content.anchoredPosition;
    }

    private void Update()
    {
        if (EventSystem.current == null)
            return;

        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null && selected != lastSelected)
        {
            lastSelected = selected;

            RectTransform selectedRect = selected.GetComponent<RectTransform>();

            if (selectedRect != null && selectedRect.IsChildOf(content))
            {
                RectTransform row = GetContentRow(selectedRect);

                if (row != null)
                    CalculateTargetPosition(row);
            }
        }

        if (isScrolling)
        {
            content.anchoredPosition = Vector2.Lerp(
                content.anchoredPosition,
                targetPosition,
                Time.unscaledDeltaTime * scrollSpeed
            );

            if (Vector2.Distance(content.anchoredPosition, targetPosition) < 0.1f)
            {
                content.anchoredPosition = targetPosition;
                isScrolling = false;
            }
        }
    }

    private RectTransform GetContentRow(RectTransform selected)
    {
        Transform current = selected;

        while (current != null && current.parent != content)
        {
            current = current.parent;
        }

        if (current != null && current.parent == content)
            return current as RectTransform;

        return selected;
    }

    private void CalculateTargetPosition(RectTransform selectedRow)
    {
        Canvas.ForceUpdateCanvases();

        Bounds rowBounds =
            RectTransformUtility.CalculateRelativeRectTransformBounds(
                viewport,
                selectedRow
            );

        Rect viewportRect = viewport.rect;

        Vector2 newTarget = content.anchoredPosition;

        // Row is below the viewport
        if (rowBounds.min.y < viewportRect.yMin + padding)
        {
            float difference =
                (viewportRect.yMin + padding) - rowBounds.min.y;

            newTarget.y += difference;
        }

        // Row is above the viewport
        else if (rowBounds.max.y > viewportRect.yMax - padding)
        {
            float difference =
                rowBounds.max.y - (viewportRect.yMax - padding);

            newTarget.y -= difference;
        }

        targetPosition = newTarget;
        isScrolling = true;
    }
}