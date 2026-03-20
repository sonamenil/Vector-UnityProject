using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ToggleHandler : MonoBehaviour
	{
		private UnityEngine.UI.Toggle _toggle;

		private ToggleGroup _toggleGroup;

		private void Awake()
		{
			_toggle = GetComponent<UnityEngine.UI.Toggle>();
		}

		public void OnToggle(bool isOn)
		{
			_toggle.interactable = _toggle.isOn == false;
		}
	}
}
