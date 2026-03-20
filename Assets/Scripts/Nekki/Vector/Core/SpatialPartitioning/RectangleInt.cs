namespace Nekki.Vector.Core.SpatialPartitioning
{
	public class RectangleInt
	{
		private int _MinX;

		private int _MaxX;

		private int _MinY;

		private int _MaxY;

		public int MinX => 0;

		public int MidX => 0;

		public int MaxX => 0;

		public int MinY => 0;

		public int MidY => 0;

		public int MaxY => 0;

		public RectangleInt()
		{
		}

		public RectangleInt(int MinX, int MinY, int MaxX, int MaxY)
		{
		}

		public RectangleInt(RectangleInt Rectangle)
		{
		}

		public static bool operator ==(RectangleInt p_rectangle1, RectangleInt p_rectangle2)
		{
			return false;
		}

		public static bool operator !=(RectangleInt p_rectangle1, RectangleInt p_rectangle2)
		{
			return false;
		}

		public void Set(int p_minX, int p_minY, int p_maxX, int p_maxY)
		{
		}

		public void Set(RectangleInt p_rectangle)
		{
		}

		public bool Intersect(Rectangle p_rectangle)
		{
			return false;
		}
	}
}
