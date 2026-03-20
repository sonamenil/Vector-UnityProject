using System.Collections.Generic;
using UnityEngine;

namespace Nekki.Vector.Core.Node
{
	public class MeshNode
	{
		public int[] Triangles;

		public Vector3[] Vertices;

		private readonly List<ModelNode> _verticesNode;

		private List<int> _trianglesList;

		public void AddTriangle(ModelNode node1, ModelNode node2, ModelNode node3)
		{
		}

		private int GetNodeIndex(ModelNode node)
		{
			return 0;
		}

		public void Init()
		{
		}

		public void Render()
		{
		}
	}
}
