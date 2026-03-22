using System.Collections.Generic;
using Nekki.Vector.Core.Controllers;

public static class KeyMapping
{
    private static readonly IReadOnlyDictionary<SwipeDirection, Key> SwipeToKeyMapping = new Dictionary<SwipeDirection, Key>
    {
        {SwipeDirection.Up, Key.Up },
        {SwipeDirection.Bottom, Key.Down},
        {SwipeDirection.Left, Key.Left},
        {SwipeDirection.Right, Key.Right},
    };

	private static readonly IReadOnlyDictionary<UnityEngine.InputSystem.Key, Key> KeyboardToKeyMapping = new Dictionary<UnityEngine.InputSystem.Key, Key>
    {
        { UnityEngine.InputSystem.Key.UpArrow, Key.Up },
        { UnityEngine.InputSystem.Key.DownArrow, Key.Down },
        { UnityEngine.InputSystem.Key.LeftArrow, Key.Left },
        { UnityEngine.InputSystem.Key.RightArrow, Key.Right }
    };

    public static Key MapFromSwipe(SwipeDirection swipeDirection)
	{
        if (SwipeToKeyMapping.ContainsKey(swipeDirection))
        {
            return SwipeToKeyMapping[swipeDirection];
        }
        return Key.None;
    }

	public static Key MapFromKeycode(UnityEngine.InputSystem.Key keyCode)
    {

        if (KeyboardToKeyMapping.ContainsKey(keyCode))
        {
            return KeyboardToKeyMapping[keyCode];
        }
        return Key.None;
    }
}
