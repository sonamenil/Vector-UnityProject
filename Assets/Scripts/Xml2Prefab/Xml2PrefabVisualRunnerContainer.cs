using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabVisualRunnerContainer : MonoBehaviour
	{
		[SerializeField]
		private int _type;

		[SerializeField]
		private string _name;

		[SerializeField]
		private float _imageWidth;

		[SerializeField]
		private float _imageHeight;

		[SerializeField]
		private Color _defaultColor = Color.white;

		[SerializeField]
		private int _depth;

		[SerializeField]
		private float _x;

		[SerializeField]
		private float _y;

		[SerializeField]
		private string _matrixTransform;

		[SerializeField, TextArea(10, 25)]
		private string _transforms;

		[SerializeField]
		private ChoiceContainer _choice;

		public int Type => _type;

		public string Name => _name;

		public float ImageWidth => _imageWidth;

		public float ImageHeight => _imageHeight;

		public Color DefaultColor => _defaultColor;

		public int Depth => _depth;

		public string MatrixTransform => _matrixTransform;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

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

		public void Init(int type, string n, float w, float h, Color color, int depth, string matrixTransform, float x, float y, string transforms, ChoiceContainer choice)
		{
			_type = type;
			_name = n;
			_imageWidth = w;
			_imageHeight = h;
			_defaultColor = color;
			_depth = depth;
			_matrixTransform = matrixTransform;
			_x = x;
			_y = y;
			_transforms = transforms;
			_choice = choice;
		}
	}
}
