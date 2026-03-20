using UnityEngine.UI;

namespace UI
{
	public class BasePopupView : ScreenViewWithCommonPayload<BasePopup>
	{
		public UnityEngine.UI.Button CreditsButton;

		public UnityEngine.UI.Button CancelButton;

		public UnityEngine.UI.Button BackgroundButton;

		public override void Init(BasePopup basePopup)
		{
			CreditsButton.onClick.AddListener(new UnityEngine.Events.UnityAction(basePopup.CreditsButton.PressedAction));
            CancelButton.onClick.AddListener(new UnityEngine.Events.UnityAction(basePopup.CancelButton.PressedAction));
            BackgroundButton.onClick.AddListener(new UnityEngine.Events.UnityAction(basePopup.BackgroundButton.PressedAction));
        }

        public override void PreShow(CommonPayloadData payload)
		{

		}
	}
}
