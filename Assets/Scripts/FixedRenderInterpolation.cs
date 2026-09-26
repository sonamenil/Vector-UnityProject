using System;
using System.Collections.Generic;
using Nekki.Vector.Core.Node;
using UnityEngine;

/// <summary>
/// Presentation snapshots, separate from the Verlet Start/End positions used by physics.
/// Transforms are blended only for camera rendering and restored immediately afterwards.
/// </summary>
public static class FixedRenderInterpolation
{
    private sealed class NodeSnapshot
    {
        public Vector3 Previous, Current;
        public bool Valid;
    }

    private struct Pose
    {
        public Vector3 Position, Scale;
        public Quaternion Rotation;

        public static Pose Read(Transform target) => new Pose
        {
            Position = target.localPosition,
            Rotation = target.localRotation,
            Scale = target.localScale
        };

        public void Write(Transform target)
        {
            target.localPosition = Position;
            target.localRotation = Rotation;
            target.localScale = Scale;
        }

        // Unity's == operators use tolerances, which would classify slow motion
        // (especially small rotations) as stationary on every tick.
        public bool Matches(Pose other) => Position.Equals(other.Position) &&
            Rotation.Equals(other.Rotation) && Scale.Equals(other.Scale);
    }

    private sealed class TransformSnapshot
    {
        public Transform Target, Parent;
        public Pose Previous, Current, Saved;
        public bool Active, Valid, Applied;
    }

    private static readonly Dictionary<ModelNode, NodeSnapshot> Nodes = new Dictionary<ModelNode, NodeSnapshot>();
    private static readonly Dictionary<Transform, TransformSnapshot> Transforms = new Dictionary<Transform, TransformSnapshot>();
    // Render frequency can be much higher than tick frequency. Do not walk or
    // dirty every registered transform for each camera render.
    private static readonly List<TransformSnapshot> MovingTransforms = new List<TransformSnapshot>();
    private static readonly List<TransformSnapshot> AppliedTransforms = new List<TransformSnapshot>();
    private static readonly List<Transform> DeadTransforms = new List<Transform>();
    private static bool _hasTick;
    private static bool _resetDuringTick;

    public static Func<bool> IsRunning;
    public static Func<float> AlphaProvider;

    private static double _lastGameplayTickTime;
    private static float _gameplayTickDuration;

    public static float Alpha =>
        !_hasTick ||
        (IsRunning != null && !IsRunning())
            ? 1f
            : AlphaProvider != null ? Mathf.Clamp01(AlphaProvider())
            : Time.timeScale == 0 ? 1f : Fraction(
                Time.timeAsDouble - _lastGameplayTickTime,
                _gameplayTickDuration);

    public static float Fraction(double elapsed, float step) => step > 0
        ? Mathf.Clamp01((float)(elapsed / step)) : 1f;

    public static void Register(ModelNode node)
    {
        if (node != null && !Nodes.ContainsKey(node))
            Nodes.Add(node, new NodeSnapshot());
    }

    public static void Unregister(ModelNode node) => Nodes.Remove(node);

    public static void Register(Transform target)
    {
        if (target != null && !Transforms.ContainsKey(target))
            Transforms.Add(target, new TransformSnapshot { Target = target });
    }

    public static void Invalidate(ModelNode node)
    {
        if (Nodes.TryGetValue(node, out var snapshot))
            snapshot.Valid = false;
    }

    public static void Invalidate(Transform target)
    {
        if (target != null && Transforms.TryGetValue(target, out var snapshot))
            snapshot.Valid = false;
    }

    public static void BeginTick(float tickDuration = 0)
    {
        _lastGameplayTickTime = Time.fixedTimeAsDouble;
        _gameplayTickDuration = tickDuration > 0f
            ? tickDuration
            : Time.fixedDeltaTime;

        RestoreTransforms();
        _resetDuringTick = false;
        foreach (var pair in Nodes)
        {
            pair.Value.Previous = Read(pair.Key);
            pair.Value.Valid = true;
        }
        DeadTransforms.Clear();
        foreach (var pair in Transforms)
        {
            var snapshot = pair.Value;
            if (snapshot.Target == null)
            {
                DeadTransforms.Add(pair.Key);
                continue;
            }
            snapshot.Previous = Pose.Read(snapshot.Target);
            snapshot.Parent = snapshot.Target.parent;
            snapshot.Active = snapshot.Target.gameObject.activeInHierarchy;
            snapshot.Valid = true;
        }
        foreach (var target in DeadTransforms)
            Transforms.Remove(target);
    }

