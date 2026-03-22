using System;
using Vector3 = UnityEngine.Vector3;

public class Vector3d
{
    private const double Pow = 10000000.0;

    private double _X;

    private double _Y;

    private double _Z;


    public double X
    {
        get => _X;
        set => _X = value;
    }

    public double FloorX
    {
        get => 0.0;
        set
        {
        }
    }

    public double Y
    {
        get => _Y;
        set => _Y = value;
    }

    public double FloorY
    {
        get => 0.0;
        set
        {
        }
    }

    public double Z
    {
        get => _Z;
        set => _Z = value;
    }

    public double FloorZ
    {
        get => 0.0;
        set
        {
        }
    }

    public double Length => Math.Sqrt(_X * _X + _Y * _Y + _Z * _Z);

    public Vector3d Normalized => null;

    public static Vector3d Right => new Vector3d(1, 0, 0);

    public static Vector3d Up => new Vector3d(0, 1, 0);

    public static Vector3d Forward => new Vector3d(0, 1, 0);

    public static Vector3d Zero => new Vector3d(0, 0, 0);

    public static Vector3d One => new Vector3d(1, 1, 1);

    public Vector3d(float X, float Y, float Z)
    {
        _X = X;
        _Y = Y;
        _Z = Z;
    }

    public Vector3d(double X, double Y, double Z)
    {
        _X = X;
        _Y = Y;
        _Z = Z;
    }

    public Vector3d(float X, float Y)
    {
        _X = X;
        _Y = Y;
        _Z = 0;
    }

    public Vector3d(double X, double Y)
    {
        _X = X;
        _Y = Y;
        _Z = 0;
    }

    public Vector3d(Vector3f vector)
    {
        _X = vector.X;
        _Y = vector.Y;
        _Z = vector.Z;
    }

    public Vector3d(Vector3d vector)
    {
        _X = vector.X;
        _Y = vector.Y;
        _Z = vector.Z;
    }

    public Vector3d(Vector3 vector)
    {
        _X = vector.x;
        _Y = vector.y;
        _Z = vector.z;
    }

    public Vector3d()
    {
        _X = 0;
        _Y = 0;
        _Z = 0;
    }

    public static Vector3d operator +(Vector3d vector, float value)
    {
        return new Vector3d(vector.X + value, vector.Y + value, vector.Z + value);
    }

    public static Vector3d operator +(Vector3d vector, double value)
    {
        return new Vector3d(vector.X + value, vector.Y + value, vector.Z + value);
    }

