namespace Nekki.Vector.Core.Utilites
{
	public class Matrix22
	{
		private float _a;

		private float _b;

		private float _c;

		private float _d;

		public float D => _a * _d - _b * _c;

		public Matrix22(float a = 1f, float b = 0f, float c = 0f, float d = 1f)
		{
            Set(a, b, c, d);
        }

		public void Set(float a, float b, float c, float d)
		{
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }

        public static Matrix22 operator +(Matrix22 matrix1, Matrix22 matrix2)
        {
            Matrix22 matrix3 = new Matrix22();
            matrix3._a = matrix1._a + matrix2._a;
            matrix3._b = matrix1._b + matrix2._b;
            matrix3._c = matrix1._c + matrix2._c;
            matrix3._d = matrix1._d + matrix2._d;
            return matrix3;
        }

        public static Matrix22 operator -(Matrix22 matrix1, Matrix22 matrix2)
        {
            Matrix22 matrix3 = new Matrix22();
            matrix3._a = matrix1._a - matrix2._a;
            matrix3._b = matrix1._b - matrix2._b;
            matrix3._c = matrix1._c - matrix2._c;
            matrix3._d = matrix1._d - matrix2._d;
            return matrix3;
        }

        public static Matrix22 operator /(Matrix22 matrix, float arg2)
        {
            Matrix22 matrix2 = new Matrix22();
            matrix2._a = matrix._a / arg2;
            matrix2._b = matrix._b / arg2;
            matrix2._c = matrix._c / arg2;
            matrix2._d = matrix._d / arg2;
            return matrix2;
        }

        public static Matrix22 operator *(Matrix22 matrix, float arg2)
        {
            Matrix22 matrix2 = new Matrix22();
            matrix2._a = matrix._a * arg2;
            matrix2._b = matrix._b * arg2;
            matrix2._c = matrix._c * arg2;
            matrix2._d = matrix._d * arg2;
            return matrix2;
        }

        public static Vector3f operator *(Vector3f vector, Matrix22 matrix)
        {
            Vector3f vector3f = new Vector3f();
            vector3f.X = matrix._a * vector.X + matrix._c * vector.Y;
            vector3f.Y = matrix._b * vector.X + matrix._d * vector.Y;
            return vector3f;
        }

        public static Matrix22 operator *(Matrix22 matrix1, Matrix22 matrix2)
        {
            float a = matrix1._a * matrix2._a + matrix1._b * matrix2._c;
            float b = matrix1._a * matrix2._b + matrix1._b * matrix2._d;
            float c = matrix1._c * matrix2._a + matrix1._d * matrix2._c;
            float d = matrix1._c * matrix2._b + matrix1._d * matrix2._d;
            return new Matrix22(a, b, c, d);
        }

        public static Matrix22 GenerateInterpolationDeltaMatrix(Matrix22 matrix1, Matrix22 matrix2, int steps)
		{
            Matrix22 matrix3 = new Matrix22();
            return (matrix2 - matrix1) / steps;
        }

		public Matrix22 GetInverseMatrix()
		{
            if (D != 0f)
            {
                Matrix22 matrix = new Matrix22(_d, 0f - _b, 0f - _c, _a);
                return matrix * (1f / D);
            }
            return new Matrix22();
        }
	}
}
