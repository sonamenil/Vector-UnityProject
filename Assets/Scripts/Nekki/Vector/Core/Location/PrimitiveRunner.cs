using System.Collections.Generic;
using Nekki.Vector.Core.Models;
using UnityEngine;
using Xml2Prefab;

namespace Nekki.Vector.Core.Location
{
    public class PrimitiveRunner : Runner
    {
        private Vector3f _position = new Vector3f();

        private Vector3f _DeltaPosition = new Vector3f();

        private Color _Color;

        private ModelType _Type = ModelType.Primitive;

        private bool _IsUpdate;

        private bool _IsLoaded;

        private List<string> _Sounds;

        private ModelPrimitive _Model;

        private float _Impulse;

        protected int _type;

        public override Vector3f Position
        {
            get => _position;
            set => _position = DeltaPosition + value;
        }

        public Vector3f DeltaPosition => _DeltaPosition == null ? new Vector3f() : _DeltaPosition;

        public Color Color
        {
            get => _Color;
            set => _Color = value;
        }

        public ModelType Type
        {
            get => _Type;
            set => _Type = value;
        }

        public bool IsUpdate => _IsUpdate;

        public ModelPrimitive Model => _Model;

        public virtual bool IsStrike
        {
            get => _Model.IsStrike;
            set => _Model.IsStrike = value;
        }

        public override bool IsDebug
        {
            get => _IsDebug;
            set
            {
                _IsDebug = value;
                _Model.DebugMode = value;
            }
        }

        public Rectangle Rectangle => _Model.Rectangle;

        public PrimitiveRunner(int type, string name, Color color, Vector3f deltaPosition, float impulse, List<string> sounds)
            : base(0f, 0f)
        {
            _Color = color;
            _DeltaPosition = deltaPosition;
            _Name = name;
            _Sounds = sounds;
            _Impulse = impulse;
            _TypeClass = RunnerType.Primitive;
            _type = type;
        }

        protected override void SerializeData()
        {
            UnityObject.AddComponent<Xml2PrefabPrimitiveContainer>().Init(_Name, _type, _DeltaPosition.X, _DeltaPosition.Y, _Impulse, _Sounds, TransformationDataRaw, Choice);
        }

        public override void InitRunner(Point point, bool serialize = false)
        {
            base.InitRunner(point, serialize);
            _DefautPosition.Set(_position);
        }

        public virtual void Load()
        {
            List<string> skins = new List<string> { _Name + ".xml" };
            _Model = new ModelPrimitive(this, skins, _Sounds);
            _Model.Type = _Type;
            _Model.Color = _Color;
            _Model.Name = _Name;
            _Model.IsEnabled = true;
            _Model.Layer = UnityObject;
            _Model.Impulse = _Impulse;
            _IsLoaded = true;
            _Model.Position(new Vector3d(Position.X, Position.Y, 0));

        }

        public override bool Render()
        {
            return true;
        }

        public virtual void Render(List<QuadRunner> Platforms)
        {
            if (!IsEnabled || !_Model.IsStrike)
            {
                return;
            }
            CollisitionPlatform(Platforms);
            Update();
            _Model.ModelObject.RenderMacroNode();
            _Model.ControllerPhysics.Render();
            if (_Model.IsStrike)
            {
                if (_Model.RenderTime > 0)
                {
                    _Model.RenderTime--;
                    return;
                }
                _Model.IsVisible = false;
                _Model.ControllerPhysics.Stop();
            }
        }

        public override void Move(Point point)
        {
            if (point == null)
            {
                return;
            }
            Position = new Vector3f(point);
            if (_Model != null)
                _Model.Position(new Vector3d(Position.X, Position.Y, 0));
        }

        public void CollisitionPlatform(List<QuadRunner> quads)
        {
            foreach (var quad in quads)
            {
                _Model.ControllerCollisions.UpdatePlatform(quad);
            }
        }

        public void Update()
        {
            if (_Model.IsEnabled && _IsUpdate)
            {
                Position += _DeltaPosition;
            }
        }

        public override void Reset()
        {
            _IsUpdate = false;
            _Model.IsStrike = false;
            _Model.IsEnabled = true;
            _Model.ControllerPhysics.Stop();
            _Model.Reset();
            Move(new Point(Position.X - _DeltaPosition.X, Position.Y - _DeltaPosition.Y));
            _Model.IsVisible = true;
            _Model.ModelObject.RenderMacroNode();
            _Model.ControllerPhysics.Render();
        }
    }
}
