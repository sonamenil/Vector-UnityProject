using UnityEngine;

namespace Xml2Prefab
{
	[ExecuteInEditMode]
	public class Xml2PrefabItemContainer : MonoBehaviour
	{
		[SerializeField]
		private int _type;

		[SerializeField]
		private float _x;

		[SerializeField]
		private float _y;

		[SerializeField]
		private string _transformData;

		[SerializeField]
		private ChoiceContainer _choice;

		public string TransformationData => _transformData;

		public int Type => _type;

		public float X => transform.localPosition.x;

		public float Y => transform.localPosition.y;

		public ChoiceContainer Choice
		{
			get => _choice;
			set => _choice = value;
		}

		public void Init(int type, float x, float y, string transformData, ChoiceContainer choice)
		{
			_type = type;
			_x = x;
			_y = y;
			_transformData = transformData;
			_choice = choice;
		}


        private void OnDrawGizmos()
		{
			var color = Color.white;
			if (Type == 0)
			{
				color = Color.green;
			}
			else
			{
				color = Color.magenta;
			}
			color.a = 0.8f;
			Gizmos.color = color;
			Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y), new Vector3(100, 100));
        }

    }
}
