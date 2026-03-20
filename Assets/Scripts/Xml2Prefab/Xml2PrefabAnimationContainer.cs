using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabAnimationContainer : MonoBehaviour
	{
		public float W;

		public float H;

		[SerializeField]
		private string _name;

		[SerializeField]
		private float _w;

		[SerializeField]
		private float _h;

		[SerializeField]
		private float _x;

		[SerializeField]
		private float _y;

		[SerializeField]
		private int _type;

		[SerializeField]
		private float _scaleX;

		[SerializeField]
		private float _scaleY;

		[SerializeField]
		private string _transformations;

		[SerializeField]
		private ChoiceContainer _choice;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

		public int Type => _type;

		public float ScaleX => _scaleX;

		public float ScaleY => _scaleY;

		public string Transformations => _transformations;

		public string Name => _name;

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

		public void Init(string n, float x, float y, float w, float h, int type, float scaleX, float scaleY, string transformationData, ChoiceContainer choice)
		{
            _name = n;
            _x = x;
            _y = y;
            _w = w;
            _h = h;
            _type = type;
            _scaleX = scaleX;
            _scaleY = scaleY;
            _transformations = transformationData;
            _choice = choice;
        }
	}
}
