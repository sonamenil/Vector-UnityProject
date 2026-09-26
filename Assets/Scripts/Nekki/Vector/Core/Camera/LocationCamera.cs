using System;
using System.Collections.Generic;
using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Node;
using UnityEngine;

namespace Nekki.Vector.Core.Camera
{
    public class LocationCamera
    {
        private const float LegacyOrthographicSize = 214f;

        private List<ZoomContainer> _Containers = new List<ZoomContainer>();

        private float _Zoom = 1;

        private ModelNode _Position = new ModelNode(new Vector3d());

        private bool _IsDrag;

        private CameraNode _Node;

        private float _TempZoom;

        private bool _IsRender;

        private bool _IgnoreZoom;

        private int _EffectFrames;

        private int _ActionLaunched;

        private List<object> _Layers = new List<object>();

        private static LocationCamera _Current;

        private System.Random _Random = new System.Random();

        private Vector3 _UnityCameraStartPosition;

        private float _PerspectiveReferenceDistance;

        private float _PerspectiveCameraZ;

        private bool _IsPerspectiveCameraConfigured;

        public static float MinZoom = 0.1f;

        public static float MaxZoom = 1.3f;

        public static float CurrentZoom = 0.5f;

        public static float FluencyCurrent = 2;

        public float Zoom => _Zoom;

        public CameraNode Node
        {
            get => _Node;
            set => _Node = value;
        }

        public bool IsRender => _IsRender;

        public bool IgnoreZoom
        {
            get => _IgnoreZoom;
            set => _IgnoreZoom = value;
        }

        public static LocationCamera Current => _Current;

        public UnityEngine.Camera UnityCamera => UnityEngine.Camera.main;

        public float X => (float)_Position.Start.X;

        public float Y => (float)_Position.Start.Y;

        public void Init()
        {
            _Position.Attenuation = 0;
            _Current = this;
            _IsRender = true;
            ConfigurePerspectiveCamera();
        }

        public void Reset()
        {
            FixedRenderInterpolation.ResetHistory();
            _Position.PositionStart(new Vector3d(0, 0, 0));
            _Position.PositionEnd(new Vector3d(0, 0, 0));
            _IsRender = true;
            _Node = null;
            Update();
        }

        public void StartPosition(Vector3d p_position)
        {
            _Position = new ModelNode(p_position);
            _Node = new CameraNode(_Position);
        }

        public void UpdatePosition()
        {
            if (!_IsRender)
            {
                return;
            }
            _Position.TimeStep(0);
            var nodeStart = _Node.Start;
            var nodeEnd = _Node.End;
            var posStart = _Position.Start;
            var posEnd = _Position.End;

            nodeStart.Z = 0;
            nodeEnd.Z = 0;
            posStart.Z = 0;
            posEnd.Z = 0;

            Vector3d v1 = posEnd + nodeStart;
            nodeEnd = v1 - nodeEnd;
            nodeEnd = nodeEnd - posStart;

            nodeStart = nodeStart - posStart;
            nodeStart = nodeStart * 0.15;
            nodeEnd = nodeEnd + nodeStart;

            if (FluencyCurrent < nodeEnd.Length)
            {
                nodeEnd.Normalize();
                nodeEnd.Multiply(FluencyCurrent);
            }
            posStart.Add(nodeEnd);
            nodeEnd = posStart - posEnd;

            if (nodeEnd.Length > 50)
            {
                nodeEnd.Multiply(50 / nodeEnd.Length);
                posStart = posEnd + nodeEnd;
            }
            _Position.PositionStart(posStart);
            Update();

        }

        public void Update()
        {
            UpdateUnityCameraPosition();

            // Layers no longer move in X/Y. Their existing zoom animation is
            // represented by perspective depth instead.
            foreach (var container in _Containers)
            {
                container.Play();
            }
        }

