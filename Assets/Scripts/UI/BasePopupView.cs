using UnityEngine.Events;

namespace UI
{
	public class BasePopupView : ScreenViewWithCommonPayload<BasePopup>
	{
		public UnityEngine.UI.Button CreditsButton;

		public UnityEngine.UI.Button CancelButton;

		public UnityEngine.UI.Button BackgroundButton;

		public override void Init(BasePopup basePopup)
		{
			CreditsButton.onClick.AddListener(new UnityAction(basePopup.CreditsButton.PressedAction));
            CancelButton.onClick.AddListener(new UnityAction(basePopup.CancelButton.PressedAction));
            BackgroundButton.onClick.AddListener(new UnityAction(basePopup.BackgroundButton.PressedAction));
        }

        public override void PreShow(CommonPayloadData payload)
		{

		}
	}
}
