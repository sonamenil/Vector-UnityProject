using UnityEngine;

public class AutoWidth : MonoBehaviour
{
    public float Width = 115;

    public RectTransform RectTransform;

    private void Update()
    {
        if (RectTransform != null)
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
        }
    }
}
