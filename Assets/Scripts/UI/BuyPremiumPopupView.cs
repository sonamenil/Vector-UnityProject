using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class BuyPremiumPopupView : ScreenViewWithCommonPayload<BuyPremiumPopup>
	{
		public UnityEngine.UI.Button UnlockButton;

		public UnityEngine.UI.Button CancelButton;

		public Image Image;

		public override void Init(BuyPremiumPopup popup)
		{
		}
		
		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(UnlockButton.gameObject);
		}

		public override void PreShow(CommonPayloadData payload)
		{
		}
	}
}
