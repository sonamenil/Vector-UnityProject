using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
	public class ChooseModeScreenView : ScreenViewWithCommonPayload<ChooseModeScreen>
	{
		public UnityEngine.UI.Button BackToLobbyButton;

		public ModeItemView ClassicModeItemView;

		public ModeItemView HunterModeItemView;

		public UnityEngine.UI.Button BuyCoinsButton;

		public override void Init(ChooseModeScreen screen)
		{
            BuyCoinsButton.gameObject.SetActive(false);
            BackToLobbyButton.onClick.AddListener(new UnityAction(screen.BackToLobbyButton.PressedAction));
			ClassicModeItemView.Button.onClick.AddListener(() =>
			{
				UserDataManager.RuntimeInfo.IsHunterMode = false;
				UserDataManager.Instance.SetClassicMode();
				Game.Instance.ScreenManager.Show<SelectLocationScreen>(true, false);
			});
			HunterModeItemView.Button.onClick.AddListener(() =>
			{
				if (isHunterModeAvailible())
				{
					UserDataManager.RuntimeInfo.IsHunterMode = true;
					UserDataManager.Instance.SetHunterMode();
					Game.Instance.ScreenManager.Show<SelectLocationScreen>(true, false);
				}
				else
				{
					Game.Instance.ScreenManager.Popup<HunterUnlockPopup>();
				}
			});
			BuyCoinsButton.onClick.AddListener(() =>
			{
				Game.Instance.ScreenManager.Show<BuyCoinsScreen>(true, true);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			});
        }

		public override void PreShow(CommonPayloadData payload)
		{
			base.PreShow(payload);
			HunterModeItemView.Lock.SetActive(!isHunterModeAvailible());
		}

		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(ClassicModeItemView.gameObject);
		}

		public override void Back()
		{
			BackToLobbyButton.onClick?.Invoke();
		}

		private bool isHunterModeAvailible()
		{
			return UserDataManager.Instance.GameStats.GetLocationsStars(LocationModeType.Classic) >= LocationManager.Instance.locations["DOWNTOWN"][LocationModeType.Hunter].UnlockInfo.StarsLocationUnlockInfo.Stars;

        }
	}
}
