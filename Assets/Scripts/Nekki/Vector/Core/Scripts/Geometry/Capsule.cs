using DG.Tweening;
using Nekki.Vector.Core.Node;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Nekki.Vector.Core.Scripts.Geometry
{
    public class Capsule : MonoBehaviour
    {
        private double _Stroke = 1;

        protected ModelLine _Base;

        private static Material _SharedMaterial;

        private LineRenderer _LineRenderer;

        private static Material SharedMaterial
        {
            get
            {
                if (_SharedMaterial == null)
                {
                    _SharedMaterial = new Material(Shader.Find("Sprites/Colored"));
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
            _LineRenderer.endWidth = (float)(_Stroke * 2);
            _LineRenderer.startWidth = (float)(_Stroke * 2);
            _LineRenderer.useWorldSpace = false;
            _LineRenderer.sharedMaterial = SharedMaterial;
            _LineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            _LineRenderer.receiveShadows = false;
            _LineRenderer.alignment = LineAlignment.TransformZ;
            _LineRenderer.allowOcclusionWhenDynamic = false;
        }

        private void UpdateLineBounds(Vector3 a, Vector3 b, float radius)
        {
            // If LineRenderer.useWorldSpace = false, convert world points to local.
            Vector3 localA = a;
            Vector3 localB = b;

            Bounds bounds = new Bounds(localA, Vector3.zero);
            bounds.Encapsulate(localB);

            // Inflate by line thickness + cap padding.
            float padding = radius * 2f;
            bounds.Expand(new Vector3(padding, padding, padding));

            _LineRenderer.localBounds = bounds;
        }

        public void Update()
        {
            if (_Stroke != _Base.Stroke)
            {
                _Stroke = _Base.Stroke;
                _LineRenderer.endWidth = (float)(_Stroke * 2);
                _LineRenderer.startWidth = (float)(_Stroke * 2);
            }

            if (_Base != null && _Base.Start != null && _Base.Start.End != null && _Base.End != null && _Base.End.End != null && _LineRenderer != null)
            {
                Vector3d start = _Base.Start.Start;
                Vector3d start2 = _Base.End.Start;
                double num = start.X - start2.X;
                double num2 = start.Y - start2.Y;
                double num3 = start.X - num * _Base.Margin1;
                double num4 = start.Y - num2 * _Base.Margin1;
                double num5 = start2.X + num * _Base.Margin2;
                double num6 = start2.Y + num2 * _Base.Margin2;

                var pos1 = new Vector3((float)num3, (float)num4, 0);
                var pos2 = new Vector3((float)num5, (float)num6, 0);

                _LineRenderer.SetPosition(0, pos1);
                _LineRenderer.SetPosition(1, pos2);

                UpdateLineBounds(pos1, pos2, (float)_Stroke);
            }
        }
    }
}
