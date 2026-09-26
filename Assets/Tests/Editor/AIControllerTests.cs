using NUnit.Framework;
using UnityEngine;

public class AIControllerTests
{
    [TestCase(1, 1100f)]
    [TestCase(-1, 900f)]
    public void ApplyFacingOffset_KeepsProbeAnchoredToWorldPosition(int sign, float expectedX)
    {
        Vector2 worldPosition = new Vector2(1000f, 50f);
        Vector2 localOffset = new Vector2(100f, -20f);

        Vector2 result = AIController.ApplyFacingOffset(worldPosition, localOffset, sign);

        Assert.That(result, Is.EqualTo(new Vector2(expectedX, 30f)));
    }
}