    public static Vector3d operator +(Vector3d v1, Vector3d v2)
    {
        return new Vector3d(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }
    public static Vector3d operator +(Vector3d v1, Vector3f v2)
    {
        return new Vector3d(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }
    public static Vector3d operator -(Vector3d vector, float value)
    {
        return new Vector3d(vector.X - value, vector.Y - value, vector.Z - value);
    }

    public static Vector3d operator -(Vector3d vector, double value)
    {
        return new Vector3d(vector.X - value, vector.Y - value, vector.Z - value);
    }

    public static Vector3d operator -(Vector3d v1, Vector3d v2)
    {
        return new Vector3d(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }

    public static Vector3d operator -(Vector3d Vector1, Vector3f Vector2)
    {
        return new Vector3d(Vector1.X - Vector2.X, Vector1.Y - Vector2.Y, Vector1.Z - Vector2.Z);
    }

    public static Vector3d operator *(Vector3d vector, float value)
    {
        return new Vector3d(vector.X * value, vector.Y * value, vector.Z * value);
    }

    public static Vector3d operator *(Vector3d vector, double value)
    {
        return new Vector3d(vector.X * value, vector.Y * value, vector.Z * value);
    }

    public static double operator *(Vector3d v1, Vector3d v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }

    public static double operator *(Vector3d v1, Vector3f v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }

    public static implicit operator Vector3(Vector3d Vector)
    {
        return new Vector3((float)Vector.X, (float)Vector.Y, (float)Vector.Z);
    }

    public static implicit operator Vector3d(Vector3 Vector)
    {
        return new Vector3d(Vector.x, Vector.y, Vector.z);
    }

    public static double Distance(Vector3d v1, Vector3d v2)
    {
        return (v2 - v1).Length;
    }

    public double Distance(Vector3d vector)
    {
        return (this - vector).Length;
    }

    public Vector3d Add(float value)
    {
        _X += value;
        _Y += value;
        _Z += value;
        return this;
    }

    public Vector3d Add(double value)
    {
        _X += value;
        _Y += value;
        _Z += value;
        return this;
    }

    public Vector3d Add(float x, float y, float z)
    {
        _X += x;
        _Y += y;
        _Z += z;
        return this;
    }

    public Vector3d Add(double x, double y, double z)
    {
        _X += x;
        _Y += y;
        _Z += z;
        return this;
    }

    public Vector3d Add(Vector3f vector)
    {
        _X += vector.X;
        _Y += vector.Y;
        _Z += vector.Z;
        return this;
    }

    public Vector3d Add(Vector3d vector)
    {
        _X += vector.X;
        _Y += vector.Y;
        _Z += vector.Z;
        return this;
    }

    public Vector3d Add(Vector3d vector, double multy)
    {
        _X += vector.X * multy;
        _Y += vector.Y * multy;
        _Z += vector.Z * multy;
        return this;
    }

    public Vector3d Add(Point point)
    {
        _X += point.X;
        _Y += point.Y;
        return this;
    }

    public Vector3d Add(Pointd point)
    {
        _X += point.X;
        _Y += point.Y;
        return this;
    }

    public Vector3d Subtract(float val)
    {
        _X -= val;
        _Y -= val;
        _Z -= val;
        return this;
    }

    public Vector3d Subtract(double value)
    {
        _X -= value;
        _Y -= value;
        _Z -= value;
        return this;
    }

    public Vector3d Subtract(float x, float y, float z)
    {
        _X -= x;
        _Y -= y;
        _Z -= z;
        return this;
    }

    public Vector3d Subtract(double x, double y, double z)
    {
        _X -= x;
        _Y -= y;
        _Z -= z;
        return this;
    }

    public Vector3d Subtract(Vector3f vector)
    {
        _X -= vector.X;
        _Y -= vector.Y;
        _Z -= vector.Z;
        return this;
    }

    public Vector3d Subtract(Vector3d vector)
    {
        _X -= vector.X;
        _Y -= vector.Y;
        _Z -= vector.Z;
        return this;
    }

    public Vector3d Subtract(Point point)
    {
        _X -= point.X;
        _Y -= point.Y;
        return this;
    }

    public Vector3d Subtract(Pointd point)
    {
        _X -= point.X;
        _Y -= point.Y;
        return this;
    }

    public Vector3d Multiply(float value)
    {
        _X *= value;
        _Y *= value;
        _Z *= value;
        return this;
    }

    public Vector3d Multiply(double value)
    {
        _X *= value;
        _Y *= value;
        _Z *= value;
        return this;
    }

    public Vector3d Multiply(float x, float y, float z)
    {
        _X *= x;
        _Y *= y;
        _Z *= z;
        return this;
    }

    public Vector3d Multiply(double x, double y, double z)
    {
        _X *= x;
        _Y *= y;
        _Z *= z;
        return this;
    }

    public Vector3d Cross(Vector3d vector)
    {
        return new Vector3d(_Y * vector.Z - _Z * vector.Y, _Z * vector.X - _X * vector.Z, _X * vector.Y - _Y - vector.X);
    }

    public Vector3d Cross(Vector3f vector)
    {
        return new Vector3d(_Y * vector.Z - _Z * vector.Y, _Z * vector.X - _X * vector.Z, _X * vector.Y - _Y - vector.X);
    }

    public static Vector3d Cross(Vector3d v1, Vector3d v2, Vector3d v3, Vector3d v4)
    {
        var num = v2.Y - v1.Y;
        var num2 = v2.X - v1.X;
        var num3 = v3.X - v4.X;
        var num4 = v3.Y - v4.Y;

        var num5 = num * num3 - num4 * num2;
        if (num5 == 0)
        {
            return null;
        }
        var num6 = v3.X - v1.X;
        var num7 = v3.Y - v1.Y;
        var num8 = (num * num6 - num7 * num2) / num5;

        if (num8 > 0 && num8 < 1)
        {
            var num9 = (num3 * num7 - num4 * num6) / num5;
            if (num9 > 0 && num9 < 1)
            {
                return new Vector3d(v1.X + num2 * num9, v1.Y + num * num9, 0);
            }
        }
        return null;
    }

    public static Vector3d Middle(Vector3d v1, Vector3d v2)
    {
        return v1 + (v2 - v1) * 0.5f;
    }

    public static void Middle(Vector3d v1, Vector3d v2, Vector3d p_result)
    {
        p_result._X = v1._X + (v2._X - v1._X) * 0.5f;
        p_result._Y = v1._Y + (v2._Y - v1._Y) * 0.5f;
        p_result._Z = v1._Z + (v2._Z - v1._Z) * 0.5f;
    }

    public static Vector3d Closest(Vector3d point, Vector3d linePoint, Vector3d lineDirection)
    {
        Vector3d vector3d = point - linePoint;
        double num = vector3d * lineDirection;
        double num2 = lineDirection * lineDirection;
        double num3 = 0f;
        if (num2 != 0f)
        {
            num3 = num / num2;
        }
        return linePoint + lineDirection * num3;
    }

    public void Reset()
    {
        _X = 0;
        _Y = 0;
        _Z = 0;
    }

    public static Vector3d Round(Vector3d vector, double pow)
    {
        vector.X = Nekki.Vector.Core.Utilites.Math.Round(vector.X, pow);
        vector.Y = Nekki.Vector.Core.Utilites.Math.Round(vector.Y, pow);
        vector.Z = Nekki.Vector.Core.Utilites.Math.Round(vector.Z, pow);
        return vector;
    }

    public Vector3d Round(double pow)
    {
        _X = Nekki.Vector.Core.Utilites.Math.Round(_X, pow);
        _Y = Nekki.Vector.Core.Utilites.Math.Round(_Y, pow);
        _Z = Nekki.Vector.Core.Utilites.Math.Round(_Z, pow);
        return this;
    }

    public Vector3d Normalize()
    {
        double num = Length;
        if (num != 0f)
        {
            num = 1f / num;
        }
        _X *= num;
        _Y *= num;
        _Z *= num;
        return this;
    }

    public static Vector3d Normal(Vector3d v1, Vector3d v2)
    {
        return (v1 - v2).Cross(Forward).Normalize();
    }

    public static double Factor(Vector3d point, Vector3d start, Vector3d end)
    {
        var a = new Vector3d(start.X, start.Y, 0);
        var b = new Vector3d(end.X, end.Y, 0);
        var p = new Vector3d(point.X, point.Y, 0);

        double length = Vector3d.Distance(a, b);
        if (length <= 1e-6)
            return 0.5;

        double t = Vector3d.Distance(a, p) / length;
        return System.Math.Max(0.0, System.Math.Min(1.0, t));
    }

    public Vector3d Clone()
    {
        return new Vector3d(_X, _Y, _Z);
    }

    public Vector3d Set(Vector3d vector)
    {
        _X = vector.X;
        _Y = vector.Y;
        _Z = vector.Z;
        return this;
    }

    public void Set(Vector3dStruct vector)
    {
        _X = vector.X;
        _Y = vector.Y;
        _Z = vector.Z;
    }

    public Vector3d Set(double x, double y, double z)
    {
        _X = x;
        _Y = y;
        _Z = z;
        return this;
    }

    public Vector3d Set(Vector3 vector)
    {
        _X = vector.x;
        _Y = vector.y;
        _Z = vector.z;
        return this;
    }

    public override string ToString()
    {
        return $"X={X} Y={Y} Z={Z}";
    }
}
