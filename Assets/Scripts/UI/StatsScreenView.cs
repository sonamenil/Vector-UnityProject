using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class StatsScreenView : ScreenViewWithCommonPayload<StatsScreen>
	{
		public Scrollbar Scroll;
		
		public UnityEngine.UI.Button BackToLobbyButton;

		public UnityEngine.UI.Button BuyCoinsButton;

		public Text TotalRunningDistance;

		public Text JumpsCount;

		public Text SlidesCount;

		public Text BonusCollected;

		public Text CoinsCollected;

		public Text TricksPerformed;

		public Text TracksPassed;

		public Text TracksPassedWith3Stars;

		public Text Death;

		public Text DeathByHunter;

		public Text HuntersKilled;

		public Text GlassBroken;

		public override void Init(StatsScreen screen)
		{
			BackToLobbyButton.onClick.AddListener(new UnityAction(screen.BackToLobbyButton.PressedAction));
			BuyCoinsButton.onClick.AddListener(new UnityAction(screen.BuyCoinsButton.PressedAction));
		}

		public override void PreShow(CommonPayloadData payload)
		{
            BuyCoinsButton.gameObject.SetActive(false);
            var stats = UserDataManager.Instance.Stats;
			TotalRunningDistance.text = string.Format("{0:F}", stats.TotalRunningDistance);
			JumpsCount.text = stats.JumpsCount.ToString();
			SlidesCount.text = stats.SlidesCount.ToString();
			BonusCollected.text = stats.BonusCollected.ToString();
			CoinsCollected.text = stats.CoinsCollected.ToString();
			TricksPerformed.text = stats.TricksPerformed.ToString();
			TracksPassed.text = UserDataManager.Instance.GameStats.GetAllTrackCompleted(0).ToString();
			TracksPassedWith3Stars.text = UserDataManager.Instance.GameStats.GetAllTrackCompleted(3).ToString();
			Death.text = stats.Death.ToString();
			DeathByHunter.text = stats.DeathByHunter.ToString();
			HuntersKilled.text = stats.HuntersKilled.ToString();
			GlassBroken.text = stats.GlassBroken.ToString();
		}

		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(Scroll.gameObject);
		}

		public override void Back()
		{
			BackToLobbyButton.onClick?.Invoke();
		}
	}
}
