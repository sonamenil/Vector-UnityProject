using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class BuyCoinsPopupView : ScreenView<BuyCoinsPopup, BuyCoinsPayloadData>
	{
		public UnityEngine.UI.Button BuyButton;

		public UnityEngine.UI.Button CancelButton;

		public UnityEngine.UI.Button BackgroundButton;

		public UnityEngine.UI.Button GetFreeButton;

		public Text Caption;

		public Image Icon;

		public Text Price;

		public Text Description;

		private BuyCoinsPopup _yesNoItemPopup;

		public override void Init(BuyCoinsPopup yesNoItemPopup)
		{
			BuyButton.gameObject.SetActive(false);
			GetFreeButton.gameObject.SetActive(false);
			_yesNoItemPopup = yesNoItemPopup;
			CancelButton.onClick.AddListener(new UnityAction(yesNoItemPopup.CancelButton.PressedAction));
            BackgroundButton.onClick.AddListener(new UnityAction(yesNoItemPopup.BackgroundButton.PressedAction));
        }

        public override void PreShow(BuyCoinsPayloadData payload)
		{
			Price.text = payload.Price.ToString();
		}

		public override void PostShow(BuyCoinsPayloadData payload)
		{
		}
		
		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(CancelButton.gameObject);
		}

		public override void Back()
		{
		}
	}
}
