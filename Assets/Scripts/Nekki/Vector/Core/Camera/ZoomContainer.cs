using Nekki.Vector.Core.Utilites;
using UnityEngine;

namespace Nekki.Vector.Core.Camera
{
    public class ZoomContainer
    {
        private const float MinPerspectiveFactor = 0.0001f;
        private const float MinPerspectiveScale = 0.0001f;

        private float _Factor;

        private float _ZoomValue;

        private float _Scale;

        private bool _IsZoom;

        private int _Frame;

        private int _CameraFrame;

        private float _Time;

        private GameObject _Layer;

        private Vector3 _BaseLocalScale;

        private float _PerspectiveReferenceDistance;

        private float _PerspectiveCameraZ;

        private bool _IsPerspectiveConfigured;

        public float FrameScale
        {
            get
            {
                float num = 2 * _Frame / _Time;
                num *= num;
                float num2 = (_ZoomValue - _Scale) / 2f;
                if (_Frame < _Time / 2f)
                {
                    return _Scale + num2 * num;
                }
                return _Scale + num2 * (8 * _Frame / _Time - 2f - num);
            }
        }

        public void Add(GameObject p_object, float p_factor)
        {
            _Factor = p_factor;
            _Layer = p_object;
            FixedRenderInterpolation.Register(_Layer.transform);
            _BaseLocalScale = _Layer.transform.localScale;
            _ZoomValue = 1;
            _Scale = 4;
            _Time = 30;
        }

        public void ConfigurePerspective(float p_referenceDistance, float p_cameraZ)
        {
            _PerspectiveReferenceDistance = p_referenceDistance;
            _PerspectiveCameraZ = p_cameraZ;
            _IsPerspectiveConfigured = true;


            float factor = SafeFactor;
            _Layer.transform.localScale = _BaseLocalScale / factor;

            ApplyPerspectiveDepth(_ZoomValue);
        }

        public void Zooming(float p_value, bool p_isStart)
        {
            _Frame = 0;
            _Scale = _ZoomValue;
            _ZoomValue = Math.Round(p_value / (p_value + _Factor * (1 - p_value)), 10);
            if (!p_isStart)
            {
                _IsZoom = true;
                return;
            }

            _Scale = _ZoomValue;
            ApplyVisualScale(_ZoomValue);
        }

        public void Play()
        {
            if (!_IsZoom)
            {
                return;
            }

            _Frame++;
            ApplyVisualScale(FrameScale);
            if (_Frame < _Time)
            {
                return;
            }

            _IsZoom = false;
        }

        public float MaxPerspectiveDistance(float p_minZoom, float p_maxZoom)
        {
            if (!_IsPerspectiveConfigured)
            {
                return 0;
            }

            float minScale = ZoomScale(p_minZoom);
            float maxScale = ZoomScale(p_maxZoom);
            float minDistance = PerspectiveDistance(minScale);
            float maxDistance = PerspectiveDistance(maxScale);
            return Mathf.Max(minDistance, maxDistance, _PerspectiveReferenceDistance);
        }

        private float SafeFactor => Mathf.Max(Mathf.Abs(_Factor), MinPerspectiveFactor);

        private float ZoomScale(float p_zoom)
        {
            float denominator = p_zoom + _Factor * (1 - p_zoom);
            if (Mathf.Abs(denominator) < MinPerspectiveScale)
            {
                denominator = denominator < 0 ? -MinPerspectiveScale : MinPerspectiveScale;
            }
            return p_zoom / denominator;
        }

        private float PerspectiveDistance(float p_visualScale)
        {
            float scale = Mathf.Max(Mathf.Abs(p_visualScale), MinPerspectiveScale);
            return _PerspectiveReferenceDistance / (SafeFactor * scale);
        }

        private void ApplyVisualScale(float p_visualScale)
        {
            if (_IsPerspectiveConfigured)
            {
                ApplyPerspectiveDepth(p_visualScale);
                return;
            }

            _Layer.transform.localScale = _BaseLocalScale * p_visualScale;
        }

        private void ApplyPerspectiveDepth(float p_visualScale)
        {
            float distance = PerspectiveDistance(p_visualScale);
            var position = _Layer.transform.position;
            position.z = _PerspectiveCameraZ + distance;
            _Layer.transform.position = position;

            float scaleSign = p_visualScale < 0 ? -1f : 1f;
            _Layer.transform.localScale = (_BaseLocalScale / SafeFactor) * scaleSign;
        }
    }
}
