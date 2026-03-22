using System;
using UnityEngine;

public class TriggerColider : MonoBehaviour
{
    private MeshFilter _meshFilter;

    private Mesh _mesh;

    public Action OnBecameVisibleAction;

    public Action OnBecameUnvisibleAction;

    private bool _isVisible;

    public void Init(Rectangle rectangle)
    {
        if (_meshFilter == null)
        {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.materials = Array.Empty<Material>();
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
        }
        var array = new[]
        {
            Vector3.zero,
            new Vector3(rectangle.Size.Width, 0, 0),
            new Vector3(0, rectangle.Size.Height, 0),
            new Vector3(rectangle.Size.Width, rectangle.Size.Height, 0)
        };
        _mesh.vertices = array;
        _mesh.triangles = new int[6] {0, 2, 1, 2, 3, 1};
    }

    private void OnBecameVisible()
    {
        _isVisible = true;
        OnBecameVisibleAction?.Invoke();
    }

    private void OnBecameInvisible()
    {
        _isVisible = false;
        OnBecameUnvisibleAction?.Invoke();
    }
}
