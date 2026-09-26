using NUnit.Framework;

public class GameplayClockTests
{
    [TestCase(0f, 0)]
    [TestCase(0.1f, 6)]
    [TestCase(0.5f, 30)]
    [TestCase(1f, 60)]
    [TestCase(2f, 120)]
    public void ScaleControlsSimulationRate(float scale, int expectedTicks)
    {
        var clock = new GameplayClock();
        int ticks = 0;
        for (int i = 0; i < 60; i++)
            ticks += clock.Advance(1.0 / 60, scale, 1.0 / 60);
        Assert.That(ticks, Is.EqualTo(expectedTicks));
    }

    [Test]
    public void StopPreservesPresentationAndResumeHasNoCatchUp()
    {
        var clock = new GameplayClock();
        Assert.That(clock.Advance(0.25, 1, 1), Is.Zero);
        Assert.That(clock.Alpha, Is.EqualTo(0.25f));
        Assert.That(clock.Advance(100, 0, 1), Is.Zero);
        Assert.That(clock.Alpha, Is.EqualTo(0.25f));
        Assert.That(clock.Advance(0.75, 1, 1), Is.EqualTo(1));
        Assert.That(clock.Alpha, Is.Zero);
    }

    [Test]
    public void SpeedChangesKeepFractionalProgress()
    {
        var clock = new GameplayClock();
        clock.Advance(0.5, 0.5f, 1);
        Assert.That(clock.Advance(0.25, 2, 1), Is.Zero);
        Assert.That(clock.Alpha, Is.EqualTo(0.75f));
        Assert.That(clock.Advance(0.25, 1, 1), Is.EqualTo(1));
    }

    [Test]
    public void FractionalSpeedIsIndependentOfRenderRate()
    {
        foreach (int fps in new[] { 30, 60, 144, 240 })
        {
            var clock = new GameplayClock();
            int ticks = 0;
            for (int i = 0; i < fps * 10; i++)
                ticks += clock.Advance(1.0 / fps, 0.37f, 1.0 / 60);
            Assert.That(ticks, Is.EqualTo(222), "Render FPS: " + fps);
            Assert.That(clock.Alpha, Is.EqualTo(0f).Within(0.0001f));
        }
    }

    [Test]
    public void ResetDiscardsOldFraction()
    {
        var clock = new GameplayClock();
        clock.Advance(0.75, 1, 1);
        clock.Reset();
        Assert.That(clock.Alpha, Is.Zero);
        Assert.That(clock.Advance(0.5, 1, 1), Is.Zero);
    }
}
