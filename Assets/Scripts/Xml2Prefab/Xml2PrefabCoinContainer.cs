using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabCoinContainer : Xml2PrefabItemContainer
	{
		[SerializeField]
		private string _prefabName;

		[SerializeField]
		private int _groupId;

		[SerializeField]
		private int _score;

		public string PrefabName => _prefabName;

		public int GroupId => _groupId;

		public int Score => _score;

		public void Init(int type, float x, float y, string prefabName, int groupId, int score, string transformData, ChoiceContainer choice)
		{
			Init(type, x, y, transformData, choice);
			_prefabName = prefabName;
			_groupId = groupId;
			_score = score;
		}
	}
}
