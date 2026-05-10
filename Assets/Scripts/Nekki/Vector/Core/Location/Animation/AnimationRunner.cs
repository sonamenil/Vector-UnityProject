using System.Collections.Generic;
using Xml2Prefab;

namespace Nekki.Vector.Core.Location.Animation
{
    public class AnimationRunner : AnimationRendering
    {
        public const int NONE = 0;

        public const int VECTOR = 1;

        protected int _Type;

        protected float _ScaleX;

        protected float _ScaleY;

        protected float _X;

        protected float _Y;

        private Dictionary<string, string> _oldAnimationNames = new Dictionary<string, string>
        {
            {"p_glass1_mini", "glass_1" },
            {"p_birds1", "bird_v0" },
            { "p_birds2", "bird_v2"},
            { "p_birds3", "bird_v3" },
            {"p_paper1", "paper_v1" }
        };

        public float ScaleX => _ScaleX;

        public float ScaleY => _ScaleY;


        public AnimationRunner(float x, float y, float width, float height, string name, int type, float scaleX, float scaleY, bool replay = false, int speed = 1)
            : base(name, x, y, width, height, replay, 2)
        {
            _X = x;
            _Y = y;
            _Type = type;
            _TypeClass = RunnerType.Animation;
            _ScaleX = scaleX;
            _ScaleY = scaleY;
        }

        public override void Init(float pivotX = 0, float pivotY = 1)
        {
            if (_oldAnimationNames.ContainsKey(_Name))
            {
                Name = _oldAnimationNames[_Name];
            }
            base.Init();
            var scale = _CachedTransform.localScale;
            scale.x *= _ScaleX;
            scale.y *= _ScaleY;
            _CachedTransform.localScale = scale;

            _DefaultScale = _CachedTransform.localScale;
        }

        protected override void SerializeData()
        {
            _UnityObject.AddComponent<Xml2PrefabAnimationContainer>().Init(_Name, _X, _Y, _Width, _Height, _Type, _ScaleX, _ScaleY, TransformationDataRaw, Choice);
        }

        public override void Reset()
        {
            Stop();
            base.Reset();
        }

        public virtual void PlayAnimation()
        {
            _IsPlay = true;
            RunnerRender.AddRunner(this);
            PlayFrom(0);
            IsEnabled = true;
        }

        public override void Stop(bool Value = false)
        {
            _IsPlay = false;
            IsEnabled = false;
        }

        public override void Move(Point Point)
        {
            base.Move(Point);
            UpdatePositionSprite();
            IsEnabled = false;
        }

        private void UpdatePositionSprite()
        {
            _UnityObject.transform.localPosition = Position;
        }
    }
}
