using UnityEngine;

namespace Nekki.Vector.Core.Utilites
{
	public static class Matrix
	{
		public static bool IsIdentity(Matrix4x4 matrix)
		{
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (matrix[i, j] != Matrix4x4.identity[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

		public static Vector3 ToRotate(Matrix4x4 matrix)
		{
            Vector3 result = default(Vector3);
            result.x = Mathf.Atan2(matrix[2, 1], matrix[2, 2]);
            result.y = Mathf.Atan2(0f - matrix[2, 0], Mathf.Sqrt(matrix[2, 1] * matrix[2, 1] + matrix[2, 2] * matrix[2, 2]));
            result.z = Mathf.Atan2(matrix[1, 0], matrix[0, 0]);
            result.x *= 180f / (float)System.Math.PI;
            result.y *= 180f / (float)System.Math.PI;
            result.z *= 180f / (float)System.Math.PI;
            return result;
        }

		public static Vector3 ToPosition(Matrix4x4 matrix)
		{
            return matrix.GetColumn(3);
        }

		public static Vector3 ToScale(Matrix4x4 matrix)
		{
            float x = Mathf.Sqrt(matrix[0, 0] * matrix[0, 0] + matrix[1, 0] * matrix[1, 0]) * Mathf.Sign(matrix[0, 0]);
            float y = Mathf.Sqrt(matrix[0, 1] * matrix[0, 1] + matrix[1, 1] * matrix[1, 1]) * Mathf.Sign(matrix[1, 1]);
            float z = 1f;
            return new Vector3(x, y, z);
        }

		public static bool ContainsSkew(Matrix4x4 matrix)
		{
            QRDecomposition qRDecomposition = new QRDecomposition(matrix.transpose);
            return qRDecomposition.ContainsSkew();
        }
	}
}
