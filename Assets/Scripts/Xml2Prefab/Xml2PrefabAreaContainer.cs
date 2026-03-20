using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabAreaContainer : MonoBehaviour
	{
		[SerializeField]
		private string _transformations;

		[SerializeField]
		private string _type;

		[SerializeField]
		private string _name;

		[SerializeField]
		private float _x;

		[SerializeField]
		private float _y;

		[SerializeField]
		private float _w;

		[SerializeField]
		private float _h;

		[SerializeField]
		private ChoiceContainer _choice;

		public string Transformations => _transformations;

		public string Type => _type;

		public string Name => _name;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

		public float W => _w;

		public float H => _h;

		public ChoiceContainer Choice
		{
			get
			{
				return _choice;
			}
			set
			{
				_choice = value;
			}
		}

		public void Init(string transformations, string type, string n, float x, float y, float w, float h, ChoiceContainer choice)
		{
			_transformations = transformations;
			_type = type;
			_name = n;
			_x = x;
			_y = y;
			_w = w;
			_h = h;
			_choice = choice;

			gameObject.AddComponent<AreaController>().Container = this;
		}

		public void ChangeHW(float h, float w)
		{
			_w = w;
			_h = h;
		}
	}
}