    public static void EndTick()
    {
        MovingTransforms.Clear();
        foreach (var pair in Nodes)
        {
            var snapshot = pair.Value;
            snapshot.Current = Read(pair.Key);
            if (!snapshot.Valid || _resetDuringTick)
                snapshot.Previous = snapshot.Current;
            snapshot.Valid = true;
        }
        foreach (var snapshot in Transforms.Values)
        {
            if (snapshot.Target == null)
                continue;
            snapshot.Current = Pose.Read(snapshot.Target);
            // Reparenting, activation and mirrored scales are discontinuities.
            if (!snapshot.Valid || _resetDuringTick || snapshot.Parent != snapshot.Target.parent ||
                snapshot.Active != snapshot.Target.gameObject.activeInHierarchy ||
                snapshot.Previous.Scale.x * snapshot.Current.Scale.x <= 0 ||
                snapshot.Previous.Scale.y * snapshot.Current.Scale.y <= 0 ||
                snapshot.Previous.Scale.z * snapshot.Current.Scale.z <= 0)
                snapshot.Previous = snapshot.Current;
            snapshot.Parent = snapshot.Target.parent;
            snapshot.Active = snapshot.Target.gameObject.activeInHierarchy;
            snapshot.Valid = true;
            if (snapshot.Active && !snapshot.Previous.Matches(snapshot.Current))
                MovingTransforms.Add(snapshot);
        }
        _hasTick = true;
    }

    private static Vector3 Read(ModelNode node) => new Vector3((float)node.Start.X, (float)node.Start.Y, (float)node.Start.Z);

    public static Vector3 Position(ModelNode node) => Position(node, Alpha);

    public static Vector3 Position(ModelNode node, float alpha)
    {
        var live = Read(node);
        if (!_hasTick || !Nodes.TryGetValue(node, out var snapshot) || !snapshot.Valid || live != snapshot.Current)
            return live;
        return Vector3.Lerp(snapshot.Previous, snapshot.Current, alpha);
    }

    public static void ApplyTransforms(float alpha)
    {
        if (!_hasTick || alpha >= 1f)
            return;
        foreach (var snapshot in MovingTransforms)
        {
            var target = snapshot.Target;
            if (target == null || !snapshot.Valid || snapshot.Applied || target.parent != snapshot.Parent ||
                !target.gameObject.activeInHierarchy || !snapshot.Active)
                continue;
            var live = Pose.Read(target);
            // An Update tween or a reset after the tick owns its new value.
            if (!live.Matches(snapshot.Current))
                continue;
            snapshot.Saved = live;
            snapshot.Applied = true;
            AppliedTransforms.Add(snapshot);
            new Pose
            {
                Position = Vector3.Lerp(snapshot.Previous.Position, snapshot.Current.Position, alpha),
                Rotation = Quaternion.Slerp(snapshot.Previous.Rotation, snapshot.Current.Rotation, alpha),
                Scale = Vector3.Lerp(snapshot.Previous.Scale, snapshot.Current.Scale, alpha)
            }.Write(target);
        }
    }

    public static void RestoreTransforms()
    {
        foreach (var snapshot in AppliedTransforms)
        {
            if (!snapshot.Applied)
                continue;
            if (snapshot.Target != null)
                snapshot.Saved.Write(snapshot.Target);
            snapshot.Applied = false;
        }
        AppliedTransforms.Clear();
    }

    public static void ResetHistory()
    {
        RestoreTransforms();
        _hasTick = false;
        _resetDuringTick = true;
        MovingTransforms.Clear();
    }

    public static void Clear()
    {
        ResetHistory();
        Nodes.Clear();
        Transforms.Clear();
        DeadTransforms.Clear();
        IsRunning = null;
        AlphaProvider = null;
    }
}
