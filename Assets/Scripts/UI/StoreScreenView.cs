using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class StoreScreenView : ScreenViewWithCommonPayload<StoreScreen>
	{
		public UnityEngine.UI.Button BackToLobbyButton;

		public UnityEngine.UI.Button GadgetsButton;

		public UnityEngine.UI.Button TricksButton;

		public UnityEngine.UI.Button GearButton;

		public UnityEngine.UI.Button CoinsButton;

		public UnityEngine.UI.Button BuyCoinsButton;

		public override void Init(StoreScreen screen)
		{
            BuyCoinsButton.gameObject.SetActive(false);
			CoinsButton.gameObject.SetActive(false);
            BackToLobbyButton.onClick.AddListener(new UnityEngine.Events.UnityAction(screen.BackToLobbyButton.PressedAction));
            GadgetsButton.onClick.AddListener(new UnityEngine.Events.UnityAction(screen.GadgetsButton.PressedAction));
            TricksButton.onClick.AddListener(new UnityEngine.Events.UnityAction(screen.TricksButton.PressedAction));
            GearButton.onClick.AddListener(new UnityEngine.Events.UnityAction(screen.GearButton.PressedAction));
            CoinsButton.onClick.AddListener(() =>
            {
				Game.Instance.ScreenManager.Show<BuyCoinsScreen>(true, true);
            });
            BuyCoinsButton.onClick.AddListener(() =>
            {
                Game.Instance.ScreenManager.Show<BuyCoinsScreen>(true, true);
            });
        }

		public override void PreShow(CommonPayloadData payload)
		{
		}

		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(GadgetsButton.gameObject);
		}

		public override void Back()
		{
			BackToLobbyButton.onClick.Invoke();
		}
	}
}
