using Nekki.Vector.Core.Location;
using NUnit.Framework;
using UnityEngine;

public class TrapezoidRunnerTests
{
    [TestCase(4f, 4f, 4f, 2f)]
    [TestCase(6f, 2f, 6f, 3f)]
    [TestCase(2f, 6f, 6f, -1f)]
    public void Generate_CreatesColliderCoveringLocalBounds(
        float height,
        float height1,
        float expectedHeight,
        float expectedOffsetY)
    {
        var runner = new TrapezoidRunner("test", 1, 10f, 20f, 8f, height, height1, true);

        runner.Generate();

        try
        {
            var collider = runner.UnityObject.GetComponent<BoxCollider2D>();
            Assert.That(collider, Is.Not.Null);
            Assert.That(collider.size, Is.EqualTo(new Vector2(8f, expectedHeight)));
            Assert.That(collider.offset, Is.EqualTo(new Vector2(4f, expectedOffsetY)));
        }
        finally
        {
            Object.DestroyImmediate(runner.UnityObject);
        }
    }

    [Test]
    public void GenerateExisting_ReusesAndConfiguresCollider()
    {
        var existingObject = new GameObject("existing trapezoid");
        var existingCollider = existingObject.AddComponent<BoxCollider2D>();
        var runner = new TrapezoidRunner("test", 1, 10f, 20f, 8f, 2f, 6f, true);

        runner.Generate(existingObject);

        try
        {
            Assert.That(existingObject.GetComponents<BoxCollider2D>(), Has.Length.EqualTo(1));
            Assert.That(existingObject.GetComponent<BoxCollider2D>(), Is.SameAs(existingCollider));
            Assert.That(existingCollider.size, Is.EqualTo(new Vector2(8f, 6f)));
            Assert.That(existingCollider.offset, Is.EqualTo(new Vector2(4f, -1f)));
        }
        finally
        {
            Object.DestroyImmediate(existingObject);
        }
    }
}
