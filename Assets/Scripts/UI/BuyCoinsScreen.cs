using UnityEngine.Purchasing;

namespace UI
{
	[View("BuyCoinsScreen")]
	public class BuyCoinsScreen : Screen
	{
		public Button BackToLobbyButton = new Button();

		public Button Buy25KButton;

		public Button Buy15KButton;

		public Button Buy10KButton;

		public Button Buy5KButton;

		public Button GetFreeButton;

		private UserDataManager playerData;

		public BuyCoinsScreen(ScreenManager screenManager)
			: base(screenManager)
		{
			BackToLobbyButton.PressedAction = () =>
			{
				ScreenManager.ShowPrevious();
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			};
		}

		private void OnPurchase(PaymentManager.PaymentProductData product, PurchaseFailureReason failureReason, bool result, int coins)
		{
		}
	}
}
