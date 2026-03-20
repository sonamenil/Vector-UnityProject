using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabSpawnContainer : MonoBehaviour
	{
		[SerializeField]
		private float _x;

		[SerializeField]
		private float _y;

		[SerializeField]
		private string _name;

		[SerializeField]
		private string _animation;

		[SerializeField]
		private string _transformations;

		[SerializeField]
		private ChoiceContainer _choice;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

		public string Name => _name;

		public string Animation => _animation;

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

		public void Init(float x, float y, string n, string anima, string transformations, ChoiceContainer choice)
		{
            _x = x;
            _y = y;
            _name = n;
            _animation = anima;
            _transformations = transformations;
            _choice = choice;
        }

        private void OnDrawGizmos()
        {
            var color = Color.blue;
            color.a = 0.8f;
            Gizmos.color = color;
            Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y), new Vector3(100, 100));
        }
    }
}
