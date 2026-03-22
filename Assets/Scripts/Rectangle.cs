using UnityEngine;

public class Rectangle
{
    public Size Size { get; }

    public Point Origin { get; }

    public float MinX => Origin.X;

    public int MinXInt => (int)Origin.X;

    public float MidX => Origin.X + Size.Width / 2f;

    public float MaxX => Origin.X + Size.Width;

    public int MaxXInt => (int)(Origin.X + Size.Width);

    public float MinY => Origin.Y;

    public int MinYInt => (int)Origin.Y;

    public float MidY => Origin.Y + Size.Height / 2f;

    public float MaxY => Origin.Y + Size.Height;

    public int MaxYInt => (int)(Origin.Y + Size.Height);

    public Point TopLeft => Origin;

    public Point TopRight => new Point(Origin.X + Size.Width, Origin.Y);

    public Point BottomLeft => new Point(Origin.X, Origin.Y + Size.Height);

    public Point BottomRight => new Point(Origin.X + Size.Width, Origin.Y + Size.Height);

    public Rectangle(float x = 0f, float y = 0f, float width = 0f, float height = 0f)
    {
        Origin = new Point(x, y);
        Size = new Size(width, height);
    }

    public Rectangle(Rectangle rectangle)
    {
        Origin = new Point(rectangle.Origin.X, rectangle.Origin.Y);
        Size = new Size(rectangle.Size.Width, rectangle.Size.Height);
    }

    public static implicit operator Rect(Rectangle rect)
    {
        return new Rect(rect.MinX, rect.MinY, rect.Size.Width, rect.Size.Height);
    }

    public static implicit operator Rectangle(Rect rect)
    {
        return new Rectangle(rect.xMin, rect.yMin, rect.width, rect.height);
    }

    public static bool operator ==(Rectangle rectangle1, Rectangle rectangle2)
    {
        if (ReferenceEquals(rectangle1, rectangle2))
        {
            return true;
        }
        if (ReferenceEquals(rectangle1, null) || ReferenceEquals(rectangle2, null))
        {
            return false;
        }
        return rectangle1.Origin == rectangle2.Origin && rectangle1.Size == rectangle2.Size;
    }

    public static bool operator !=(Rectangle rectangle1, Rectangle rectangle2)
    {
        return !(rectangle1 == rectangle2);
    }

    public void Set(float x, float y, float width, float height)
    {
        Origin.X = x;
        Origin.Y = y;
        Size.Width = width;
        Size.Height = height;
    }

    public void Set(Rectangle rectangle)
    {
        Origin.X = rectangle.Origin.X;
        Origin.Y = rectangle.Origin.Y;
        Size.Width = rectangle.Size.Width;
        Size.Height = rectangle.Size.Height;
    }

    public bool Contains(Point point)
    {
        return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
    }
    public bool Contains(double x, double y)
    {
        return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
    }
    public bool Contains(Vector3d point)
    {
        return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
    }
    public bool Contains(Vector3d point, float epsilon)
    {
        return point.X >= MinX - epsilon && point.X <= MaxX + epsilon && point.Y >= MinY - epsilon && point.Y <= MaxY + epsilon;
    }

    public bool Intersect(Rectangle rect)
    {
        return !(MaxX < rect.MinX) && !(rect.MaxX < MinX) && !(MaxY < rect.MinY) && !(rect.MaxY < MinY);
    }
}
