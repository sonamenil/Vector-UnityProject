using UnityEngine;

namespace Xml2Prefab
{
	public class Xml2PrefabAnimationVectorContainer : Xml2PrefabAnimationContainer
	{
		[SerializeField]
		private float _directionX;

		[SerializeField]
		private float _directionY;

		[SerializeField]
		private float _accelerationX;

		[SerializeField]
		private float _accelerationY;

		[SerializeField]
		private int _time;

		public float DirectionX => _directionX;

		public float DirectionY => _directionY;

		public float AccelerationX => _accelerationX;

		public float AccelerationY => _accelerationY;

		public int Time => _time;

		public void Init(string n, float x, float y, float w, float h, int type, float scaleX, float scaleY, string transformationData, float directionX, float directionY, float accelerationX, float accelerationY, int time, ChoiceContainer choice)
		{
			Init(n, x, y, w, h, type, scaleX, scaleY, transformationData, choice);
            _directionX = directionX;
            _directionY = directionY;
            _accelerationX = accelerationX;
            _accelerationY = accelerationY;
            _time = time;
        }
	}
}
