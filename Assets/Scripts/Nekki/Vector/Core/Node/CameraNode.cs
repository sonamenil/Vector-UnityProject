namespace Nekki.Vector.Core.Node
{
	public class CameraNode
	{
		public ModelNode Node
		{
			get;
		}

		public Vector3d End => Node == null ? new Vector3d() : Node.End;

		public Vector3d Start => Node == null ? new Vector3d() : Node.Start;

		private void Redraw()
		{
			Node.Radius = 8;
		}

		public CameraNode(ModelNode node)
		{
			if (node == null)
			{
				return;
			}
			Node = node;
			Node.Radius = 8;
		}
	}
}
