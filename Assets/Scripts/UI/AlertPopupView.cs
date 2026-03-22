using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class AlertPopupView : ScreenViewWithAlertPayload<AlertPopup>
	{
		public UnityEngine.UI.Button QuitButton;

		public UnityEngine.UI.Button CancelButton;

		public Text Title;

		public Text Message;

		public Text ButtonOKText;

		public Text ButtonCancelText;

		public Action<bool> onResult;

		private BuyCoinsPopup _yesNoItemPopup;

		public override void Init(AlertPopup popup)
		{
			QuitButton.onClick.AddListener(() =>
			{
				onResult?.Invoke(true);
			});
            CancelButton.onClick.AddListener(() =>
            {
	            onResult?.Invoke(false);
            });
        }

		public override void PreShow(AlertPayloadData payload)
		{
			DebugUtils.Log("PreShow");
			Title.text = payload.Title;
			Message.text = payload.Message;
			ButtonOKText.text = payload.ButtonOKText;
			ButtonCancelText.text = payload.ButtonCancelText;
			QuitButton.gameObject.SetActive(payload.ButtonOKText != "");
			CancelButton.gameObject.SetActive(payload.ButtonCancelText != "");
			onResult = payload.OnClick;
		}

		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(QuitButton.gameObject);
		}

		public override void Back()
		{
		}
	}
}
