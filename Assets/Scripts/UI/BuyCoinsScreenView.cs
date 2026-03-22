using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
	public class BuyCoinsScreenView : ScreenView<BuyCoinsScreen, CommonPayloadData>
	{
		public UnityEngine.UI.Button BackToLobbyButton;

		public UnityEngine.UI.Button Buy25KButton;

		public UnityEngine.UI.Button Buy15KButton;

		public UnityEngine.UI.Button Buy10KButton;

		public UnityEngine.UI.Button Buy5KButton;

		public UnityEngine.UI.Button GetFreeButton;

		public Text ButtonCoins25000_Text;

		public Text ButtonCoins10000_Text;

		public Text ButtonCoins5000_Text;

		public Text ButtonCoins15000_Text;

		private UserDataManager _playerData;

		public override void Init(BuyCoinsScreen screen)
		{
			BackToLobbyButton.onClick.AddListener(new UnityAction(screen.BackToLobbyButton.PressedAction));
		}

		public override void PreShow(CommonPayloadData payload)
		{
		}

		public override void PostShow(CommonPayloadData payload)
		{
		}
		
		public override void SetSelectedGO()
		{
		}

		public override void Back()
		{
		}
	}
}
