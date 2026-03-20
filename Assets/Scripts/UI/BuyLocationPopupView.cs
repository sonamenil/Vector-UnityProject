using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class BuyLocationPopupView : ScreenView<BuyLocationPopup, BuyLocationPayloadData>
	{
		public Text Caption;

		public Text Price;

		public UnityEngine.UI.Button Buy;

		public UnityEngine.UI.Button Cancel;

		public override void Init(BuyLocationPopup screen)
		{
			Cancel.onClick.AddListener(() =>
			{
				Game.Instance.ScreenManager.ClosePopup();
			});
		}

		public override void PreShow(BuyLocationPayloadData payload)
		{
			Caption.text = payload.LocationId;
			Price.text = string.Format("{0}", payload.Price);

			Buy.onClick.AddListener(() =>
			{
				if (UserDataManager.Instance.MainData.GetCoins() < payload.Price)
				{
					Game.Instance.ScreenManager.ClosePopup();
                    Game.Instance.ScreenManager.Popup<BuyCoinsPopup, BuyCoinsPayloadData>(new BuyCoinsPayloadData(payload.Price));
                }
				else
				{
					UserDataManager.Instance.MainData.AddCoins(-payload.Price);
					UserDataManager.Instance.ShopData.Add(payload.LocationId, 1, true, false);
					Game.Instance.ScreenManager.ClosePopup();
					Game.Instance.ScreenManager.Refresh();
				}
			});
		}

		public override void PostShow(BuyLocationPayloadData payload)
		{
		}
		
		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(Buy.gameObject);
		}

		public override void Back()
		{
		}
	}
}
