using Nekki.Vector.Core.Location;
using UnityEngine;

public class QuadsRenderer : MonoBehaviour
{
    private Material material;

    [SerializeField]
    private bool overrideShowPlatforms;

    [SerializeField]
    private bool overrideShowTriggers;

    [SerializeField]
    private bool overrideShowAreas;

    private void OnEnable()
    {
        if (material == null)
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader != null)
            {
                material = new Material(shader);
            }
        }
    }

    private void OnRenderObject()
    {
        material.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.TRIANGLES);

        var tr = Sets.Current.Containers[1].Object.transform;

        foreach (var quad in Sets.Current.QuadsAll)
        {
            if (!quad.IsEnabled)
                continue;

            if ((quad.TypeClass == RunnerType.Platform || quad.TypeClass == RunnerType.Trapezoid) && (!Game.Instance.SnailSett.ShowPlatforms && !overrideShowPlatforms))
                continue;

            if (quad.TypeClass == RunnerType.Trigger && (!Game.Instance.SnailSett.ShowTriggers && !overrideShowTriggers))
                continue;

            if (quad.TypeClass == RunnerType.Area && (!Game.Instance.SnailSett.ShowAreas && !overrideShowAreas))
                continue;

            // Local corners
            Vector3 bl = quad.Point4;
            Vector3 tl = quad.Point1;
            Vector3 trc = quad.Point2;
            Vector3 br = quad.Point3;

            // Convert to world space
            bl = tr.TransformPoint(bl);
            tl = tr.TransformPoint(tl);
            trc = tr.TransformPoint(trc);
            br = tr.TransformPoint(br);

            var color = Color.green;

            switch (quad.TypeClass)
            {
                case RunnerType.Platform:
                case RunnerType.Trapezoid:
                    color = new Color(0f, 0f, 1f, 0.2f);
                    break;
                case RunnerType.Trigger:
                    color = new Color(0.5f, 1, 1, 0.2f);
                    if (quad.Choice == null || string.IsNullOrEmpty(quad.Choice.Variant) || quad.Choice.Variant == "CommonMode")
                    {
                        color = new Color(1, 0, 0, 0.2f);
                    }
                    break;
                case RunnerType.Area:
                    color = new Color(1, 1, 0, 0.2f);
                    break;
            }

            GL.Color(color);

            // Triangle 1
            GL.Vertex(bl);
            GL.Vertex(tl);
            GL.Vertex(trc);

            // Triangle 2
            GL.Vertex(trc);
            GL.Vertex(br);
            GL.Vertex(bl);
        }

        GL.End();
        GL.PopMatrix();
    }

}