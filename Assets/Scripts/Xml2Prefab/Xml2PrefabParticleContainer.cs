using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabParticleContainer : MonoBehaviour
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
		private string _transforms;

		[SerializeField]
		private ChoiceContainer _choice;

		public string Name => _name;

		public float X => _x;

		public float Y => _y;

		public float W => _w;

		public float H => _h;

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

		public void Init(string n, float x, float y, float w, float h, string transforms, ChoiceContainer choice)
		{
            _name = n;
            _x = x;
            _y = y;
            _w = w;
            _h = h;
            _transforms = transforms;
            _choice = choice;
        }
	}
}
