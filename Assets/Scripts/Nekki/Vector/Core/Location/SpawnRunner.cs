using Nekki.Vector.Core.Animation;
using Xml2Prefab;

namespace Nekki.Vector.Core.Location
{
	public class SpawnRunner : Runner
	{
		protected string _animations;

		public AnimationReaction Reaction;

		public SpawnRunner()
			: base(0f, 0f)
		{
			_TypeClass = RunnerType.Spawn;
		}

		public SpawnRunner(float x, float y, string n, string animation)
			: base(x, y)
		{
			_animations = animation;
			_TypeClass = RunnerType.Spawn;
			_Name = n;
			_Hash = n.GetHashCode();
			if (string.IsNullOrEmpty(_animations))
			{
				return;	
			}
			Reaction = AnimationLoader.ParseReaction(_animations.Split('|'));
		}

		protected override void SerializeData()
		{
			_UnityObject.AddComponent<Xml2PrefabSpawnContainer>().Init(_DefautPosition.X, _DefautPosition.Y, _Name, _animations, TransformationDataRaw, Choice);
		}

		public override bool Render()
		{
			return true;
		}
	}
}
