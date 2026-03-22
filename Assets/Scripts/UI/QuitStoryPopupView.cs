using System.Collections;
using UnityEngine.EventSystems;

namespace UI
{
	public class QuitStoryPopupView : ScreenViewWithCommonPayload<QuitStoryPopup>
	{
		public UnityEngine.UI.Button QuitButton;

		public UnityEngine.UI.Button CancelButton;

		private BuyCoinsPopup _yesNoItemPopup;

		public override void Init(QuitStoryPopup popup)
		{
			QuitButton.onClick.AddListener(() =>
			{
				Game.Instance.ScreenManager.FadeIn(() =>
				{
                    Game.Instance.ScreenManager.ClosePopup();
					LevelMainController.current.pauseRender = false;
					LevelMainController.current.ClearScene();
					Game.Instance.ScreenManager.Show<SelectStoryScreen>(false, false);
					Game.Instance.ScreenManager.FadeOut();
                    SoundsManager.Instance.PlaySounds(SoundType.ui_click);
					SoundsManager.Instance.PlayBackground(MusicType.menu);
                });
			});
			CancelButton.onClick.AddListener(() => {
				Game.Instance.ScreenManager.ClosePopup();
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			});
		}

		private IEnumerator Hide(ScreenManager screenManager)
		{
			screenManager.Show<EmptyScreen>(false, false);
			yield return null;

			screenManager.Show<SelectStoryScreen>(true, false);
			yield return null;

			yield break;
		}
		
		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(QuitButton.gameObject);
		}

		public override void PreShow(CommonPayloadData payload)
		{
		}
	}
}
