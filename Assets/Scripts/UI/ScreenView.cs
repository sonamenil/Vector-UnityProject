using UnityEngine;

namespace UI
{
	public abstract class ScreenView : MonoBehaviour
	{
		protected PlayerInputActions actions;

		private void Awake()
		{
			if (actions == null)
				actions =  new PlayerInputActions();
		}

		public virtual void OnEnable()
		{
			actions.Enable();
		}

		public virtual void OnDisable()
		{
			actions.Disable();
			actions.Dispose();
			actions = new PlayerInputActions();
		}

		public abstract void SetSelectedGO();

		public abstract void Back();
	}
	public abstract class ScreenView<TScreen, TPayload> : ScreenView where TScreen : Screen
	{
		public abstract void Init(TScreen screen);

		public abstract void PreShow(TPayload payload);

		public abstract void PostShow(TPayload payload);
	}
}
