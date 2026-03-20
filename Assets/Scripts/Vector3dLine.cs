using UnityEngine;
using static UnityEngine.UI.Extensions.Gradient2;

public class Vector3dLine
{
    public Vector3d Start { get; set; }

    public Vector3d End { get; set; }

    public double Stroke { get; set; }

    public Color Color { get; set; }

    public double Distance => Vector3d.Distance(Start, End);

    public Vector3dLine()
    {
    }

    public Vector3dLine(Vector3d start, Vector3d end)
    {
        Stroke = 1;
        Start = new Vector3d(start);
        End = new Vector3d(end);
    }

    public void Set(Vector3d start, Vector3d end)
    {
        Start.Set(start);
        End.Set(end);
    }

    public void SetZerroOnZ()
    {
        Start.Z = 0f;
        End.Z = 0f;
    }

    public static Vector3d CrossLine(Vector3dLine line1, Vector3dLine line2)
    {
        return Vector3d.Cross(line1.Start, line1.End, line2.Start, line2.End);
    }

    public bool CroosLine(Point p_point1, Point p_point2)
    {
        double num = (p_point2.Y - p_point1.Y) * (Start.X - End.X) - (Start.Y - End.Y) * (p_point2.X - p_point1.X);
        double num2 = (p_point2.Y - p_point1.Y) * (Start.X - p_point1.X) - (Start.Y - p_point1.Y) * (p_point2.X - p_point1.X);
        double num3 = (Start.Y - p_point1.Y) * (Start.X - End.X) - (Start.Y - End.Y) * (Start.X - p_point1.X);
        if ((double)num == 0.0 && (double)num2 == 0.0 && (double)num3 == 0.0)
        {
            return false;
        }
        if ((double)num == 0.0)
        {
            return false;
        }
        double num4 = num2 / num;
        double num5 = num3 / num;
        if (0f < num4 && num4 < 1f && 0f < num5 && num5 < 1f)
        {
            return true;
        }
        return false;
    }
}
