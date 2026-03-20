using UnityEngine;

public struct Vector3dStruct
{
	private const double Pow = 10000000.0;

	private double _X;

	private double _Y;

	private double _Z;

    public double X
    {
        get
        {
            return _X;
        }
        set
        {
            _X = value;
        }
    }

    public double FloorX
    {
        get
        {
            return 0.0;
        }
        set
        {
        }
    }

    public double Y
    {
        get
        {
            return _Y;
        }
        set
        {
            _Y = value;
        }
    }

    public double FloorY
    {
        get
        {
            return 0.0;
        }
        set
        {
        }
    }

    public double Z
    {
        get
        {
            return _Z;
        }
        set
        {
            _Z = value;
        }
    }

    public double FloorZ
    {
        get
        {
            return 0.0;
        }
        set
        {
        }
    }
    public double Length => Mathf.Sqrt((float)_X * (float)_X + (float)_Y * (float)_Y + (float)_Z * (float)_Z);

    public Vector3dStruct(float X, float Y, float Z)
	{
        _X = X;
        _Y = Y;
        _Z = Z;
    }

	public Vector3dStruct(double X, double Y, double Z)
	{
        _X = X;
        _Y = Y;
        _Z = Z;
    }

	public Vector3dStruct(double X, double Y)
	{
        _X = X;
        _Y = Y;
        _Z = 0;
    }

	public Vector3dStruct(Vector3d vector)
	{
        _X = vector.X;
        _Y = vector.Y;
        _Z = vector.Z;
    }

	public static double operator *(Vector3dStruct v1, Vector3dStruct v2)
	{
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }

	public void Add(float value)
	{
        _X += value;
        _Y += value;
        _Z += value;
    }

	public void Add(double value)
	{
        _X += value;
        _Y += value;
        _Z += value;
    }

	public void Add(float x, float y, float z)
	{
        _X += x;
        _Y += y;
        _Z += z;
    }

	public void Add(double x, double y, double z)
	{
        _X += x;
        _Y += y;
        _Z += z;
    }

	public void Add(Vector3f vector)
	{
        _X += vector.X;
        _Y += vector.Y;
        _Z += vector.Z;
    }

	public void Add(Vector3d vector)
	{
        _X += vector.X;
        _Y += vector.Y;
        _Z += vector.Z;
    }

	public void Add(Vector3d vector, double multy)
	{
        _X += vector.X * multy;
        _Y += vector.Y * multy;
        _Z += vector.Z * multy;
    }

	public void Add(Vector3dStruct vector)
	{
        _X += vector.X;
        _Y += vector.Y;
        _Z += vector.Z;
    }

	public void Add(Point point)
	{
        _X += point.X;
        _Y += point.Y;
    }

	public void Add(Pointd point)
	{
        _X += point.X;
        _Y += point.Y;
    }

	public void Subtract(Vector3d vector)
	{
        _X -= vector.X;
        _Y -= vector.Y;
        _Z -= vector.Z;
    }

	public void Subtract(Vector3dStruct vector)
	{
        _X -= vector.X;
        _Y -= vector.Y;
        _Z -= vector.Z;
    }

	public void Multiply(double value)
	{
        _X *= value;
        _Y *= value;
        _Z *= value;
    }

	public static Vector3dStruct Middle(Vector3d v1, Vector3d v2)
	{
        return new Vector3dStruct(v1 + (v2 - v1) * 0.5f);
    }

	public static Vector3dStruct Closest(Vector3d point, Vector3d linePoint, Vector3d lineDirection)
	{
        Vector3d vector3d = point - linePoint;
        double num = vector3d * lineDirection;
        double num2 = lineDirection * lineDirection;
        double num3 = 0f;
        if (num2 != 0f)
        {
            num3 = num / num2;
        }
        return new Vector3dStruct(linePoint + lineDirection * num3);
    }

	public static Vector3dStruct Round(Vector3d vector, double pow)
	{
        vector.X = Nekki.Vector.Core.Utilites.Math.Round(vector.X, pow);
        vector.Y = Nekki.Vector.Core.Utilites.Math.Round(vector.Y, pow);
        vector.Z = Nekki.Vector.Core.Utilites.Math.Round(vector.Z, pow);
        return new Vector3dStruct(vector);
    }

	public void Set(Vector3d vector)
	{
        _X = vector.X;
        _Y = vector.Y;
        _Z = vector.Z;
    }
}
