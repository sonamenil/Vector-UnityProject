using System;
using System.Collections;
using System.Linq;
using Nekki.Vector.Core.Location;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class BuyItemPopupView : ScreenView<BuyItemPopup, BuyItemPayloadData>
    {
        public UnityEngine.UI.Button BuyButton;

        public UnityEngine.UI.Button CancelButton;

        public UnityEngine.UI.Button BackgroundButton;

        public Text Caption;

        public Image Icon;

        public Text Price;

        public Text Description;

        public GameObject CounterHolder;

        public Text Counter;

        private BuyItemPopup _yesNoItemPopup;

        public override void Init(BuyItemPopup yesNoItemPopup)
        {
            _yesNoItemPopup = yesNoItemPopup;
            BuyButton.onClick.AddListener(new UnityAction(_yesNoItemPopup.BuyButton.PressedAction));
            CancelButton.onClick.AddListener(new UnityAction(_yesNoItemPopup.CancelButton.PressedAction));
            BackgroundButton.onClick.AddListener(new UnityAction(_yesNoItemPopup.BackgroundButton.PressedAction));
        }

        public override void PreShow(BuyItemPayloadData payload)
        {
            var items = StoreManager.Instance.GetItems(payload.ItemType);
            var item = items.Single(i => i.Id == payload.ItemId);
            Caption.text = LocalizationManager.Instance.GetTranslationByID(item.Id);
            Price.text = item.Price.ToString();
            Icon.sprite = payload.Sprite;
            Description.gameObject.SetActive(payload.BuyItemAdditionalPayloadData != null);
            CounterHolder.gameObject.SetActive(payload.BuyItemAdditionalPayloadData != null &&
                                               payload.BuyItemAdditionalPayloadData.MultiplePurchase);
            if (payload.BuyItemAdditionalPayloadData != null)
            {
                Description.text = payload.BuyItemAdditionalPayloadData.Description;
                Counter.text = string.Format("{0}", UserDataManager.Instance.ShopData.GetCount(item.Id));
            }

            var playerData = UserDataManager.Instance;
            BuyButton.onClick.RemoveAllListeners();
            BuyButton.onClick.AddListener(() =>
            {
                if (playerData.MainData.GetCoins() < item.Price)
                {
                    Game.Instance.ScreenManager.ClosePopup();
                    Game.Instance.ScreenManager.Popup<BuyCoinsPopup, BuyCoinsPayloadData>(
                        new BuyCoinsPayloadData(item.Price));
                    return;
                }

                playerData.MainData.AddCoins(-item.Price);
                playerData.ShopData.Add(payload.ItemId, 1, true, false);
                playerData.SaveUserDate();
                SoundsManager.Instance.PlaySounds(SoundType.cash_register);

                if (payload.ItemType == StoreItemType.Gear)
                {
                    UserDataManager.RuntimeInfo.LastSelectedGear = payload.Index;
                }
                else if (payload.ItemType == StoreItemType.Tricks)
                {
                    UserDataManager.RuntimeInfo.LastSelectedTrick = payload.Index;
                }

                // CoroutineRunner.Instance.Run(Wait(payload));


                if (payload.BuyItemAdditionalPayloadData == null ||
                    !payload.BuyItemAdditionalPayloadData.MultiplePurchase)
                {
                    _yesNoItemPopup.BuyButton.PressedAction?.Invoke();
                }
                else
                {
                    Counter.text = string.Format("{0}", UserDataManager.Instance.ShopData.GetCount(item.Id));
                    SetSelectedGO();
                }

                if (LevelMainController.current != null)
                    LevelMainController.current.RefreshTricks();
            });
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (Game.Instance.ScreenManager != null)
                Game.Instance.ScreenManager.Refresh();
        }

        IEnumerator Wait(BuyItemPayloadData payload)
        {
            Game.Instance.ScreenManager.Refresh();

            yield return null;
            yield return new WaitForEndOfFrame();

            if (payload.BuyItemAdditionalPayloadData == null || !payload.BuyItemAdditionalPayloadData.MultiplePurchase)
            {
                _yesNoItemPopup.BuyButton.PressedAction?.Invoke();
            }
            else
            {
                Game.Instance.ScreenManager.Popup<BuyItemPopup, BuyItemPayloadData>(payload);
            }

            if (LevelMainController.current != null)
                LevelMainController.current.RefreshTricks();
        }

        public override void PostShow(BuyItemPayloadData payload)
        {
        }

        public override void SetSelectedGO()
        {
            EventSystem.current.SetSelectedGameObject(BuyButton.gameObject);
        }

        public override void Back()
        {
        }
    }
}