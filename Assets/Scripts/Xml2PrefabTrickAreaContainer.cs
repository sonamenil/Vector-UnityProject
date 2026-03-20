using UnityEngine;
using Xml2Prefab;

public class Xml2PrefabTrickAreaContainer : Xml2PrefabAreaContainer
{
	[SerializeField]
	private string _itemName;

	[SerializeField]
	private int _score;

	public string ItemName => _itemName;

	public int Score => _score;

	public void Init(string transformations, string type, string n, float x, float y, float w, float h, string itemName, int score, ChoiceContainer choice)
	{
		Init(transformations, type, n, x, y, w, h, choice);
		_itemName = itemName;
		_score = score;
	}
}
