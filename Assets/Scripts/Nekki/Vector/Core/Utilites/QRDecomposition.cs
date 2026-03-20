using UnityEngine;

namespace Nekki.Vector.Core.Utilites
{
	internal class QRDecomposition
	{
		public const float Delta = 0.001f;

		private float _scaleX;

		private float _scaleY;

		private Matrix4x4 A;

		private Matrix4x4 Q;

		private Matrix4x4 R;

		public bool DecomposedSuccessfully
		{
			get;
			private set;
		}

		public float ScaleX
		{
			get
			{
                if (DecomposedSuccessfully)
                {
                    return R.m00;
                }
                return _scaleX;
            }
		}

		public float ScaleY
		{
            get
            {
                if (DecomposedSuccessfully)
                {
                    return R.m11;
                }
                return _scaleY;
            }
        }

		public Matrix4x4 Rotation => Q;

		public QRDecomposition(Matrix4x4 a)
		{
            A = a;
            CalculateDecomposition();
        }

		private void CalculateDecomposition()
		{
            int num = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (num >= 2)
                    {
                        break;
                    }
                    if (System.Math.Abs(A[i, j]) > 0.001f)
                    {
                        num++;
                    }
                }
            }
            if (num < 2)
            {
                DecomposedSuccessfully = false;
                ExtractWHFromMatrix();
                return;
            }
            Matrix4x4 matrix4x = default(Matrix4x4);
            Q = default(Matrix4x4);
            matrix4x.SetColumn(0, A.GetColumn(0));
            for (int k = 1; k < 4; k++)
            {
                Vector4 vector = default(Vector4);
                for (int l = 0; l < k; l++)
                {
                    Vector4 vector2 = Vector4.Project(A.GetColumn(k), matrix4x.GetColumn(k - l - 1));
                    vector += vector2;
                }
                Vector4 v = A.GetColumn(k) - vector;
                matrix4x.SetColumn(k, v);
            }
            for (int m = 0; m < 4; m++)
            {
                float magnitude = matrix4x.GetColumn(m).magnitude;
                for (int n = 0; n < 4; n++)
                {
                    Q[n, m] = matrix4x[n, m] / magnitude;
                }
            }
            R = Q.transpose * A;
            DecomposedSuccessfully = true;
        }

		private void ExtractWHFromMatrix()
		{
            _scaleX = 0f;
            _scaleY = 0f;
            if (System.Math.Abs(A.m00) > 0.001f)
            {
                _scaleX = A.m00;
            }
            else if (System.Math.Abs(A.m01) > 0.001f)
            {
                _scaleX = A.m01;
            }
            if (System.Math.Abs(A.m11) > 0.001f)
            {
                _scaleY = A.m11;
            }
            else if (System.Math.Abs(A.m10) > 0.001f)
            {
                _scaleY = A.m10;
            }
            Q = Matrix4x4.identity;
        }

		public bool ContainsSkew()
		{
            return DecomposedSuccessfully && (System.Math.Abs(R.m10) > 0.001f || System.Math.Abs(R.m01) > 0.001f);

        }
    }
}
