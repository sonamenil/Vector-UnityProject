using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace UI
{
	public class OptionsScreenView : ScreenViewWithCommonPayload<OptionsScreen>
	{
		public UnityEngine.UI.Button BackToLobbyButton;

		public UnityEngine.UI.Button LanguageButton;

        public UnityEngine.UI.Button ResolutionButton;

        public UnityEngine.UI.Button FullScreenButton;

        public Text LanguageButtonText;
        
        public Text ResolutionButtonText;
        
        public Text FullScreenButtonText;
        
		public UnityEngine.UI.Button MusicButton;

		public UnityEngine.UI.Button SoundButton;

		public Slider MusicSlider;

		public Slider SoundSlider;

		public UnityEngine.UI.Button BuyCoinsButton;

		public UnityEngine.UI.Button RestorePurchasesButton;

		public UnityEngine.UI.Button GDPRButton;

		public Text GDPRButtonText;

		public OptionsScreenView() : base() { }

		public override void Init(OptionsScreen screen)
		{
			BuyCoinsButton.gameObject.SetActive(false);
			BackToLobbyButton.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
			{
				Game.Instance.ScreenManager.Show<LobbyScreen>(true, false);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			}));
			LanguageButtonText.text = LocalizationManager.Instance.GetTranslation("options_lang_but");
            LanguageButton.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                LocalizationManager.Instance.ChangeLocale(LocalizationManager.Instance.GetNextLocale());
				LanguageButtonText.text = LocalizationManager.Instance.GetTranslation("options_lang_but");
				UserDataManager.Instance.Options.Locale = LocalizationManager.Instance.CurrentLocale.index;
				UserDataManager.Instance.Options.SaveData();
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            }));
            ResolutionButtonText.text = ResolutionManager.Instance.CurrentResolution.width + "x" + ResolutionManager.Instance.CurrentResolution.height;
			ResolutionButton.onClick.AddListener(() =>
			{
				ResolutionManager.Instance.ChangeResolution(ResolutionManager.Instance.GetNextResolution());
				ResolutionButtonText.text = ResolutionManager.Instance.CurrentResolution.width + "x" + ResolutionManager.Instance.CurrentResolution.height;
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            });
			FullScreenButtonText.text = ResolutionManager.Instance.GetFullScreenModeName();
            FullScreenButton.onClick.AddListener(() =>
            {
                ResolutionManager.Instance.ChangeFullScreenMode(ResolutionManager.Instance.GetNextFullScreenMode());
                FullScreenButtonText.text = ResolutionManager.Instance.GetFullScreenModeName();
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            });
            MusicButton.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
				UserDataManager.Instance.Options.ToggleMusic();
				MusicSlider.SetValueWithoutNotify(UserDataManager.Instance.Options.MusicLevel);
            }));
            SoundButton.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                UserDataManager.Instance.Options.ToggleSound();
                SoundSlider.SetValueWithoutNotify(UserDataManager.Instance.Options.SoundLevel);
            }));
            MusicSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>((value) =>
            {
				UserDataManager.Instance.Options.MusicLevel = value;
				UserDataManager.Instance.Options.LastUsedSetMusicValue = value;
				UserDataManager.Instance.Options.Update();
            }));
            SoundSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>((value) =>
            {
                UserDataManager.Instance.Options.SoundLevel = value;
                UserDataManager.Instance.Options.LastUsedSetSoundValue = value;
                UserDataManager.Instance.Options.Update();
            }));
            BuyCoinsButton.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
				Game.Instance.ScreenManager.Show<BuyCoinsScreen>(true, true);
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            }));
			GDPRButton.gameObject.SetActive(false);
			RestorePurchasesButton.gameObject.SetActive(false);
        }

		private void ShowGDPR(Action<bool> onResult)
		{
		}

		public override void PreShow(CommonPayloadData payload)
		{
			base.PreShow(payload);
			MusicSlider.SetValueWithoutNotify(UserDataManager.Instance.Options.MusicLevel);
            SoundSlider.SetValueWithoutNotify(UserDataManager.Instance.Options.SoundLevel);
        }

		public override void SetSelectedGO()
		{	
			EventSystem.current.SetSelectedGameObject(MusicSlider.gameObject);
		}

		public override void Back()
		{
			BackToLobbyButton.onClick?.Invoke();
		}
	}
}
