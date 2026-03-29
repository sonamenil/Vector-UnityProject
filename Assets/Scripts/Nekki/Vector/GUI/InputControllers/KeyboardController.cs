using UnityEngine;
using UnityEngine.InputSystem;

namespace Nekki.Vector.GUI.InputControllers
{
	public class KeyboardController : MonoBehaviour
	{
		public KeyEvent OnKeyDown = new KeyEvent();
		public KeyEvent OnKeyUp = new KeyEvent();

		public bool this[Key key]
		{
			get
			{
				var keyboard = Keyboard.current;
				return keyboard != null && keyboard[key].isPressed;
			}
		}

		public static void SetEnabledAll(bool value)
		{
			foreach (var controller in FindObjectsByType<KeyboardController>(FindObjectsSortMode.None))
			{
				controller.enabled = value;
			}
		}
		

		// private void Update()
		// {
		// 	HandleKeyboard();
		// 	
		// 	
		// }

		private void HandleKeyboard()
		{
			var keyboard = Keyboard.current;
			if (keyboard == null)
				return;

			foreach (var keyControl in keyboard.allKeys)
			{
				if (keyControl.wasPressedThisFrame)
					OnKeyDown.Invoke(keyControl.keyCode);

				if (keyControl.wasReleasedThisFrame)
					OnKeyUp.Invoke(keyControl.keyCode);
			}
		}
	}
}
