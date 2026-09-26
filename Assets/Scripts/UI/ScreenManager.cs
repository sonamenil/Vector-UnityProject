using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace UI
{
    //fuck this and fuck nekki
    public class ScreenManager
    {
        private class CacheRecord<T, TPayload> where T : Screen
        {
            public T Screen;

            public ScreenView<T, TPayload> ScreenView;

            public CacheRecord(T screen, ScreenView<T, TPayload> screenView)
            {
                Screen = screen;
                ScreenView = screenView;
            }
        }

        private AsyncOperationHandle<GameObject> _uiRootHandle;

        private readonly List<AsyncOperationHandle<GameObject>> _viewHandles = new();

        private readonly Dictionary<Type, object> _screens = new Dictionary<Type, object>();

        private Type _currentScreenType;

        private GameObject _currentScreenView;

        private Stack<Tuple<Type, GameObject, Action, Action, Action>> _previousStack = new Stack<Tuple<Type, GameObject, Action, Action, Action>>();

        private Type _previousScreenType;

        private GameObject _previousScreenView;

        private GameObject _popupView;

        public float Duration = 0.4f;

        public UiRoot uiRoot;

        public ScreenManager()
        {
            //Used ChatGPT for this because what in the fuck is this method
            //God bless the poor soul who thought that this was the only way to do this
            //Sadly, I couldn't care less about rewriting UI code

            _uiRootHandle = Addressables.LoadAssetAsync<GameObject>(
                "Assets/UI/Prefabs/UIROOT.prefab");

            GameObject prefab = _uiRootHandle.WaitForCompletion();
            if (_uiRootHandle.Status != AsyncOperationStatus.Succeeded || prefab == null)
            {
                Addressables.Release(_uiRootHandle);
                throw new InvalidOperationException("Could not load UIRoot");
            }

            GameObject instance = Object.Instantiate(prefab);
            uiRoot = instance.GetComponent<UiRoot>();
            if (uiRoot == null)
            {
                Object.Destroy(instance);
                Addressables.Release(_uiRootHandle);
                throw new InvalidOperationException("UIROOT prefab has no UiRoot component");
            }

            Object.DontDestroyOnLoad(instance);

            _screens[typeof(LobbyScreen)] = InitAndCreateCacheRecord(new LobbyScreen(this), LoadScreenView<LobbyScreen, CommonPayloadData>());
            _screens[typeof(OptionsScreen)] = InitAndCreateCacheRecord(new OptionsScreen(this), LoadScreenView<OptionsScreen, CommonPayloadData>());
            _screens[typeof(ChooseModeScreen)] = InitAndCreateCacheRecord(new ChooseModeScreen(this), LoadScreenView<ChooseModeScreen, CommonPayloadData>());
            _screens[typeof(SelectLocationScreen)] = InitAndCreateCacheRecord(new SelectLocationScreen(this), LoadScreenView<SelectLocationScreen, CommonPayloadData>());
            _screens[typeof(SelectStoryScreen)] = InitAndCreateCacheRecord(new SelectStoryScreen(this), LoadScreenView<SelectStoryScreen, CommonPayloadData>());
            _screens[typeof(PreGameplayScreen)] = InitAndCreateCacheRecord(new PreGameplayScreen(this), LoadScreenView<PreGameplayScreen, CommonPayloadData>());
            _screens[typeof(GameplayScreen)] = InitAndCreateCacheRecord(new GameplayScreen(this), LoadScreenView<GameplayScreen, CommonPayloadData>());
            _screens[typeof(PostGameplayWinScreen)] = InitAndCreateCacheRecord(new PostGameplayWinScreen(this), LoadScreenView<PostGameplayWinScreen, CommonPayloadData>());
            _screens[typeof(GameplayPauseScreen)] = InitAndCreateCacheRecord(new GameplayPauseScreen(this), LoadScreenView<GameplayPauseScreen, CommonPayloadData>());
            _screens[typeof(StoreGadgetsScreen)] = InitAndCreateCacheRecord(new StoreGadgetsScreen(this), LoadScreenView<StoreGadgetsScreen, GadgetScreenPayloadData>());
            _screens[typeof(StoreTricksScreen)] = InitAndCreateCacheRecord(new StoreTricksScreen(this), LoadScreenView<StoreTricksScreen, CommonPayloadData>());
            _screens[typeof(StoreGearScreen)] = InitAndCreateCacheRecord(new StoreGearScreen(this), LoadScreenView<StoreGearScreen, CommonPayloadData>());
            _screens[typeof(StoreScreen)] = InitAndCreateCacheRecord(new StoreScreen(this), LoadScreenView<StoreScreen, CommonPayloadData>());
            _screens[typeof(StatsScreen)] = InitAndCreateCacheRecord(new StatsScreen(this), LoadScreenView<StatsScreen, CommonPayloadData>());
            _screens[typeof(EmptyScreen)] = InitAndCreateCacheRecord(new EmptyScreen(this), LoadScreenView<EmptyScreen, CommonPayloadData>());
            _screens[typeof(LoadingScreen)] = InitAndCreateCacheRecord(new LoadingScreen(this), LoadScreenView<LoadingScreen, CommonPayloadData>());
            _screens[typeof(VideoScreen)] = InitAndCreateCacheRecord(new VideoScreen(this), LoadScreenView<VideoScreen, VideoScreenPayloadData>());
            _screens[typeof(GetFreeScreen)] = InitAndCreateCacheRecord(new GetFreeScreen(this), LoadScreenView<GetFreeScreen, CommonPayloadData>());
            _screens[typeof(BuyCoinsScreen)] = InitAndCreateCacheRecord(new BuyCoinsScreen(this), LoadScreenView<BuyCoinsScreen, CommonPayloadData>());
            _screens[typeof(YesNoPopup)] = InitAndCreateCacheRecord(new YesNoPopup(this), LoadPopupView<YesNoPopup, CommonPayloadData>());
            _screens[typeof(ComingSoonPopup)] = InitAndCreateCacheRecord(new ComingSoonPopup(this), LoadPopupView<ComingSoonPopup, CommonPayloadData>());
            _screens[typeof(BuyItemPopup)] = InitAndCreateCacheRecord(new BuyItemPopup(this), LoadPopupView<BuyItemPopup, BuyItemPayloadData>());
            _screens[typeof(BuyCoinsPopup)] = InitAndCreateCacheRecord(new BuyCoinsPopup(this), LoadPopupView<BuyCoinsPopup, BuyCoinsPayloadData>());
            _screens[typeof(UnlockLocationPopup)] = InitAndCreateCacheRecord(new UnlockLocationPopup(this), LoadPopupView<UnlockLocationPopup, UnlockLocationPopupPayloadData>());
            _screens[typeof(BuyLocationPopup)] = InitAndCreateCacheRecord(new BuyLocationPopup(this), LoadPopupView<BuyLocationPopup, BuyLocationPayloadData>());
            _screens[typeof(UnlockStoryPopup)] = InitAndCreateCacheRecord(new UnlockStoryPopup(this), LoadPopupView<UnlockStoryPopup, UnlockStoryPopupPayloadData>());
            _screens[typeof(HunterUnlockPopup)] = InitAndCreateCacheRecord(new HunterUnlockPopup(this), LoadPopupView<HunterUnlockPopup, CommonPayloadData>());
            _screens[typeof(QuitStoryPopup)] = InitAndCreateCacheRecord(new QuitStoryPopup(this), LoadPopupView<QuitStoryPopup, CommonPayloadData>());
            _screens[typeof(AlertPopup)] = InitAndCreateCacheRecord(new AlertPopup(this), LoadPopupView<AlertPopup, AlertPayloadData>());
            _screens[typeof(BuyPremiumPopup)] = InitAndCreateCacheRecord(new BuyPremiumPopup(this), LoadPopupView<BuyPremiumPopup, CommonPayloadData>());
            _screens[typeof(CountdownPopup)] = InitAndCreateCacheRecord(new CountdownPopup(this), LoadPopupView<CountdownPopup, CommonPayloadData>());
            _screens[typeof(BoostPopup)] = InitAndCreateCacheRecord(new BoostPopup(this), LoadPopupView<BoostPopup, BoostPayloadData>());
        }

        private CacheRecord<T, TPayload> InitAndCreateCacheRecord<T, TPayload>(T screen, ScreenView<T, TPayload> screenView) where T : Screen
        {
            screenView.Init(screen);
            return new CacheRecord<T, TPayload>(screen, screenView);
        }

        private ScreenView<T, TPayload> LoadScreenView<T, TPayload>() where T : Screen
        {
            return LoadView<T, TPayload>(uiRoot.ScreensParent.transform);
        }

        private ScreenView<T, TPayload> LoadPopupView<T, TPayload>() where T : Screen
        {
            return LoadView<T, TPayload>(uiRoot.PopupParent.transform);
        }

        private ScreenView<T, TPayload> LoadView<T, TPayload>(Transform parent)
            where T : Screen
        {
            var attribute = typeof(T).GetCustomAttribute<ViewAttribute>();
            if (attribute == null)
                throw new InvalidOperationException($"{typeof(T).Name} has no ViewAttribute");

            string address = $"Assets/UI/Prefabs/{attribute.Path}.prefab";
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            GameObject prefab = handle.WaitForCompletion();

            if (handle.Status != AsyncOperationStatus.Succeeded || prefab == null)
            {
                Addressables.Release(handle);
                throw new InvalidOperationException($"Could not load view: {address}");
            }

            var obj = Object.Instantiate(prefab, parent);
            var view = obj.GetComponent<ScreenView<T, TPayload>>();
            if (view == null)
            {
                Object.Destroy(obj);
                Addressables.Release(handle);
                throw new InvalidOperationException(
                    $"{address} has no ScreenView<{typeof(T).Name}, {typeof(TPayload).Name}>");
            }

            obj.SetActive(false);
            _viewHandles.Add(handle);
            return view;
        }

        public void Refresh(Action postRefresh = null, bool keepPopUp = false)
        {
            var popUp = _popupView;
            if (_previousStack.Count > 0)
                _previousStack.Peek().Item5?.Invoke();
            
            if (popUp != null && keepPopUp)
            {
                popUp.SetActive(true);
                _popupView = popUp;
            }
            
            if (postRefresh != null)
            {
                postRefresh();
            }
        }

        private IEnumerator RefreshCoroutine(Action postRefresh)
        {
            if (_previousStack.Count > 0)
                _previousStack.Peek().Item5?.Invoke();
            
            yield return null; 
            
            if (postRefresh != null)
            {
                postRefresh();
            }
        }

        private void ReShow<T, TPayload>(TPayload payload, bool fade) where T : Screen
        {
            var cached = (CacheRecord<T, TPayload>)_screens[typeof(T)];

            cached.ScreenView.PreShow(payload);
            Action post = () =>
            {
                cached.ScreenView.PostShow(payload);
                cached.ScreenView.SetSelectedGO();
                
            };

            Show(typeof(T), cached.ScreenView.gameObject, fade, post);
        }


        public void Show<T>(bool fade, bool stack) where T : Screen
        {
            Show<T, CommonPayloadData>(new CommonPayloadData(), fade, stack);
        }

        public void Show<T, TPayload>(TPayload payload, bool fade, bool stack) where T : Screen
        {
            if (!stack)
                _previousStack.Clear();

            var cached = (CacheRecord<T, TPayload>)_screens[typeof(T)];
            cached.ScreenView.PreShow(payload);

            Action postShow = () =>
            {
                cached.ScreenView.PostShow(payload);
                cached.ScreenView.SetSelectedGO();

            };
            
            Show(typeof(T), cached.ScreenView.gameObject, fade, postShow);
            
            if (stack && _previousStack.Count > 0 && _previousStack.Peek().Item1 == typeof(T))
                return;

            _previousStack.Push(new Tuple<Type, GameObject, Action, Action, Action>(
                typeof(T),
                cached.ScreenView.gameObject,
                () => cached.ScreenView.Back(),
                () => ReShow<T, TPayload>(payload, true),
                () => ReShow<T, TPayload>(payload, false)
            ));
        }


        private void Show(Type screenType, GameObject screenViewGameObject, bool fade, Action postShow = null)
        {
            CoroutineRunner.Instance.Run(ShowCoroutine(_currentScreenView, _popupView, screenViewGameObject, fade, postShow));
            screenViewGameObject.transform.SetAsLastSibling();
            _currentScreenType = screenType;
            _currentScreenView = screenViewGameObject;
        }

        public void Back()
        {
            if (_popupView != null) { ClosePopup(); return; }
            if (_previousStack.Count == 0) return;
            _previousStack.Peek().Item3?.Invoke();
        }

        public void ClearStack()
        {
            _previousStack.Clear();
        }

        public void ShowPrevious()
        {
            if (_previousStack.Count == 0) return;
            _previousStack.Pop();
            if (_previousStack.Count == 0) return;
            _previousStack.Peek().Item4?.Invoke();
        }

        public T Popup<T>() where T : Screen
        {
            return Popup<T, CommonPayloadData>(new CommonPayloadData()); ;
        }

        public T Popup<T, TPayload>(TPayload payload) where T : Screen
        {
            var cached = (CacheRecord<T, TPayload>)_screens[typeof(T)];
            cached.ScreenView.PreShow(payload);
            var val = ShowPopup(cached, payload);
            cached.ScreenView.PostShow(payload);
            
            cached.ScreenView.SetSelectedGO();
            return val;
        }

        private T ShowPopup<T, TPayload>(CacheRecord<T, TPayload> screen, TPayload payload) where T : Screen
        {
            _popupView = screen.ScreenView.gameObject;
            _popupView.SetActive(true);
            
            if (_currentScreenView != null)
                _currentScreenView.GetComponent<ScreenView>().OnDisable();
            
            return screen.Screen;
        }

        public void CommingSoon()
        {
            ShowPopup((CacheRecord<ComingSoonPopup, CommonPayloadData>)_screens[typeof(ComingSoonPopup)], new CommonPayloadData());
        }

        public void ClosePopup()
        {
            if (_popupView != null)
            {
                _popupView.gameObject.SetActive(false);
                _popupView = null;

                if (_currentScreenView != null)
                {
                    var screenView = _currentScreenView.GetComponent<ScreenView>();
                    screenView.OnEnable();
                    screenView.SetSelectedGO();
                }
            }
        }

        public void FadeIn(Action action = null)
        {
            CoroutineRunner.Instance.Run(FadeInCoroutine(action));
        }

        public IEnumerator FadeInCoroutine(Action action = null)
        {
            var startTime = Time.realtimeSinceStartup;
            var fadeInOut = uiRoot.FadeInOUt;

            fadeInOut.gameObject.SetActive(true);
            var color = fadeInOut.color;
            color.a = 0;
            fadeInOut.color = color;

            while (Time.realtimeSinceStartup < startTime + Duration)
            {
                color.a = (Time.realtimeSinceStartup - startTime) / Duration;
                fadeInOut.color = color;
                yield return null;
            }
            action?.Invoke();
        }

        public void FadeOut(Action action = null)
        {
            CoroutineRunner.Instance.Run(FadeOutCoroutine(action));
        }

        public IEnumerator FadeOutCoroutine(Action action = null)
        {
            var startTime = Time.realtimeSinceStartup;
            var fadeInOut = uiRoot.FadeInOUt;

            var color = fadeInOut.color;
            color.a = 1;
            fadeInOut.color = color;

            while (Time.realtimeSinceStartup < startTime + Duration)
            {
                color.a = 1 - (Time.realtimeSinceStartup - startTime) / Duration;
                fadeInOut.color = color;
                yield return null;
            }

            fadeInOut.gameObject.SetActive(false);
            action?.Invoke();
        }

        private IEnumerator ShowCoroutine(GameObject currentScreenView, GameObject popupView, GameObject nextScreenView, bool fade, Action postShow)
        {
            var input = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            BackButtonManager.Instance.playerActions.Disable();
            if (input != null)
                input.enabled = false;
            if (fade)
            {
                yield return FadeInCoroutine();
            }
            if (popupView != null)
            {
                popupView.gameObject.SetActive(false);
            }
            if (currentScreenView != null)
            {
                currentScreenView.gameObject.SetActive(false);
            }
            if (nextScreenView != null)
            {
                nextScreenView.gameObject.SetActive(true);
            }
            yield return new WaitForEndOfFrame();
            postShow?.Invoke();
            if (fade)
            {
                yield return FadeOutCoroutine();
            }
            BackButtonManager.Instance.playerActions.Enable();
            if (input != null)
                input.enabled = true;
            yield return true;
        }
    }
}
