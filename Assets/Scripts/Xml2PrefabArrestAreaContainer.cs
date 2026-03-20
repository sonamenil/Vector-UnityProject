using UnityEngine;
using Xml2Prefab;

public class Xml2PrefabArrestAreaContainer : Xml2PrefabAreaContainer
{
	[SerializeField]
	private float _distance;

	public float Distance => _distance;

	public void Init(string transformations, string type, string n, float x, float y, float w, float h, float distance, ChoiceContainer choice)
	{
		Init(transformations, type, n, x, y, w, h, choice);
		_distance = distance;
	}
}
