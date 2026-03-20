using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnlockStoryPopupView : UnlockPopupView<UnlockStoryPopup, UnlockStoryPopupPayloadData>
    {
        public Text LocationName;

        public Text Title;

        public override void Init(UnlockStoryPopup yesNoLocationPopup)
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

        public override void PreShow(UnlockStoryPopupPayloadData payload)
        {
            Title.text = payload.Caption;
            Caption.text = payload.Caption;
            LocationName.text = payload.StoryName;
            Price.text = payload.Price.ToString();
            Stars.text = payload.StarsNumber.ToString();
            BuyButton.onClick.RemoveAllListeners();
            BuyButton.onClick.AddListener(() =>
            {
                Game.Instance.ScreenManager.ClosePopup();
                if (UserDataManager.Instance.MainData.GetCoins() < payload.Price)
                {
                    Game.Instance.ScreenManager.Popup<BuyCoinsPopup, BuyCoinsPayloadData>(new BuyCoinsPayloadData(payload.Price));
                    return;
                }
                UserDataManager.Instance.MainData.AddCoins(-payload.Price);
                payload.Action?.Invoke();
                UserDataManager.Instance.SaveUserDate();
                Game.Instance.ScreenManager.Refresh();
            });
        }

        public override void Back()
        {
        }
    }
}
