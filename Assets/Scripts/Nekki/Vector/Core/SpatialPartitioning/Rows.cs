using System.Collections.Generic;
using Nekki.Vector.Core.Location;

namespace Nekki.Vector.Core.SpatialPartitioning
{
	public class Rows
	{
		public List<HashSet<QuadRunner>> _Up = new List<HashSet<QuadRunner>>();

		public List<HashSet<QuadRunner>> _Down = new List<HashSet<QuadRunner>>();

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

		public HashSet<QuadRunner> Get(RectangleInt p_rectangle, HashSet<QuadRunner> result)
		{
			return null;
		}
	}
}
