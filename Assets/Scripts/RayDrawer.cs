using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class RayDrawer : MonoBehaviour
{
    public readonly struct RayData
    {
        public readonly Vector3 origin;
        public readonly Vector3 direction;
        public readonly float radius;
        public readonly float distance;
        public readonly bool hit;
        public readonly RaycastHit2D hitInfo;

        public RayData(
            Vector3 origin,
            Vector3 direction,
            float radius,
            float distance,
            bool hit,
            RaycastHit2D hitInfo)
        {
            this.origin = origin;
            this.direction = direction.normalized;
            this.radius = radius;
            this.distance = distance;
            this.hit = hit;
            this.hitInfo = hitInfo;
        }
    }

    public readonly struct SphereData
    {
        public readonly Vector2 origin;
        public readonly float radius;
        public readonly Color color;

        public SphereData(Vector2 origin, float radius, Color color)
        {
            this.origin = origin;
            this.radius = radius;
            this.color = color;
        }
    }

    public static RayDrawer _instance;

    public static RayDrawer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("RayDrawer").AddComponent<RayDrawer>();
            }
            return _instance;
        }
    }

    private readonly List<RayData> rays = new();

    private readonly List<SphereData> spheres = new();


    // The frame in which the current rays were registered.
    private int rayFrame = -1;
    private int sphereFrame = -1;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning(
                "Multiple RayDrawer instances found. Destroying duplicate.",
                this
            );

            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void DrawRay(RayData data)
    {
        // Begin a new collection when the game frame changes.
        if (rayFrame != Time.frameCount)
        {
            rays.Clear();
            rayFrame = Time.frameCount;
        }

        rays.Add(data);
    }

    public void DrawSphere(SphereData data)
    {
        if (sphereFrame != Time.frameCount)
        {
            spheres.Clear();
            sphereFrame = Time.frameCount;
        }

        spheres.Add(data);
    }

    private void OnDrawGizmos()
    {
        // Do not draw information left over from an older frame.
        if (!Application.isPlaying)
            return;

        if (rayFrame == Time.frameCount)
        {
            foreach (RayData ray in rays)
            {
                DrawSphereCast(ray);
            }
        }

        if (sphereFrame == Time.frameCount)
        {
            foreach (var sphere in spheres)
            {
                Gizmos.color = sphere.color;
                Gizmos.DrawSphere(sphere.origin, sphere.radius);
            }
        }
    }

    private static void DrawSphereCast(RayData data)
    {
        Vector3 direction = data.direction.normalized;

        if (direction.sqrMagnitude < 0.001f)
            return;

        float drawDistance = data.hit
            ? data.hitInfo.distance
            : data.distance;

        Vector3 end =
            data.origin + direction * drawDistance;

        Gizmos.color = data.hit
            ? Color.red
            : Color.green;

        // Starting sphere.
        Gizmos.DrawWireSphere(
            data.origin,
            data.radius
        );

        // Path followed by the sphere's center.
        Gizmos.DrawLine(
            data.origin,
            end
        );

        // Sphere at the impact or maximum distance.
        Gizmos.DrawWireSphere(
            end,
            data.radius
        );

        // Contact point.
        Gizmos.DrawSphere(
            data.origin,
            8f
        );

        if (!data.hit)
            return;

        // Contact point.
        Gizmos.DrawSphere(
            data.hitInfo.point,
            8f
        );

        // Surface normal.
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(
            data.hitInfo.point,
            data.hitInfo.normal * 0.5f
        );
    }
}