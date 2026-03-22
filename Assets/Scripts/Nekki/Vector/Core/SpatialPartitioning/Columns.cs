using System.Collections.Generic;
using Nekki.Vector.Core.Location;

namespace Nekki.Vector.Core.SpatialPartitioning
{
	public class Columns
	{
		public List<Rows> _Left = new List<Rows>();

		public List<Rows> _Right = new List<Rows>();

		public Columns()
		{
			_Left.Add(null);
		}

		public void Prepare(RectangleInt p_rectangle)
		{
		}

		public HashSet<HashSet<QuadRunner>> Add(QuadRunner p_quad, RectangleInt p_rectangle)
		{
			return null;
		}

		public void Remove(QuadRunner p_quad, RectangleInt p_rectangle)
		{
		}

		public void Get(RectangleInt p_rectangle, HashSet<QuadRunner> result)
		{
		}
	}
}
