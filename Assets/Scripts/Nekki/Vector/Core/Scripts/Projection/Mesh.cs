using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Nekki.Vector.Core.Scripts.Projection
{
	public class Mesh : MonoBehaviour
	{
		private Color _Color = Color.black;

		protected List<Triangle> _Base;

		private UnityEngine.Mesh _Mesh;

		private static Shader _Shader;

		private Material _Material;

		private Vector3[] _vertices;

		public Color Color
		{
			get
			{
				return _Color;
			}
			set
			{
				_Color = value;
			}
		}

		public List<Triangle> Base
		{
			get
			{
				return _Base;
			}
			set
			{
				_Base = value;
			}
		}

		private static Shader Shader
		{
			get
			{
				if (_Shader == null)
				{
					_Shader = Shader.Find("Mesh/Colored");
				}
				return _Shader;
			}
		}

		private void Start()
		{
			_Mesh = new UnityEngine.Mesh();
			var renderer = gameObject.AddComponent<MeshRenderer>();
			var filter = gameObject.AddComponent<MeshFilter>();
			filter.mesh = _Mesh;
			_Material = renderer.material;
			_Material.shader = Shader;
			_Material.SetVector("_Color", _Color);
			InitMesh();
		}

		private void InitMesh()
		{
			_vertices = new Vector3[_Base.Count * 3];
			int[] triangles = new int[_Base.Count * 3];
			for (int t = 0; t < _Base.Count; t++)
			{
				var i = t * 3;
				var tri = _Base[t];
				_vertices[i + 0] = new Vector3((float)tri.Node0.Start.X, (float)tri.Node0.Start.Y, 0);
				_vertices[i + 1] = new Vector3((float)tri.Node1.Start.X, (float)tri.Node1.Start.Y, 0);
                _vertices[i + 2] = new Vector3((float)tri.Node2.Start.X, (float)tri.Node2.Start.Y, 0);

                triangles[i + 0] = i + 0;
                triangles[i + 1] = i + 1;
                triangles[i + 2] = i + 2;
            }
			_Mesh.vertices = _vertices;
			_Mesh.triangles = triangles;
			_Mesh.RecalculateBounds();
		}

		private void Update()
		{
            for (int t = 0; t < _Base.Count; t++)
            {
                var i = t * 3;
                var tri = _Base[t];
                _vertices[i + 0] = new Vector3((float)tri.Node0.Start.X, (float)tri.Node0.Start.Y, 0);
                _vertices[i + 1] = new Vector3((float)tri.Node1.Start.X, (float)tri.Node1.Start.Y, 0);
                _vertices[i + 2] = new Vector3((float)tri.Node2.Start.X, (float)tri.Node2.Start.Y, 0);
            }
            _Mesh.vertices = _vertices;
			_Mesh.RecalculateBounds();
        }
	}
}
