using System.Collections.Generic;
using Nekki.Vector.Core.Location;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class QuadsRenderer : MonoBehaviour
{
    public static QuadsRenderer Instance;

    [SerializeField] private bool overrideShowPlatforms;
    [SerializeField] private bool overrideShowTriggers;
    [SerializeField] private bool overrideShowAreas;

    private Mesh mesh;
    private Material material;

    private readonly List<Vector3> vertices = new();
    private readonly List<int> triangles = new();
    private readonly List<Color> colors = new();

    private List<TextMesh> labels = new();

    int TransformationQuads = 0;

    private void Awake()
    {
        mesh = new Mesh
        {
            name = "Quads Debug Mesh"
        };

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        GetComponent<MeshFilter>().sharedMesh = mesh;

        Shader shader = Shader.Find("Sprites/Default");
        material = new Material(shader);

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;
        meshRenderer.sortingOrder = 1;
        meshRenderer.sortingLayerName = "Debug";

        Instance = this;

        foreach (var quad in Sets.Current.QuadsAll)
        {
            quad.OnTransformationStart += OnTransformationStart;
            quad.OnTransformationEnd += OnTransformationEnd;
        }

        BuildMesh();
    }

    public void OnTransformationStart(QuadRunner quad)
    {
        TransformationQuads++;
    }

    public void OnTransformationEnd(QuadRunner quad)
    {
        TransformationQuads--;
    }

    private void LateUpdate()
    {
        if (TransformationQuads > 0)
        {
            BuildMesh();
        }
    }

    private void BuildMesh()
    {
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        foreach (var quad in Sets.Current.QuadsAll)
        {
            if (!ShouldRenderQuad(quad))
                continue;

            int startIndex = vertices.Count;

            Vector3 bl = quad.Point4;
            Vector3 tl = quad.Point1;
            Vector3 tr = quad.Point2;
            Vector3 br = quad.Point3;

            Color color = GetQuadColor(quad);

            vertices.Add(bl);
            vertices.Add(tl);
            vertices.Add(tr);
            vertices.Add(br);

            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);

            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);

            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
            triangles.Add(startIndex + 0);
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetColors(colors);

        mesh.RecalculateBounds();
    }

    private bool ShouldRenderQuad(QuadRunner quad)
    {
        if (!quad.IsEnabled)
            return false;

        if ((quad.TypeClass == RunnerType.Platform || quad.TypeClass == RunnerType.Trapezoid) &&
            (!Game.Instance.SnailSett.ShowPlatforms && !overrideShowPlatforms))
            return false;

        if (quad.TypeClass == RunnerType.Trigger &&
            (!Game.Instance.SnailSett.ShowTriggers && !overrideShowTriggers))
            return false;

        if (quad.TypeClass == RunnerType.Area &&
            (!Game.Instance.SnailSett.ShowAreas && !overrideShowAreas))
            return false;

        return true;
    }

    private Color GetQuadColor(QuadRunner quad)
    {
        switch (quad.TypeClass)
        {
            case RunnerType.Platform:
            case RunnerType.Trapezoid:
                return new Color(0f, 0f, 1f, 0.2f);

            case RunnerType.Trigger:
                if (quad.Choice == null ||
                    string.IsNullOrEmpty(quad.Choice.Variant) ||
                    quad.Choice.Variant == "CommonMode")
                {
                    return new Color(1f, 0f, 0f, 0.2f);
                }

                return new Color(0.5f, 1f, 1f, 0.2f);

            case RunnerType.Area:
                return new Color(1f, 1f, 0f, 0.2f);

            default:
                return Color.green;
        }
    }

    public TextMesh AddLabel(Vector3 position, string text)
    {
        var label = new GameObject("Label").AddComponent<TextMesh>();
        label.transform.SetParent(transform, false);
        label.transform.localPosition = position;
        label.text = text;
        label.fontSize = 500;
        label.transform.localScale = new Vector3(1, -1);
        label.anchor = TextAnchor.MiddleCenter;

        labels.Add(label);

        return label;
    }

    public void RemoveLabel(TextMesh label)
    {
        labels.Remove(label);
        Destroy(label.gameObject);
    }
}