        public void Stop()
        {
            Vector3d p_position = _Node.Start + (_Node.Start - _Node.End) * 20f;
            _Node = new CameraNode(new ModelNode(p_position));
        }

        public void Layers(BaseSets p_sets)
        {
            foreach (var container in p_sets.Containers.Values)
            {
                var zoomContainer = new ZoomContainer();
                zoomContainer.Add(container.Object, container.Factor);

                if (_IsPerspectiveCameraConfigured)
                {
                    zoomContainer.ConfigurePerspective(_PerspectiveReferenceDistance, _PerspectiveCameraZ);
                }

                _Containers.Add(zoomContainer);
                _Layers.Add(container);
            }

            UpdateCameraFarClipPlane();
        }

        public void Zooming(float p_value = 1f, bool p_isStart = false)
        {
            if (_Zoom != p_value && !_IgnoreZoom)
            {
                if (p_value < MinZoom)
                {
                    p_value = MinZoom;
                }
                if (p_value > MaxZoom)
                {
                    p_value = MaxZoom;
                }
                _Zoom = p_value;
                foreach (var container in _Containers)
                {
                    container.Zooming(p_value, p_isStart);
                }

                UpdateCameraFarClipPlane();
            }
        }

        public void ZoomIncrease(float p_value)
        {
            Zooming(_Zoom + p_value);
        }

        public void ZoomReduce(float p_value)
        {
            Zooming(_Zoom - p_value);
        }

        private void ConfigurePerspectiveCamera()
        {
            var camera = UnityCamera;
            if (camera == null)
            {
                return;
            }

            _UnityCameraStartPosition = camera.transform.position;
            FixedRenderInterpolation.Register(camera.transform);

            // Match the vertical framing of the old orthographic camera:
            // orthographicSize = distance * tan(verticalFov / 2).
            float halfFovRadians = Mathf.Clamp(camera.fieldOfView, 1f, 179f) * 0.5f * Mathf.Deg2Rad;
            _PerspectiveReferenceDistance = LegacyOrthographicSize / Mathf.Tan(halfFovRadians);

            // The legacy 2D content is assumed to live around world Z = 0.
            _PerspectiveCameraZ = -_PerspectiveReferenceDistance;

            var cameraPosition = _UnityCameraStartPosition;
            cameraPosition.z = _PerspectiveCameraZ;
            camera.transform.position = cameraPosition;
            camera.orthographic = false;

            _IsPerspectiveCameraConfigured = true;

            foreach (var container in _Containers)
            {
                container.ConfigurePerspective(_PerspectiveReferenceDistance, _PerspectiveCameraZ);
            }

            UpdateCameraFarClipPlane();
            UpdateUnityCameraPosition();
        }

        private void UpdateUnityCameraPosition()
        {
            if (!_IsPerspectiveCameraConfigured)
            {
                ConfigurePerspectiveCamera();
            }

            var camera = UnityCamera;
            if (camera == null || !_IsPerspectiveCameraConfigured)
            {
                return;
            }

            var position = camera.transform.position;
            position.x = _UnityCameraStartPosition.x + (float)_Position.Start.X;
            position.y = _UnityCameraStartPosition.y + (float)_Position.Start.Y;
            position.z = _PerspectiveCameraZ;
            camera.transform.position = position;
        }

        private void UpdateCameraFarClipPlane()
        {
            var camera = UnityCamera;
            if (camera == null || !_IsPerspectiveCameraConfigured)
            {
                return;
            }

            float requiredDistance = _PerspectiveReferenceDistance;
            foreach (var container in _Containers)
            {
                requiredDistance = Mathf.Max(
                    requiredDistance,
                    container.MaxPerspectiveDistance(MinZoom, MaxZoom));
            }

            // Leave a little margin so the farthest plane is not clipped while
            // its zoom interpolation is settling.
            camera.farClipPlane = Mathf.Max(camera.farClipPlane, requiredDistance * 1.1f);
        }
    }
}
