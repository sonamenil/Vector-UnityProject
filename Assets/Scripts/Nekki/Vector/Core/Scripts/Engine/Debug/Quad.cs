using Nekki.Vector.Core.Location;
using System.Collections.Generic;
using UnityEngine;

namespace Nekki.Vector.Core.Scripts.Engine.Debug
{
	public class Quad : MonoBehaviour
	{
		private Color _Color;

		private Color _BackgoundColor;

		private float _Border;

		private float _HalfBorder;

		protected QuadRunner _Base;

		private static Shader _SpriteShader;

		private static Shader _MeshShader;

		private List<GameObject> _Lines;

		private GameObject _MeshObject;

		private Mesh _Mesh;

		private BoxCollider2D _Collider;

		private Rectangle _OldRect;

		private List<Transform> _points;

		[SerializeField]
		private Vector3 _offset;

		public Color Color
		{
			get
			{
				return default(Color);
			}
			set
			{
			}
		}

		public Color BackgoundColor
		{
			get
			{
				return default(Color);
			}
			set
			{
			}
		}

		public float Border
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public QuadRunner Base
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		private static Shader SpriteShader => null;

		private static Shader MeshShader => null;

		private void CreateMesh()
		{
		}

		private GameObject CreateLine()
		{
			return null;
		}

		private void Start()
		{
		}

		public void UpdateLine(GameObject Line, Vector3 Start, Vector3 End)
		{
		}

		public void UpdateMesh()
		{
		}

		public void Update()
		{
		}

		private void UpdateRect()
		{
		}

		private void OnMouseDown()
		{
		}
	}
}
