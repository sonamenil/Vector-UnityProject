using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutPosKeeper : MonoBehaviour
{
    public List<LayoutElement> left = new List<LayoutElement>();

    public List<LayoutElement> right = new List<LayoutElement>();

    public HorizontalLayoutGroup horizontalLayout;

    [ContextMenu("Set positions")]
    public void SetPositions()
    {
        if (horizontalLayout == null)
        {
            return;
        }

        SetLeft();
        Canvas.ForceUpdateCanvases();
        SetRight();

    }

    public void Clear()
    {
        left.Clear();
        right.Clear();
    }
    
    public void SetLeft()
    {
        if (left.Count > 0)
        {
            float leftX = -left[0].preferredWidth - horizontalLayout.spacing;

            for (int i = 0; i < left.Count; i++)
            {
                var item = left[i];
                item.ignoreLayout = true;
                var rt = item.GetComponent<RectTransform>();
                var pos = rt.anchoredPosition;
                pos.x = leftX * (i + 1);
                rt.anchoredPosition = pos;
            }
        }
    }

    public void SetRight()
    {
        if (right.Count > 0)
        {
            //var rectTransform = GetComponent<RectTransform>();

            //if (rectTransform == null)
            //{
            //    return;
            //}
            //var rrt = right[0].GetComponent<RectTransform>();
            //right[0].ignoreLayout = true;

            //Canvas.ForceUpdateCanvases();

            //float rightX = rectTransform.rect.width + horizontalLayout.spacing;

            //var apos = rrt.anchoredPosition;
            //apos.x = rightX;
            //rrt.anchoredPosition = apos;



            //for (int i = 1; i < right.Count; i++)
            //{
            //    var item = right[i];
            //    item.ignoreLayout = true;
            //    var rt = item.GetComponent<RectTransform>();
            //    var pos = rt.anchoredPosition;
            //    pos.x = rightX + (item.preferredWidth * i) + (horizontalLayout.spacing * i);
            //    rt.anchoredPosition = pos;
            //}
            for (int i = 0; i < right.Count; i++)
            {
                right[i].ignoreLayout = false;
            }
            
            Canvas.ForceUpdateCanvases();

            for (int i = 0; i < right.Count; i++)
            {

                right[i].ignoreLayout = true;
            }

            //Canvas.ForceUpdateCanvases();
        }
    }
}