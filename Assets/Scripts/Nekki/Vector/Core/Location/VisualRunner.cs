using System.IO;
using System.Xml;
using Nekki.Vector.Core.Scripts;
using Nekki.Vector.Core.Utilites;
using UnityEngine;
using Xml2Prefab;
using Sprite = UnityEngine.Sprite;

namespace Nekki.Vector.Core.Location
{
    public class VisualRunner : MatrixSupport
    {
        private int _Type;

        [SerializeField]
        private float _ImageWidth;

        private float _ImageHeight;

        private float _OriginalWidth;

        private float _OriginalHeight;

        private Pointd Point;

        private int _Depth;

        private XmlNode Node;

        private Color _DefaultColor = Color.white;

        private Color _Color = Color.white;

        //private Vector3f _DefaultScale = new Vector3f();

        private Vector3f _CurrentScale = new Vector3f();

        private SpriteRenderer _SpriteRender;

        public const int None = 0;

        public const int Static = 1;

        public const int Vanishing = 2;

        public const int Dynamic = 3;

        private static string[] animations = {
            "bird_v0",
            "bird_v2",
            "bird_v3",
            "bonus_v4_off",
            "bonus_v4",
            "credits_off",
            "credits",
            "glass_1",
            "paper_v1",
            "reverse_indicator_left",
            "reverse_indicator_right",
            "run_indicator",
            "stopsign"
        };

        public override GameObject Layer
        {
            get => _Layer;
            set
            {
                _Layer = value;
                if (_Layer == null)
                {
                    return;
                }
                _CachedTransform.SetParent(_Layer.transform, false);
            }
        }

        public Color DefaultColor => _DefaultColor;

        public Color Color
        {
            get => _Color;
            set => _Color = value;
        }

        public Rectangle Rectangle => new Rectangle(Position.X, Position.Y, _ImageWidth, _ImageHeight);

        public bool IsStatic => _Type < 2;

        public bool IsDynamic => _Type == 3;

        public bool IsVanishing => _Type == 2;

        public VisualRunner(int type, string name, Pointd position, float width, float height, Color color, int depth, XmlNode node)
            : base((float)position.X, (float)position.Y, node)
        {
            _Type = type;
            _Name = name;
            Node = node;
            Point = position;
            _ImageHeight = height;
            _ImageWidth = width;
            _OriginalHeight = height;
            _OriginalWidth = width;
            _Color = color;
            _DefaultColor = color;
            _TypeClass = RunnerType.Visual;
            _Depth = depth;
        }

        public override void Generate(GameObject existRunner)
        {
            base.Generate(existRunner);
            _SpriteRender = UnityObject.GetComponent<SpriteRenderer>();
        }

        public override void Generate()
        {
            base.Generate();
            GenerateContent();
        }

        protected override void SerializeData()
        {
            var node = "";
            if (Node != null)
            {
                node = Node.OuterXml;
            }
            _UnityObject.AddComponent<Xml2PrefabVisualRunnerContainer>().Init(_Type, _Name, _ImageWidth, _ImageHeight, _DefaultColor, _Depth, node, (float)Point.X, (float)Point.Y, TransformationDataRaw, Choice);
        }

        public void GenerateContent()
        {
            if (string.IsNullOrEmpty(_Name))
            {
                DebugUtils.Dialog("Image with no name", false);
            }
            if (IsAnimation(_Name))
            {
                MakeAnimationAndRun();
                return;
            }
            _SpriteRender = UnityObject.AddComponent<SpriteRenderer>();
            _SpriteRender.flipY = true;
            Sprite sprite = null;
            string path = Application.streamingAssetsPath + "/textures/" + _Name + ".png";

            if (File.Exists(path))
            {
                sprite = ResourceManager.LoadSpriteFromExternal(path, new Vector2(0, 1), 1);
            }
            else
            {
                sprite = Resources.Load<Sprite>("LevelContent/Textures/" + _Name);
            }
            if (sprite == null)
            {
                DebugUtils.Dialog("Image not found: " + _Name, false);
                return;
            }

            _SpriteRender.sprite = sprite;
            float width = sprite.rect.width;
            float height = sprite.rect.height;
            if (_Support != null)
            {
                _Transformation[0, 0] = _Transformation[0, 0] / width;
                _Transformation[0, 1] = _Transformation[0, 1] / width;
                _Transformation[1, 0] = _Transformation[1, 0] / height;
                _Transformation[1, 1] = _Transformation[1, 1] / height;
                return;
            }
            if (Matrix.IsIdentity(_Transformation))
            {
                _CachedTransform.localScale = new Vector3(_OriginalWidth / width, _OriginalHeight / height);
                return;
            }
            _CachedTransform.localScale = new Vector3(_CachedTransform.localScale.x / width, _CachedTransform.localScale.y / height, 1f);
        }

