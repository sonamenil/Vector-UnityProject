using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class Snap : MonoBehaviour, IDragHandler, IEventSystemHandler, IEndDragHandler, IBeginDragHandler
	{
		public ScrollRect ScrollRect;

		public UnityEngine.UI.Button ButtonLeft;

		public UnityEngine.UI.Button ButtonRight;

		public HorizontalLayoutGroup HorizontalLayoutGroup;

		private float _shift;

		private float _basePos;

		private int _currentIndex;

		private Dictionary<int, float> _children;

		private float distFromPrevious;

		private float previousPosition;

		private void UpdateScrollRect(int newIndex)
		{
		}

		private void UpdateScrollRect()
		{
		}

		private void Update()
		{
		}

		private void Start()
		{
		}

		private void IncreaseIndex()
		{
		}

		private void DecreaseIndex()
		{
		}

		public void OnDrag(PointerEventData eventData)
		{
		}

		public void OnEndDrag(PointerEventData eventData)
		{
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
		}

		private RectTransform FindNearestItem()
		{
			return null;
		}
	}
}
