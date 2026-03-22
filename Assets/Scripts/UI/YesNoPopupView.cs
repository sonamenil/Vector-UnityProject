using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
	public class YesNoPopupView : ScreenViewWithCommonPayload<YesNoPopup>
	{
		public UnityEngine.UI.Button YesButton;

		public UnityEngine.UI.Button NoButton;

		public UnityEngine.UI.Button BackgroundButton;

		public override void Init(YesNoPopup yesNoPopup)
		{
            YesButton.onClick.AddListener(new UnityAction(yesNoPopup.YesButton.PressedAction));
            NoButton.onClick.AddListener(new UnityAction(yesNoPopup.NoButton.PressedAction));
            BackgroundButton.onClick.AddListener(new UnityAction(yesNoPopup.BackgroundButton.PressedAction));
        }

		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(YesButton.gameObject);
		}

		public override void PreShow(CommonPayloadData payload)
		{
		}
	}
}
