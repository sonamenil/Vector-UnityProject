using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public abstract class UnlockPopupView<T, TPayload> : ScreenView<T, TPayload> where T : Screen
	{
		public UnityEngine.UI.Button BuyButton;

		public UnityEngine.UI.Button CancelButton;

		public UnityEngine.UI.Button BackgroundButton;

		public Text Caption;

		public Image Icon;

		public Text Price;

		public Text Stars;

		public Text Description;

		public override void PreShow(TPayload payload)
		{
		}

		public override void PostShow(TPayload payload)
		{
		}
		
		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(BuyButton.gameObject);
		}
	}
}
