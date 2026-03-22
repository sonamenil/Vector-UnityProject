using System.Collections.Generic;
using Nekki.Vector.Core.Node;
using Nekki.Vector.Core.Scripts.Geometry;
using UnityEngine;
using Mesh = Nekki.Vector.Core.Scripts.Projection.Mesh;

namespace Nekki.Vector.Core.Models
{
	public class ModelRender
	{
		private Color _Color;

		private GameObject _Layer;

		private bool _debugMode;

		private bool _IsEnabled = true;

		private GameObject _Object = new GameObject("Object");

		private GameObject _CapsulesContainer = new GameObject("Capsules");

		private List<Capsule> _Capsules = new List<Capsule>();

		private GameObject _Mesh = new GameObject("Mesh");

		private List<Triangle> _Triangles = new List<Triangle>();

		public Color Color
		{
			get => _Color;
			set => _Color = value;
		}

		public string Name
		{
			get => _Object.name;
			set => _Object.name = value;
		}

		public GameObject Layer
		{
			get => _Layer;
			set
			{
                _Layer = value;
                _Object.transform.SetParent(!(_Layer == null) ? _Layer.transform : null, false);
            }
		}

		public bool DebugMode
		{
			get => _debugMode;
			set => _debugMode = value;
		}

		public bool IsEnabled
		{
			get => _IsEnabled;
			set
			{
				_IsEnabled = value;
				_Mesh.SetActive(value);
				foreach (var c in _Capsules)
				{
					c.gameObject.SetActive(value);
				}
			}
		}

		public GameObject Object => _Object;

		public List<Capsule> Capsules => _Capsules;

		public List<Triangle> Triangles => _Triangles;

		public ModelRender()
		{
			var mesh = _Mesh.AddComponent<Mesh>();
			mesh.Base = _Triangles;
			_Mesh.transform.localPosition = Vector2.zero;
            _Mesh.transform.SetParent(_Object.transform);
            _CapsulesContainer.transform.localPosition = Vector2.zero;
			_CapsulesContainer.transform.SetParent(_Object.transform);
		}

		public void Add(List<ModelNode> Nodes)
		{
		}

		public void Add(ModelNode Node)
		{
		}

		public void Add(List<ModelLine> Lines)
		{
		}

		public void Add(ModelLine modelLine)
		{
			GameObject obj = null;
			switch (modelLine.Type)
			{
				case "Capsule":
					obj = new GameObject(modelLine.Name);
					var capsule = obj.AddComponent<Capsule>();
					capsule.Init(modelLine);
                    _Capsules.Add(capsule);
					obj.transform.SetParent(_CapsulesContainer.transform);
					break;
			}
			if (obj != null)
			{
				obj.transform.localPosition = Vector2.zero;
				obj.SetActive(_IsEnabled);
			}
		}

		public void Add(List<Triangle> Triangles)
		{
			_Triangles.AddRange(Triangles);
		}

		public void Add(Triangle Triangles)
		{
			_Triangles.Add(Triangles);
		}

		public void Add(Model Model)
		{
		}

		public Transform GetCapsulTransform(string name)
		{
			foreach (var c in _Capsules)
			{
				if (c.Name == name)
				{
					return c.middleRect;
				}
			}
			return null;
		}
	}
}
