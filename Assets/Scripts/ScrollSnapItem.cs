using UnityEngine;
using UnityEngine.UI;

public class ScrollSnapItem : MonoBehaviour
{
	public RectTransform Content;

	public Text SelectionText;

	public Image SelectionImage;

	public Color SelectionColor;

	public Color RegularColor;

	public bool IsSelected;

	public void Select()
	{
        if (SelectionText != null)
        {
            SelectionText.color = SelectionColor;
        }
        if (SelectionImage != null)
        {
            SelectionImage.color = SelectionColor;
        }
        IsSelected = transform;
    }

	public void Deselect()
	{
        if (SelectionText != null)
        {
            SelectionText.color = RegularColor;
        }
		if (SelectionImage != null)
		{
			SelectionImage.color = RegularColor;
		}
		IsSelected = false;
    }
}
