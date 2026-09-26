using System;

/// <summary>Accumulates scaled gameplay time while keeping simulation steps fixed.</summary>
public sealed class GameplayClock
{
    private double _pendingTicks;

    public float Alpha => (float)_pendingTicks;

    public int Advance(double deltaTime, float scale, double step)
    {
        if (scale <= 0 || deltaTime <= 0 || step <= 0)
            return 0;

        _pendingTicks += deltaTime * scale / step;
        // Avoid losing a tick to floating-point rounding at an exact boundary.
        int ticks = (int)Math.Floor(_pendingTicks + 1e-9);
        _pendingTicks = Math.Max(0, _pendingTicks - ticks);
        return ticks;
    }

    public void Reset() => _pendingTicks = 0;
}
