using UnityEngine;

public class SwipeGestureRecognizer
{
	public SwipeDirection Direction
	{
		get;
	}

	public SwipeGestureRecognizer(Vector2 start, Vector2 end)
	{
        Direction = GetSwipeDirection(start, end);
	}

	private static SwipeDirection GetSwipeDirection(Vector2 start, Vector2 end)
	{
        float num = end.x - start.x;
        float num2 = end.y - start.y;
        if (num2 > 0f && Mathf.Abs(num) < Mathf.Abs(num2))
        {
            return SwipeDirection.Up;
        }
        if (num < 0f && Mathf.Abs(num) > Mathf.Abs(num2))
        {
            return SwipeDirection.Left;
        }
        if (num > 0f && Mathf.Abs(num) > Mathf.Abs(num2))
        {
            return SwipeDirection.Right;
        }
        if (num2 < 0f && Mathf.Abs(num) < Mathf.Abs(num2))
        {
            return SwipeDirection.Bottom;
        }
        return SwipeDirection.None;
    }
}
