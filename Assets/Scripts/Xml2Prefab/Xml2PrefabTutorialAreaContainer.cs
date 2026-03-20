using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabTutorialAreaContainer : Xml2PrefabAreaContainer
	{
		[SerializeField]
		private string _key;

		[SerializeField]
		private string _description;

		public string Key => _key;

		public string Description => _description;

		public void Init(string transformations, string type, string n, float x, float y, float w, float h, string key, string description, ChoiceContainer choice)
		{
			Init(transformations, type, n, x, y, w, h, choice);
			_description = description;
			_key = key;
		}
	}
}
