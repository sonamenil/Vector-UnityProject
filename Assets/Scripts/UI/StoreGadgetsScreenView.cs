using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class StoreGadgetsScreenView : ScreenView<StoreGadgetsScreen, GadgetScreenPayloadData>
    {
        public UnityEngine.UI.Button BackToLobbyButton;

        public UnityEngine.UI.Button BuyCoinsButton;

        public UnityEngine.UI.Button BuyButton;

        public UnityEngine.UI.Button ShowInfoButton;

        public GameObject ContentParent;

        public GameObject GadgetsArea;

        public GameObject GadgetIcon;

        public ScrollSnap ScrollSnap;

        private StoreGadgetsScreen _storeGadgetsScreen;

        public override void Init(StoreGadgetsScreen screen)
        {
            BuyCoinsButton.gameObject.SetActive(false);

            _storeGadgetsScreen = screen;
            BuyCoinsButton.onClick.AddListener(() =>
            {
                Game.Instance.ScreenManager.Show<BuyCoinsScreen>(true, true);
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            });
            BuyButton.onClick.AddListener(() =>
            {
                var items = StoreManager.Instance.GetItems(StoreItemType.Gadgets);
                StoreTricksScreenView.Buy(items[ScrollSnap.CurrentIndex - 10], true, ScrollSnap.CurrentIndex - 10);
            });

            EventUtil.AddListener(EventTypes.EQUIP_ITEM, (EventArgs args) =>
            {
                if (GadgetIcon == null || GadgetIcon.gameObject == null)
                {
                    return;
                }
                GadgetIcon.gameObject.SetActive((bool)args.args[0]);
            });
            
            ScrollSnap.SnapEvent += (i) =>
            {
                EventSystem.current.SetSelectedGameObject(ScrollSnap._content.GetChild(i).GetComponent<HolderItem>().Button.gameObject);
            };
        }

        private void InsertDummies(int count)
        {
            for (int i = count; i > 0; i--)
            {
                var obj = Instantiate(Resources.Load("HolderItemDummy"), ContentParent.transform);
            }
        }

        public override void PreShow(GadgetScreenPayloadData payload)
        {
            BackToLobbyButton.onClick.RemoveAllListeners();
            if (!payload.BackToPrevious)
            {
                BackToLobbyButton.onClick.AddListener(() =>
                {
                    Game.Instance.ScreenManager.Show<StoreScreen>(true, false);
                    SoundsManager.Instance.PlaySounds(SoundType.ui_click);
                });
            }
            else
            {
                BackToLobbyButton.onClick.AddListener(() =>
                {
                    Game.Instance.ScreenManager.ShowPrevious();
                    SoundsManager.Instance.PlaySounds(SoundType.ui_click);
                });
            }

            foreach (Transform child in ContentParent.transform)
            {
                Destroy(child.gameObject);
            }

            InsertDummies(10);
            ScrollSnap.StartIndex = 10;
            ScrollSnap.EndIndex = 10;

            StoreTricksScreenView.PutItemsIntoContent(ScrollSnap, StoreManager.Instance.GetItems(StoreItemType.Gadgets), StoreItemType.Gadgets, true, true);
            InsertDummies(10);
            GadgetIcon.gameObject.SetActive(UserDataManager.Instance.ShopData.IsEquippedGadget());
        }

        public override void PostShow(GadgetScreenPayloadData payload)
        {
            ScrollSnap.Snap(10, true);
        }
        
        public override void SetSelectedGO()
        {
            EventSystem.current.SetSelectedGameObject(ScrollSnap._content.GetChild(ScrollSnap.CurrentIndex).GetComponent<HolderItem>().Button.gameObject);
        }
        
        public override void OnEnable()
        {
            base.OnEnable();
            ScrollSnap.enabled = true;
            
            actions.UI.XButton.performed += _ =>
            {
                var holder = ScrollSnap._content.GetChild(ScrollSnap.CurrentIndex).GetComponent<HolderItem>();
                if (holder.EquipButton.gameObject.activeSelf)
                {
                    holder.EquipButton.onClick?.Invoke();
                }
            };
        }

        public override void OnDisable()
        {
            base.OnDisable();
            ScrollSnap.enabled = false;
        }

        public override void Back()
        {
            BackToLobbyButton.onClick.Invoke();
        }
    }
}
