using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabItemScoreContainer : Xml2PrefabItemContainer
	{
		[SerializeField]
		private string _prefabName;

		[SerializeField]
		private int _score;

		public string PrefabName => _prefabName;

		public int Score => _score;

		public void Init(int type, float x, float y, string prefabName, int score, string transformData, ChoiceContainer choice)
		{
			Init(type, x, y, transformData, choice);
			_score = score;
			_prefabName = prefabName;
		}
    }
}
