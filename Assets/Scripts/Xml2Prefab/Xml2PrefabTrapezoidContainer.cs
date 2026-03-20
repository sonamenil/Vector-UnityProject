using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabTrapezoidContainer : MonoBehaviour
	{
		[SerializeField]
		private string _className;

		[SerializeField]
		private int _type;

		[SerializeField]
		private float _x;

		[SerializeField]
		private float _y;

		[SerializeField]
		private float _width;

		[SerializeField]
		private float _height;

		[SerializeField]
		private float _height1;

		[SerializeField]
		private bool _sticky;

		[SerializeField]
		private string _transforms;

		[SerializeField]
		private ChoiceContainer _choice;

		public string ClassName => _className;

		public int Type => _type;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

		public float Width => _width;

		public float Height => _height;

		public float Height1 => _height1;

		public bool Sticky => _sticky;

		public string Transforms => _transforms;

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

		public void Init(string name, int type, float x, float y, float width, float height, float height1, bool sticky, string transforms, ChoiceContainer choice)
		{
            _className = name;
            _type = type;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _height1 = height1;
            _sticky = sticky;
            _transforms = transforms;
            _choice = choice;
        }
	}
}
