using UnityEngine;

public class CursorVisibilityController : MonoBehaviour
{
    private void OnEnable()
    {
        if (GamepadController.Instance != null)
        {
            GamepadController.Instance.OnGamepadConnected += HideMouse;
            GamepadController.Instance.OnGamepadDisconnected += ShowMouse;

            // Set correct state on startup
            if (GamepadController.Instance.IsGamepadConnected)
                HideMouse();
            else
                ShowMouse();
        }
    }

    private void OnDisable()
    {
        if (GamepadController.Instance != null)
        {
            GamepadController.Instance.OnGamepadConnected -= HideMouse;
            GamepadController.Instance.OnGamepadDisconnected -= ShowMouse;
        }
    }

    private void HideMouse()
    {
        Cursor.visible = false;
    }

    private void ShowMouse()
    {
        Cursor.visible = true;
    }
}