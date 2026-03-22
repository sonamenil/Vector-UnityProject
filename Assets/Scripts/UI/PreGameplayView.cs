using System.Collections;
using System.Linq;
using Nekki.Vector.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PreGameplayView : ScreenViewWithCommonPayload<PreGameplayScreen>
    {
        public UnityEngine.UI.Button BackToSelectStoryButton;

        public UnityEngine.UI.Button PlayButton;

        public UnityEngine.UI.Button BoostButton;

        public UnityEngine.UI.Button SelectTrickButton;

        public GameObject ContentParent;

        public UnityEngine.UI.Button BuyCoinsButton;

        public UnityEngine.UI.Button BuyGadgetsButton;

        public GameObject GadgetIcon;

        public Text Caption;

        public Text Title;

        public ScrollSnap ScrollSnap;

        private bool _ShowBoostDialogRequestBeen;

        public override void Init(PreGameplayScreen screen)
        {
            BuyCoinsButton.gameObject.SetActive(false);

            PlayButton.onClick.AddListener(() =>
            {
                _ShowBoostDialogRequestBeen = false;
                CoroutineRunner.Instance.Run(Load());
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
                SoundsManager.Instance.AudioSourceBackground.Stop();
            });
            BackToSelectStoryButton.onClick.AddListener(() =>
            {
                Game.Instance.ScreenManager.Show<SelectStoryScreen>(true, false);
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            });
            BuyCoinsButton.onClick.AddListener(() =>
            {
                Game.Instance.ScreenManager.Show<BuyCoinsScreen>(true, true);
                SoundsManager.Instance.PlaySounds(SoundType.ui_click);
            });
            BuyGadgetsButton.onClick.AddListener(() =>
            {
                Game.Instance.ScreenManager.Show<StoreGadgetsScreen, GadgetScreenPayloadData>(new GadgetScreenPayloadData(true), true, true);
            });
            
            ScrollSnap.SnapEvent += i =>
            {
                EventSystem.current.SetSelectedGameObject(ScrollSnap._content.GetChild(i).GetComponent<HolderItem>().Button.gameObject);
            };
        }

        private void OnBoostButton()
        {
        }

        public static IEnumerator Load()
        {
            var sm = Game.Instance.ScreenManager;
            sm.Show<LoadingScreen>(false, false);
            var storyInfo = UserDataManager.Instance.CurrentBalanceLocation.CurrentStoryModeStoryInfos[UserDataManager.RuntimeInfo.CurrentStory];
            CurrentTrackInfo.Current.LocationFile = storyInfo.Name.Replace("_HUNTER", "") + ".xml";

            SceneManager.LoadScene("Scenes/Level", LoadSceneMode.Additive);

            yield return null;

            LevelMainController.current.pauseRender = true;

            if (storyInfo.CutsceneStart != null)
            {
                var payload = new VideoScreenPayloadData(storyInfo.CutsceneStart, null);
                sm.Show<VideoScreen, VideoScreenPayloadData>(payload, true, false);
                while (payload.IsPlaying)
                {
                    yield return null;
                }

            }

            yield return sm.FadeInCoroutine();

            yield return null;

            sm.Show<GameplayScreen>(false, false);

            yield return sm.FadeOutCoroutine();

            yield return null;

            LevelMainController.current.pauseRender = false;
            yield break;
        }


        public override void PreShow(CommonPayloadData payload)
        {
            var playerData = UserDataManager.Instance;
            var storyInfo = playerData.CurrentBalanceLocation.CurrentStoryModeStoryInfos[UserDataManager.RuntimeInfo.CurrentStory];
            Title.text = LocalizationManager.Instance.GetTranslationByID(storyInfo.Name);
            GadgetIcon.SetActive(playerData.ShopData.IsEquipped("GADGET_FORCEBLASTER"));
            foreach (Transform child in ContentParent.transform)
            {
                Destroy(child.gameObject);
            }
            var tricks = StoreManager.Instance.GetItems(StoreItemType.Tricks).Where(t => storyInfo.TrickIds.Contains(t.Id)).ToList();
            StoreTricksScreenView.InsertEmptyDummies(ScrollSnap._content, 10);
            StoreTricksScreenView.PutItemsIntoContent(ScrollSnap, tricks, StoreItemType.Tricks, false);
            StoreTricksScreenView.InsertEmptyDummies(ScrollSnap._content, 10);
            ScrollSnap.StartIndex = 10;
            ScrollSnap.EndIndex = tricks.Count + 9;
            RefreshBoostButton();
            
        }

        private void RefreshBoostButton()
        {
            BoostButton.gameObject.SetActive(false);
        }

        public override void PostShow(CommonPayloadData payload)
        {
            ScrollSnap.Recalculate();
            ScrollSnap.Snap(ScrollSnap.StartIndex, true);
        }
        
        public override void OnEnable()
        {
            base.OnEnable();
            ScrollSnap.enabled = true;
            
            actions.UI.YButton.performed += _ => BuyGadgetsButton.onClick?.Invoke();
            actions.UI.XButton.performed += _ => PlayButton.onClick?.Invoke();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            ScrollSnap.enabled = false;
        }

        public override void Back()
        {
            BackToSelectStoryButton.onClick?.Invoke();
        }
    }
}
