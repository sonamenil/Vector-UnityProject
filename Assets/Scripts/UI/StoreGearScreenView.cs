using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class StoreGearScreenView : ScreenViewWithCommonPayload<StoreGearScreen>
	{
		public UnityEngine.UI.Button BackToLobbyButton;

		public UnityEngine.UI.Button BuyCoinsButton;

		public UnityEngine.UI.Button BuyButton;

		public UnityEngine.UI.Button ShowInfoButton;

		public GameObject ContentParent;

		public ScrollSnap ScrollSnap;

        public LayoutPosKeeper LayoutPosKeeper;

        private StoreGearScreen _storeGearScreen;

		public override void Init(StoreGearScreen screen)
		{
            BuyCoinsButton.gameObject.SetActive(false);
            _storeGearScreen = screen;
			BackToLobbyButton.onClick.AddListener(new UnityAction(screen.BackToLobbyButton.PressedAction));
            BuyButton.onClick.AddListener(() =>
            {
                var items = StoreManager.Instance.GetItems(StoreItemType.Gear);
                var item = items[ScrollSnap.CurrentIndex];
                if (!UserDataManager.Instance.ShopData.IsBought(item.Id))
                {
                    StoreTricksScreenView.Buy(item, false, ScrollSnap.CurrentIndex);
                }
            });
            
            ScrollSnap.SnapEvent += i =>
            {
	            EventSystem.current.SetSelectedGameObject(ScrollSnap.CurrentObject.GetComponent<HolderItem>().Button.gameObject);
            };
        }

		private void InsertDummies(int count, bool right = false)
		{
            if (count > 0)
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
        }

		public override void PreShow(CommonPayloadData payload)
		{
			LayoutPosKeeper.Clear();
            foreach (Transform child in ContentParent.transform)
            {
                Destroy(child.gameObject);
            }
            var items = StoreManager.Instance.GetItems(StoreItemType.Gear);
            InsertDummies(10);
            StoreTricksScreenView.PutItemsIntoContent(ScrollSnap, items, StoreItemType.Gear, true);
            InsertDummies(10, true);

            ScrollSnap.StartIndex = 0;
            ScrollSnap.EndIndex = items.Count - 1;

			ScrollSnap._childOffset = 10;
        }

		public override void PostShow(CommonPayloadData payload)
		{
			LayoutPosKeeper.SetPositions();

			ScrollSnap.Recalculate();

			ScrollSnap.Snap(UserDataManager.RuntimeInfo.LastSelectedGear, true);
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
			BackToLobbyButton.onClick?.Invoke();
		}
	}
}
