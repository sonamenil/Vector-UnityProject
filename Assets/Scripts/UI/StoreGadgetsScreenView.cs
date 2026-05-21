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

        public LayoutPosKeeper LayoutPosKeeper;

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
                StoreTricksScreenView.Buy(items[ScrollSnap.CurrentIndex], true, ScrollSnap.CurrentIndex);
            });

            EventUtil.AddListener(EventTypes.EQUIP_ITEM, args =>
            {
                if (GadgetIcon == null || GadgetIcon.gameObject == null)
                {
                    return;
                }
                GadgetIcon.gameObject.SetActive((bool)args.args[0]);
            });
            
            ScrollSnap.SnapEvent += i =>
            {
                EventSystem.current.SetSelectedGameObject(ScrollSnap.CurrentObject.GetComponent<HolderItem>().Button.gameObject);
            };
        }

        private void InsertDummies(int count, bool right = false)
        {
            for (int i = count; i > 0; i--)
            {
                var obj = Instantiate(Resources.Load<LayoutElement>("HolderItemDummy"), ContentParent.transform);
                if (right)
                {
                    LayoutPosKeeper.right.Add(obj);
                }
                else
                {
                    LayoutPosKeeper.left.Add(obj);
                }
            }
        }

        public override void PreShow(GadgetScreenPayloadData payload)
        {
            LayoutPosKeeper.Clear();
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

            StoreTricksScreenView.PutItemsIntoContent(ScrollSnap, StoreManager.Instance.GetItems(StoreItemType.Gadgets), StoreItemType.Gadgets, true, true);
            InsertDummies(10, true);
            GadgetIcon.gameObject.SetActive(GadgetUtils.HasAnyEquippedGadget());

			var gadgets = StoreManager.Instance.GetItems(StoreItemType.Gadgets);

			ScrollSnap.StartIndex = 0;
			ScrollSnap.EndIndex = gadgets.Count - 1;

            ScrollSnap._childOffset = 10;

			ScrollSnap.Recalculate();
			ScrollSnap.Snap(0, true);
        }

        public override void PostShow(GadgetScreenPayloadData payload)
        {
            LayoutPosKeeper.SetPositions();

            ScrollSnap.Recalculate();
            ScrollSnap.Snap(0, true);
        }
        
        public override void SetSelectedGO()
        {
            EventSystem.current.SetSelectedGameObject(ScrollSnap.CurrentObject.GetComponent<HolderItem>().Button.gameObject);
        }
        
        public override void OnEnable()
        {
            base.OnEnable();
            ScrollSnap.enabled = true;
            
            actions.UI.XButton.performed += _ =>
            {
                var holder = ScrollSnap.CurrentObject.GetComponent<HolderItem>();
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
