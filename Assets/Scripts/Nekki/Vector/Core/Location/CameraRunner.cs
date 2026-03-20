using Xml2Prefab;

namespace Nekki.Vector.Core.Location
{
	public class CameraRunner : SpawnRunner
	{
		public CameraRunner(float x, float y, string n, string animation) : base(x, y, n, animation)
		{
			_TypeClass = RunnerType.Camera;
		}

		protected override void SerializeData()
		{
			_UnityObject.AddComponent<Xml2PrefabCameraContainer>().Init(_DefautPosition.X, _DefautPosition.Y, _Name, _animations, TransformationDataRaw, Choice);
		}
	}
}
