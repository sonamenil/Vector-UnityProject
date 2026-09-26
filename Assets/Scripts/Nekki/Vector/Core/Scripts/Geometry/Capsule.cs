using DG.Tweening;
using Nekki.Vector.Core.Node;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Nekki.Vector.Core.Scripts.Geometry
{
    public class Capsule : MonoBehaviour
    {
        private Color _Color = Color.black;

        private double _Stroke = 1;

        protected ModelLine _Base;

        private static Material _SharedMaterial;

        private LineRenderer _LineRenderer;

        public Color Color
        {
            get => _Color;
            set => _Color = value;
        }

        private static Material SharedMaterial
        {
            get
            {
                if (_SharedMaterial == null)
                {
                    _SharedMaterial = new Material(Shader.Find("Sprites/Default"));
                }
                return _SharedMaterial;
            }
        }

        public Vector2 middleRect => transform.TransformPoint(Vector3f.Middle(_LineRenderer.GetPosition(0), _LineRenderer.GetPosition(1)));

        public string Name => _Base.Name;

        public void Init(ModelLine modelLine)
        {
            _Base = modelLine;
            _Stroke = modelLine.Stroke;

            _Stroke = _Base.Stroke;
            _LineRenderer = gameObject.AddComponent<LineRenderer>();
            _LineRenderer.numCapVertices = 9;
            _LineRenderer.widthMultiplier = (float)(_Stroke * 2);
            _LineRenderer.useWorldSpace = false;
            _LineRenderer.sharedMaterial = SharedMaterial;
            _LineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            _LineRenderer.receiveShadows = false;
            _LineRenderer.alignment = LineAlignment.TransformZ;
            _LineRenderer.allowOcclusionWhenDynamic = false;
            _LineRenderer.sortingLayerName = "Model";
            _LineRenderer.startColor = _Color;
            _LineRenderer.endColor = _Color;
        }

        // Gameplay advances in Update; sample its completed presentation state.
        public void LateUpdate()
        {
            if (_Stroke != _Base.Stroke)
            {
                _Stroke = _Base.Stroke;
                _LineRenderer.widthMultiplier = (float)(_Stroke * 2);

            }

            if (_Base != null && _Base.Start != null && _Base.Start.End != null && _Base.End != null && _Base.End.End != null && _LineRenderer != null)
            {
                Vector3 start = FixedRenderInterpolation.Position(_Base.Start);
                Vector3 start2 = FixedRenderInterpolation.Position(_Base.End);
                double dx = start.x - start2.x;
                double dy = start.y - start2.y;
                //double dz = start.Z - start2.Z;
                double x1 = start.x - dx * _Base.Margin1;
                double y1 = start.y - dy * _Base.Margin1;
                //double z1 = start.Z - dz * _Base.Margin1;
                double x2 = start2.x + dx * _Base.Margin2;
                double y2 = start2.y + dy * _Base.Margin2;
                //double z2 = start2.Z + dz * _Base.Margin2;

                var pos1 = new Vector3((float)x1, (float)y1, 0);
                var pos2 = new Vector3((float)x2, (float)y2, 0);

                _LineRenderer.SetPosition(0, pos1);
                _LineRenderer.SetPosition(1, pos2);
            }
        }
    }
}
