using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class HunterUnlockPopupView : ScreenViewWithCommonPayload<HunterUnlockPopup>
	{
		public UnityEngine.UI.Button CancelButton;

		public UnityEngine.UI.Button BackgroundButton;

		public Text Coins;

		public override void Init(HunterUnlockPopup hunterUnlockPopup)
		{
			CancelButton.onClick.AddListener(() =>
			{
				Game.Instance.ScreenManager.ClosePopup();
			});
            BackgroundButton.onClick.AddListener(() =>
            {
                Game.Instance.ScreenManager.ClosePopup();
            });
        }

		public override void PreShow(CommonPayloadData payload)
		{
		}
		
		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(CancelButton.gameObject);
		}
	}
}
