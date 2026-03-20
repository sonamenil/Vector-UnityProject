using System;
using Nekki.Vector.Core.Node;
using UnityEngine;
using UnityEngine.Rendering;

namespace Nekki.Vector.Core.Scripts.Geometry
{
    public class Capsule : MonoBehaviour
    {
        private double _Stroke = 1;

        protected ModelLine _Base;

        private string _SortingLayerName = "";

        private int _SortingOrder;

        private static Material _SharedQuadMaterial;

        private static Material _SharedCircleMaterial;

        private Transform _BeginCircle;

        private Transform _EndCircle;

        private Transform _MiddleRect;

        public string SortingLayerName
        {
            get
            {
                return _SortingLayerName;
            }
            set
            {
                _SortingLayerName = value;
            }
        }

        public int SortingOrder
        {
            get
            {
                return _SortingOrder;
            }
            set
            {
                _SortingOrder = value;
            }
        }

        private static Material SharedQuadMaterial
        {
            get
            {
                if (_SharedQuadMaterial == null)
                {
                    _SharedQuadMaterial = new Material(Shader.Find("Sprites/Colored"));
                }
                return _SharedQuadMaterial;
            }
        }

        private static Material SharedCircleMaterial
        {
            get
            {
                if (_SharedCircleMaterial == null)
                {
                    _SharedCircleMaterial = new Material(Shader.Find("Sprites/Circle-Colored"));
                }
                return _SharedCircleMaterial;
            }
        }

        public Transform middleRect => _MiddleRect;

        public string Name => _Base.Name;

        public void Init(ModelLine modelLine)
        {
            _Base = modelLine;
            _Stroke = modelLine.Stroke;
            var obj = new GameObject("CircleBegin " + modelLine.Start.Name);
            InitGameObject(obj, SharedCircleMaterial);
            _BeginCircle = obj.transform;
            _BeginCircle.SetParent(transform);
            var obj2 = new GameObject("CircleEnd " + modelLine.End.Name);
            InitGameObject(obj2, SharedCircleMaterial);
            _EndCircle = obj2.transform;
            _EndCircle.SetParent(transform);
            var obj3 = new GameObject("Rect");
            InitGameObject(obj3, SharedQuadMaterial);
            _MiddleRect = obj3.transform;
            _MiddleRect.SetParent(transform);
            var scale = new Vector3((float)_Stroke * 2, (float)_Stroke * 2, (float)_Stroke * 2);
            _BeginCircle.localScale = scale;
            _EndCircle.localScale = scale;
            Update();
        }

        private void InitGameObject(GameObject p_object, Material p_material)
        {
            MeshFilter meshFilter = p_object.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = p_object.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[4]
            {
                new Vector3(-0.5f, -0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0f),
                new Vector3(-0.5f, 0.5f, 0f),
                new Vector3(0.5f, 0.5f, 0f)
            };
            mesh.uv = new Vector2[4]
            {
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(0f, 1f),
                new Vector2(1f, 1f)
            };
            mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };
            mesh.RecalculateBounds();
            meshFilter.mesh = mesh;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            meshRenderer.lightProbeUsage = LightProbeUsage.Off;
            meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            meshRenderer.sharedMaterial = p_material;
            meshRenderer.sortingLayerName = _SortingLayerName;
            meshRenderer.sortingOrder = _SortingOrder;
        }

        public void Update()
        {
            if (_Base != null && _Base.Start != null && _Base.Start.End != null && _Base.End != null && _Base.End.End != null && !(_BeginCircle == null) && !(_EndCircle == null) && !(_MiddleRect == null))
            {
                Vector3d start = _Base.Start.Start;
                Vector3d start2 = _Base.End.Start;
                double num = start.X - start2.X;
                double num2 = start.Y - start2.Y;
                double num3 = start.X - num * _Base.Margin1;
                double num4 = start.Y - num2 * _Base.Margin1;
                double num5 = start2.X + num * _Base.Margin2;
                double num6 = start2.Y + num2 * _Base.Margin2;
                _BeginCircle.localPosition = new Vector2((float)num3, (float)num4);
                _EndCircle.localPosition = new Vector2((float)num5, (float)num6);
                num = num3 - num5;
                num2 = num4 - num6;
                double y = Math.Sqrt(num * num + num2 * num2);
                _MiddleRect.transform.up = new Vector3((float)num, (float)num2, 0f);
                num = (num3 + num5) / 2f;
                num2 = (num4 + num6) / 2f;
                _MiddleRect.localPosition = new Vector3((float)num, (float)num2);
                if (_Stroke != _Base.Stroke)
                {
                    _Stroke = _Base.Stroke;
                    double num7 = _Stroke * 2f;
                    _BeginCircle.localScale = new Vector3((float)num7, (float)num7, 1f);
                    _EndCircle.localScale = new Vector3((float)num7, (float)num7, 1f);
                }
                _MiddleRect.localScale = new Vector3((float)_Stroke * 2f, (float)y, 1f);
            }
        }
    }
}
