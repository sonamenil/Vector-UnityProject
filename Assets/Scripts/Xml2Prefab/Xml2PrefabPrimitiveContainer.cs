using System.Collections.Generic;
using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabPrimitiveContainer : MonoBehaviour
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
		private float _impulse;

		[SerializeField]
		private List<string> _sounds;

		[SerializeField]
		private string _transforms;

		[SerializeField]
		private ChoiceContainer _choice;

		public string ClassName => _className;

		public int Type => _type;

		public float X => _x;

		public float Y => _y;

		public float Impulse => _impulse;

		public List<string> Sounds => _sounds;

		public string Transforms => _transforms;

		public ChoiceContainer Choice
		{
			get => _choice;
			set => _choice = value;
		}

		public void Init(string className, int type, float x, float y, float impulse, List<string> sounds, string transforms, ChoiceContainer choice)
		{
            _className = className;
            _type = type;
            _x = x;
            _y = y;
            _impulse = impulse;
            _sounds = sounds;
            _transforms = transforms;
            _choice = choice;
        }
	}
}
