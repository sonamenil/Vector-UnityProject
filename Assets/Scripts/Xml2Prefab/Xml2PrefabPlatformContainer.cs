using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabPlatformContainer : MonoBehaviour
	{
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
		private bool _sticky;

		[SerializeField]
		private string _transformations;

		[SerializeField]
		private ChoiceContainer _choice;

		public string Name => _name;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

		public float W => _w;

		public float H => _h;

		public bool Sticky => _sticky;

		public string Transformations => _transformations;

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

		public void Init(string n, float x, float y, float w, float h, bool s, string transformations, ChoiceContainer choice)
		{
			_name = n;
			_x = x;
			_y = y;
			_w = w;
			_h = h;
			_transformations = transformations;
			_choice = choice;
			_sticky = s;

            gameObject.AddComponent<PlatformController>().Container = this;
        }

		public void ChangeHW(float h, float w)
		{
			_h = h;
			_w = w;
		}
	}
}
