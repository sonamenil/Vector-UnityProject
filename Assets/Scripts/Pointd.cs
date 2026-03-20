using System.Xml;
using UnityEngine;

public class Pointd
{
    private double _X;

    private double _Y;

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

    public Pointd()
    {
        _X = 0;
        _Y = 0;
    }

    public Pointd(Point Point)
    {
        _X = (double)Point.X;
        _Y = (double)Point.Y;
    }

    public Pointd(Pointd Point)
    {
        _X = Point.X;
        _Y = Point.Y;
    }

    public Pointd(float X, float Y)
    {
        _X = (double)X;
        _Y = (double)Y;
    }

    public Pointd(double X, double Y)
    {
        _X = X;
        _Y = Y;
    }

    public Pointd(int X, int Y)
    {
        _X = (double)X;
        _Y = (double)Y;
    }

    public static Pointd operator -(Pointd Point1, Pointd Point2)
    {
        return new Pointd(Point1.X - Point2.X, Point1.Y - Point2.Y);
    }

    public static Pointd operator +(Pointd Point1, Pointd Point2)
    {
        return new Pointd(Point1.X + Point2.X, Point1.Y + Point2.Y);
    }

    public static bool operator ==(Pointd Point1, Pointd Point2)
    {
        if (object.ReferenceEquals(Point1, Point2))
        {
            return true;
        }
        if (object.ReferenceEquals(Point1, null) || object.ReferenceEquals(Point2, null))
        {
            return false;
        }
        return Point1.X == Point2.X && Point1.Y == Point2.Y;
    }

    public static bool operator !=(Pointd Point1, Pointd Point2)
    {
        return !(Point1 == Point2);
    }

    public static implicit operator Vector3(Pointd Vector)
    {
        return new Vector3((float)Vector.X, (float)Vector.Y, 0f);
    }

    public void Set(Pointd p_point)
    {
        _X = p_point._X;
        _Y = p_point._Y;
    }

    public Pointd Add(Pointd Point)
    {
        _X += Point.X;
        _Y += Point.Y;
        return this;
    }

    public Pointd Subtract(Pointd Point)
    {
        _X -= Point.X;
        _Y -= Point.Y;
        return this;
    }

    public Pointd Multiply(float Value)
    {
        _X *= Value;
        _Y *= Value;
        return this;
    }

    public void Round(int Pow)
    {
        _X = Nekki.Vector.Core.Utilites.Math.Round(_X, Pow);
    }

    public void IRound(int Pow)
    {
        _X = Nekki.Vector.Core.Utilites.Math.Round(_X, 1f);
    }

    public double DistBP(Pointd Point)
    {
        return (_X - Point._X) * (_X - Point._Y) + (_Y - Point._Y) * (_Y - Point._Y);
    }

    public static Pointd Create(XmlNode Node)
    {
        return new Pointd(float.Parse(Node.Attributes["X"].Value), float.Parse(Node.Attributes["Y"].Value));
    }

    public Pointd Clone()
    {
        return this;
    }
}