        protected void MakeAnimationAndRun()
        {
            _SpriteRender = UnityObject.AddComponent<SpriteRenderer>();
            _SpriteRender.flipY = true;
            UnityObject.AddComponent<AnimationSprite>().Init("LevelContent/Animations/" + _Name, _SpriteRender);
            float width = _SpriteRender.sprite.rect.width;
            float height = _SpriteRender.sprite.rect.height;
            if (_Support != null)
            {
                _Transformation[0, 0] = _Transformation[0, 0] / width;
                _Transformation[0, 1] = _Transformation[0, 1] / width;
                _Transformation[1, 0] = _Transformation[1, 0] / height;
                _Transformation[1, 1] = _Transformation[1, 1] / height;
            }
            else
            {
                if (Matrix.IsIdentity(_Transformation))
                {
                    _CachedTransform.localScale = new Vector3(_OriginalWidth / width, _OriginalHeight / height);
                    return;
                }
                _CachedTransform.localScale = new Vector3(_CachedTransform.localScale.x / width, _CachedTransform.localScale.y / height, 1f);
            }
        }

        public override bool Render()
        {
            return true;
        }

        public override void InitRunner(Point point, bool serialize = false)
        {
            base.InitRunner(point, serialize);
            UpdateUnityObjectPosition(Position);
            Transform();
            UpdateColor();
            _DefaultScale.Set(_CachedTransform.localScale);
            _DefaulRotation.Set(_CachedTransform.localEulerAngles);
        }

        public virtual void Init()
        {
            if (IsAnimation(_Name))
            {
                var animationSprite = UnityObject.GetComponent<AnimationSprite>();
                animationSprite.Init("LevelContent/Animations/" + _Name, _SpriteRender);
                animationSprite.FPS = 20;
                animationSprite.IsWork = true;
            }
        }

        public override void Move(Point point)
        {
            if (point == null)
            {
                return;
            }
            var vector3 = new Vector3(Position.X + point.X, Position.Y + point.Y, Position.Z);
            base.UpdateUnityObjectPosition(vector3);
        }

        protected override void UpdateUnityObjectPosition(Vector3 position)
        {
            var z = position.z;
            if (_Depth == 1)
            {
                z += 3;
            }
            if (_Depth == 0)
            {
                z -= 3;
            }
            _CachedTransform.localPosition = new Vector3(position.x, position.y, z);
        }

        private void UpdatePositionSprite()
        {
        }

        public override void Reset()
        {
            base.Reset();
            _Color = _DefaultColor;
            UpdateColor();

            _ImageWidth = _OriginalWidth;
            _ImageHeight = _OriginalHeight;
        }

        private void ResetObjects()
        {
        }

        public override void TransformColor(Color p_delta)
        {

            _Color += p_delta;
            UpdateColor();
        }

        public override void TransformColorEnd(Color p_color)
        {
            _Color = p_color;
            UpdateColor();
        }

        public override void TransformResize(Point size)
        {
            var scale = _CachedTransform.localScale;
            scale.x = size.X / _SpriteRender.sprite.texture.width;
            scale.y = size.Y / _SpriteRender.sprite.texture.height;
            _CachedTransform.localScale = scale;


            _ImageWidth = scale.x;
            _ImageHeight = scale.y;
        }

        public void UpdateColor()
        {
            if (_Color.r < 0f)
            {
                return;
            }
            if (_SpriteRender != null)
            {
                _SpriteRender.color = _Color;
            }
        }

        private bool IsAnimation(string name)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (name == animations[i])
                {
                    return true;
                }
            }
            return false;
        }

    }
}
