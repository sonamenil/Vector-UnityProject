using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabTriggerContainer : MonoBehaviour
	{
		[SerializeField]
		private float _w;

		[SerializeField]
		private float _h;

		[SerializeField, TextArea(10, 25)]
		private string _Node;

		[SerializeField]
		private ChoiceContainer _choice;

		public bool DisabledByVariant;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

		public string Node => _Node;

		public float H => _h;

		public float W => _w;

		public ChoiceContainer Choice
		{
			get => _choice;
			set => _choice = value;
		}

		public void Init(string node, float h, float w, ChoiceContainer choice)
		{
			_Node = node;
			_h = h;
			_w = w;
			_choice = choice;
			CreateInnerController();
		}

		public void CreateInnerController()
		{
			if (Game.Instance.SnailSett.ShowTriggers)
			{
				var controller = new GameObject("Controller");
				controller.transform.SetParent(transform, false);
				controller.AddComponent<TriggerController>().Container = this;
			}
        }

		public void ChangeHW(float h, float w)
		{
			_h = h;
			_w = w;
		}
	}
}
