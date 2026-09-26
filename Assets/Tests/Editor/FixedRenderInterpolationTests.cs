using NUnit.Framework;
using Nekki.Vector.Core.Node;
using UnityEngine;

public class FixedRenderInterpolationTests
{
    [Test]
    public void ScaledClockFreezesRenderedPoseAndResumesWithoutSnapping()
    {
        var clock = new GameplayClock();
        var node = new ModelNode(new Vector3d());
        FixedRenderInterpolation.Register(node);
        FixedRenderInterpolation.AlphaProvider = () => clock.Alpha;
        FixedRenderInterpolation.BeginTick();
        node.Start.X = 10;
        FixedRenderInterpolation.EndTick();

        clock.Advance(0.5, 0.5f, 1);
        Assert.That(FixedRenderInterpolation.Position(node).x, Is.EqualTo(2.5f));
        clock.Advance(100, 0, 1);
        Assert.That(FixedRenderInterpolation.Position(node).x, Is.EqualTo(2.5f));
        clock.Advance(0.25, 1, 1);
        Assert.That(FixedRenderInterpolation.Position(node).x, Is.EqualTo(5f));
        Assert.That(node.Start.X, Is.EqualTo(10));
    }

    [Test]
    public void StationaryTransformsRemainUnchangedDuringRendering()
    {
        var objects = new GameObject[1000];
        try
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i] = new GameObject("static scenery");
                FixedRenderInterpolation.Register(objects[i].transform);
            }
            FixedRenderInterpolation.BeginTick();
            FixedRenderInterpolation.EndTick();
            foreach (var obj in objects)
                obj.transform.hasChanged = false;
            FixedRenderInterpolation.ApplyTransforms(0.5f);
            FixedRenderInterpolation.RestoreTransforms();
            foreach (var obj in objects)
                Assert.That(obj.transform.hasChanged, Is.False, "Stationary objects must not dirty Unity's transform hierarchy");
        }
        finally
        {
            foreach (var obj in objects)
                if (obj != null) Object.DestroyImmediate(obj);
        }
    }

    [Test]
    public void CurrentPoseRenderingLeavesMovingTransformUnchanged()
    {
        var obj = new GameObject("moving scenery");
        try
        {
            FixedRenderInterpolation.Register(obj.transform);
            FixedRenderInterpolation.BeginTick();
            obj.transform.localPosition = Vector3.right * 10;
            FixedRenderInterpolation.EndTick();
            obj.transform.hasChanged = false;
            FixedRenderInterpolation.ApplyTransforms(1f);
            FixedRenderInterpolation.RestoreTransforms();
            Assert.That(obj.transform.hasChanged, Is.False);
        }
        finally { Object.DestroyImmediate(obj); }
    }

    [Test]
    public void SmallRotationsStillInterpolate()
    {
        var obj = new GameObject("slowly rotating scenery");
        try
        {
            FixedRenderInterpolation.Register(obj.transform);
            FixedRenderInterpolation.BeginTick();
            obj.transform.localRotation = Quaternion.Euler(0, 0, 0.1f);
            FixedRenderInterpolation.EndTick();
            FixedRenderInterpolation.ApplyTransforms(0.5f);
            Assert.That(obj.transform.localRotation.z, Is.EqualTo(Quaternion.Euler(0, 0, 0.05f).z).Within(0.000001f));
            FixedRenderInterpolation.RestoreTransforms();
            Assert.That(obj.transform.localRotation.z, Is.EqualTo(Quaternion.Euler(0, 0, 0.1f).z).Within(0.000001f));
        }
        finally { Object.DestroyImmediate(obj); }
    }

    [Test, Explicit("Native Unity performance regression; run on an idle Editor")]
    public void RenderCostDoesNotScaleWithStationaryScenery()
    {
        double small = MeasureRenderCost(1000);
        double large = MeasureRenderCost(10000);
        // Keep timing assertions out of normal test runs. A generous allowance
        // absorbs host noise while catching the former full-registry render loop.
        Assert.That(large, Is.LessThan(small * 5 + 25),
            "1000 vs 10000 registered transforms (10 moving): " + small + " vs " + large + " ms");
    }

    private static double MeasureRenderCost(int count)
    {
        FixedRenderInterpolation.Clear();
        var objects = new GameObject[count];
        try
        {
            for (int i = 0; i < count; i++)
            {
                objects[i] = new GameObject("benchmark scenery");
                FixedRenderInterpolation.Register(objects[i].transform);
            }
            FixedRenderInterpolation.BeginTick();
            for (int i = 0; i < 10; i++)
                objects[i].transform.localPosition = Vector3.right * 10;
            FixedRenderInterpolation.EndTick();
            for (int i = 0; i < 20; i++)
            {
                FixedRenderInterpolation.ApplyTransforms(0.5f);
                FixedRenderInterpolation.RestoreTransforms();
            }
            var timer = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                FixedRenderInterpolation.ApplyTransforms(0.5f);
                FixedRenderInterpolation.RestoreTransforms();
            }
            timer.Stop();
            return timer.Elapsed.TotalMilliseconds;
        }
        finally
        {
            FixedRenderInterpolation.Clear();
            foreach (var obj in objects)
                if (obj != null) Object.DestroyImmediate(obj);
        }
    }

    [Test]
    public void StoppingMovementLeavesNoStaleRenderWork()
    {
        var obj = new GameObject("moving scenery");
        try
        {
            FixedRenderInterpolation.Register(obj.transform);
            FixedRenderInterpolation.BeginTick();
            obj.transform.localPosition = Vector3.right * 10;
            FixedRenderInterpolation.EndTick();
            FixedRenderInterpolation.ApplyTransforms(0.5f);
            Assert.That(obj.transform.localPosition.x, Is.EqualTo(5));
            FixedRenderInterpolation.RestoreTransforms();
            FixedRenderInterpolation.BeginTick();
            FixedRenderInterpolation.EndTick();
            obj.transform.hasChanged = false;
            FixedRenderInterpolation.ApplyTransforms(0.5f);
            FixedRenderInterpolation.RestoreTransforms();
            Assert.That(obj.transform.hasChanged, Is.False);
            Assert.That(obj.transform.localPosition.x, Is.EqualTo(10));
        }
        finally { Object.DestroyImmediate(obj); }
    }

    [SetUp]
    public void SetUp() => FixedRenderInterpolation.Clear();

    [TearDown]
    public void TearDown() => FixedRenderInterpolation.Clear();

    [Test]
    public void PoseBlendsCompletedTicksWithoutChangingPhysicsNodes()
    {
        var node = new ModelNode(new Vector3d(0, 0, 0));
        FixedRenderInterpolation.Register(node);
        FixedRenderInterpolation.BeginTick();
        node.Start.X = 10;
        FixedRenderInterpolation.EndTick();

        Assert.That(FixedRenderInterpolation.Position(node, 0.25f).x, Is.EqualTo(2.5f));
        Assert.That(node.Start.X, Is.EqualTo(10));
        Assert.That(node.End.X, Is.EqualTo(0));

        FixedRenderInterpolation.BeginTick();
        node.Start.X = 20;
        FixedRenderInterpolation.EndTick();
        Assert.That(FixedRenderInterpolation.Position(node, 0.5f).x, Is.EqualTo(15));
    }

    [Test]
    public void TeleportDuringTickSnapsInsteadOfBlendingAcrossLevel()
    {
        var node = new ModelNode(new Vector3d());
        FixedRenderInterpolation.Register(node);
        FixedRenderInterpolation.BeginTick();
        node.Start.X = 1000;
        FixedRenderInterpolation.Invalidate(node);
        FixedRenderInterpolation.EndTick();
        Assert.That(FixedRenderInterpolation.Position(node, 0.25f).x, Is.EqualTo(1000));
    }

    [Test]
    public void ResetOutsideTickImmediatelyUsesLivePose()
    {
        var node = new ModelNode(new Vector3d());
        FixedRenderInterpolation.Register(node);
        FixedRenderInterpolation.BeginTick();
        node.Start.X = 10;
        FixedRenderInterpolation.EndTick();
        node.Start.X = 200;
        FixedRenderInterpolation.ResetHistory();
        Assert.That(FixedRenderInterpolation.Position(node, 0.5f).x, Is.EqualTo(200));
    }

    [Test]
    public void RenderTransformsRestoreAuthoritativeValuesIncludingNestedLayers()
    {
        var layer = new GameObject("layer");
        var child = new GameObject("moving visual");
        try
        {
            child.transform.SetParent(layer.transform, false);
            FixedRenderInterpolation.Register(layer.transform);
            FixedRenderInterpolation.Register(child.transform);
            FixedRenderInterpolation.BeginTick();
            layer.transform.localPosition = new Vector3(0, 0, 100);
            child.transform.localPosition = new Vector3(20, 0, 0);
            FixedRenderInterpolation.EndTick();
            FixedRenderInterpolation.ApplyTransforms(0.5f);
            Assert.That(layer.transform.localPosition.z, Is.EqualTo(50));
            Assert.That(child.transform.localPosition.x, Is.EqualTo(10));
            FixedRenderInterpolation.RestoreTransforms();
            Assert.That(layer.transform.localPosition.z, Is.EqualTo(100));
            Assert.That(child.transform.localPosition.x, Is.EqualTo(20));
        }
        finally
        {
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(layer);
        }
    }

    [Test]
    public void RenderDoesNotOverwriteTransformChangedOutsideSimulation()
    {
        var obj = new GameObject("tweened visual");
        try
        {
            FixedRenderInterpolation.Register(obj.transform);
            FixedRenderInterpolation.BeginTick();
            obj.transform.localPosition = Vector3.right * 10;
            FixedRenderInterpolation.EndTick();
            obj.transform.localPosition = Vector3.right * 30;
            FixedRenderInterpolation.ApplyTransforms(0.5f);
            Assert.That(obj.transform.localPosition.x, Is.EqualTo(30));
            FixedRenderInterpolation.RestoreTransforms();
            Assert.That(obj.transform.localPosition.x, Is.EqualTo(30));
        }
        finally { Object.DestroyImmediate(obj); }
    }

    [TestCase(-0.5, 0f)]
    [TestCase(0.005, 0.3f)]
    [TestCase(1.0, 1f)]
    public void BlendFractionIsClamped(double elapsed, float expected)
    {
        Assert.That(FixedRenderInterpolation.Fraction(elapsed, 1f / 60), Is.EqualTo(expected).Within(0.00001f));
    }

    [TestCase(false, 5f)]
    [TestCase(true, 10f)]
    public void ContinuousModelTranslationBlendsButTeleportSnaps(bool snapRendering, float expected)
    {
        var model = new Nekki.Vector.Core.Models.ModelObject(new System.Collections.Generic.List<string>());
        try
        {
            var node = new ModelNode(new Vector3d()) { Name = "NPivot" };
            model.NodesAll.Add(node);
            FixedRenderInterpolation.Register(node);
            FixedRenderInterpolation.BeginTick();
            model.Position(new Vector3d(10, 0, 0), snapRendering: snapRendering);
            FixedRenderInterpolation.EndTick();
            Assert.That(FixedRenderInterpolation.Position(node, 0.5f).x, Is.EqualTo(expected));
        }
        finally { Object.DestroyImmediate(model._Container); }
    }

    [Test]
    public void ResetDuringTickSnapsAllSnapshots()
    {
        var node = new ModelNode(new Vector3d());
        FixedRenderInterpolation.Register(node);
        FixedRenderInterpolation.BeginTick();
        node.Start.X = 500;
        FixedRenderInterpolation.ResetHistory();
        FixedRenderInterpolation.EndTick();
        Assert.That(FixedRenderInterpolation.Position(node, 0.1f).x, Is.EqualTo(500));
    }

    [Test]
    public void NodeSpawnedMidTickStartsAtItsSpawnPose()
    {
        FixedRenderInterpolation.BeginTick();
        var node = new ModelNode(new Vector3d(75, 20, 0));
        FixedRenderInterpolation.Register(node);
        FixedRenderInterpolation.EndTick();
        Assert.That(FixedRenderInterpolation.Position(node, 0f), Is.EqualTo(new Vector3(75, 20, 0)));
    }

    [TestCase(30)]
    [TestCase(60)]
    [TestCase(120)]
    [TestCase(144)]
    [TestCase(240)]
    public void RenderRateDoesNotChangeSimulationAndMotionStaysSmooth(int renderRate)
    {
        const float step = 1f / 60;
        var node = new ModelNode(new Vector3d());
        FixedRenderInterpolation.Register(node);
        int ticks = 0;
        for (int frame = 1; frame <= renderRate; frame++)
        {
            double now = (double)frame / renderRate;
            while ((ticks + 1) * (double)step <= now)
            {
                FixedRenderInterpolation.BeginTick();
                node.Start.X = ++ticks;
                FixedRenderInterpolation.EndTick();
            }
            float alpha = FixedRenderInterpolation.Fraction(now - ticks * (double)step, step);
            float rendered = FixedRenderInterpolation.Position(node, alpha).x;
            Assert.That(rendered, Is.EqualTo(System.Math.Max(0, now / step - 1)).Within(0.0001),
                "Frame " + frame + " at " + renderRate + " FPS");
            Assert.That(node.Start.X, Is.EqualTo(ticks), "Rendering must not advance simulation");
        }
    }
}
