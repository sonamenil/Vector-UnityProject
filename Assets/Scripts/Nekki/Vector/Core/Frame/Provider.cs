using UnityEngine;

namespace Nekki.Vector.Core.Frame
{
	public class Provider
	{
		public Vector3[][] Data
		{
			get;
		}

		public int Length => Data.Length;

		public Vector3[] this[int p_index] => null;

		public Provider(int count)
		{
            Data = new Vector3[count][];
        }

        public void Add(Vector3[] p_item, int p_index)
		{
            Data[p_index] = p_item;
        }
    }
}
