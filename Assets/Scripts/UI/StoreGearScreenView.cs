using UnityEngine;
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

		private StoreGearScreen _storeGearScreen;

		public override void Init(StoreGearScreen screen)
		{
            BuyCoinsButton.gameObject.SetActive(false);
            _storeGearScreen = screen;
			BackToLobbyButton.onClick.AddListener(new UnityEngine.Events.UnityAction(screen.BackToLobbyButton.PressedAction));
            BuyButton.onClick.AddListener(() =>
            {
                var items = StoreManager.Instance.GetItems(StoreItemType.Gear);
                var item = items[ScrollSnap.CurrentIndex - 10];
                if (!UserDataManager.Instance.ShopData.IsBought(item.Id))
                {
                    StoreTricksScreenView.Buy(item, false, ScrollSnap.CurrentIndex - 10);
                }
            });
            
            ScrollSnap.SnapEvent += (i) =>
            {
	            EventSystem.current.SetSelectedGameObject(ScrollSnap._content.GetChild(i).GetComponent<HolderItem>().Button.gameObject);
            };
        }

		private void InsertDummies(int count)
		{
            if (count > 0)
            {
                for (int i = count; i > 0; i--)
                {
                    Object.Instantiate(Resources.Load<ScrollSnapItem>("HolderItemDummy"), ContentParent.transform);
                }
            }
        }

		public override void PreShow(CommonPayloadData payload)
		{
            foreach (Transform child in ContentParent.transform)
            {
                Destroy(child.gameObject);
            }
            var items = StoreManager.Instance.GetItems(StoreItemType.Gear);
            InsertDummies(10);
            ScrollSnap.StartIndex = 10;
            ScrollSnap.EndIndex = items.Count + 9;
            StoreTricksScreenView.PutItemsIntoContent(ScrollSnap, items, StoreItemType.Gear, true, false);
            InsertDummies(10);
        }

		public override void PostShow(CommonPayloadData payload)
		{
			ScrollSnap.Snap(UserDataManager.RuntimeInfo.LastSelectedGear + 10, true);
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
			BackToLobbyButton.onClick?.Invoke();
		}
	}
}
