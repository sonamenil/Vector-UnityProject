using Nekki.Vector.Core.Controllers;
using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Trigger.Actions;
using Nekki.Vector.GUI.InputControllers;
using System.ComponentModel;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using Key = UnityEngine.InputSystem.Key;

public class LevelSceneController : MonoBehaviour
{
    public const float Z_BASE = -1f;

    public const float Z_OVERLAP = -5f;

    public const float Z_DEBUG = -10f;

    public const float Z_PLAYER = -15f;

    [SerializeField]
    private KeyboardController _keyboardController;

    [SerializeField]
    private TutorialUIController _tutorialUIController;

    [SerializeField]
    private TouchController _touchController;

    [SerializeField]
    private BotIcon _botIcon;

    [SerializeField]
    private TrickDescription _trickDescription;

    [SerializeField]
    private MessageOnScreen _messageOnScreen;

    [SerializeField]
    private DebugMenu _debugMenu;

    private bool _debugPause;

    private bool _canRender;

    private PlayerInputActions actions;

    private readonly GameplayClock _gameplayClock = new GameplayClock();

    private void Awake()
    {
        FixedRenderInterpolation.Clear();
        _debugMenu.Init();
        HideTutorialUIController();
        // _keyboardController.OnKeyDown.AddListener(OnKeyDown);
        _touchController.OnSlide += OnSlide;
        
        actions =  new PlayerInputActions();
    }

    private void OnEnable()
    {
        Camera.onPreCull += BeginCameraRender;
        Camera.onPostRender += EndCameraRender;
        RenderPipelineManager.beginCameraRendering += BeginPipelineCameraRender;
        RenderPipelineManager.endCameraRendering += EndPipelineCameraRender;
        FixedRenderInterpolation.IsRunning = CanInterpolate;
        FixedRenderInterpolation.AlphaProvider = () => _gameplayClock.Alpha;
        actions.Gameplay.Up.performed += _ => OnKeyDown(Key.UpArrow);
        actions.Gameplay.Down.performed += _ => OnKeyDown(Key.DownArrow);
        actions.Gameplay.Left.performed += _ => OnKeyDown(Key.LeftArrow);
        actions.Gameplay.Right.performed += _ => OnKeyDown(Key.RightArrow);
        
        actions.Enable();
    }

    private void OnDisable()
    {
        Camera.onPreCull -= BeginCameraRender;
        Camera.onPostRender -= EndCameraRender;
        RenderPipelineManager.beginCameraRendering -= BeginPipelineCameraRender;
        RenderPipelineManager.endCameraRendering -= EndPipelineCameraRender;
        FixedRenderInterpolation.ResetHistory();
        FixedRenderInterpolation.IsRunning = () => false;
        FixedRenderInterpolation.AlphaProvider = null;
        _gameplayClock.Reset();
        actions.Disable();
    }

    private void OnDestroy()
    {
        FixedRenderInterpolation.Clear();
        // _keyboardController.OnKeyDown.RemoveListener(OnKeyDown);
        _touchController.OnSlide -= OnSlide;
        RunnerRender.Reset();
        LevelMainController.Clear();
    }

    private void OnSlide(int _, Vector2 from, Vector2 to)
    {
        var recognizer = new SwipeGestureRecognizer(from, to);
        HandleNewInput(KeyMapping.MapFromSwipe(recognizer.Direction));
    }

    private void OnKeyDown(Key keyCode)
    {
        HandleNewInput(KeyMapping.MapFromKeycode(keyCode));
    }

    private void HandleNewInput(Nekki.Vector.Core.Controllers.Key key)
    {
        if (key == Nekki.Vector.Core.Controllers.Key.None)
        {
            return;
        }
        LevelMainController.current.HandleNewInput(new KeyVariables(key));
    }

    private void Start()
    {
        LevelMainController.Init(this);
        _botIcon.Init(LevelMainController.current.Location.GetAllBotModels());
        _canRender = true;

        if (Game.Instance.Snail)
        {
            var quadsRenderer = new GameObject("[QuadsRenderer]");
            quadsRenderer.transform.SetParent(Sets.Current.Containers[1].Object.transform, false);
            quadsRenderer.AddComponent<MeshFilter>();
            quadsRenderer.AddComponent<MeshRenderer>();
            quadsRenderer.AddComponent<QuadsRenderer>();
        }
    }

    private void Update()
    {
        if (!CanInterpolate())
        {
            _gameplayClock.Reset();
            return;
        }

        int ticks = _gameplayClock.Advance(Time.deltaTime,
            LevelMainController.current.slowModeFrames, Time.fixedDeltaTime);
        for (int i = 0; i < ticks; i++)
        {
            FixedRenderInterpolation.BeginTick();
            LevelMainController.current.Render();
            FixedRenderInterpolation.EndTick();
            if (!CanInterpolate() || LevelMainController.current.slowModeFrames == 0f)
                break;
        }
    }

    private bool CanInterpolate() => _canRender && !_debugPause &&
        LevelMainController.current != null && !LevelMainController.current.pauseRender &&
        !LevelMainController.current.tutorialPause;

    private void BeginPipelineCameraRender(ScriptableRenderContext context, Camera camera) => BeginCameraRender(camera);

    private void EndPipelineCameraRender(ScriptableRenderContext context, Camera camera) => EndCameraRender(camera);

    private void BeginCameraRender(Camera camera)
    {
        if (!_canRender || camera != Camera.main)
            return;
        FixedRenderInterpolation.ApplyTransforms(FixedRenderInterpolation.Alpha);
        _botIcon.Render();
    }

    private void EndCameraRender(Camera camera)
    {
        if (camera == Camera.main)
            FixedRenderInterpolation.RestoreTransforms();
    }

    public void ShowTutorialUIController(KeyVariables key, string description)
    {
        _tutorialUIController.gameObject.SetActive(true);
        _tutorialUIController.ShowKey(key, description);
    }

    public void HideTutorialUIController()
    {
        _tutorialUIController.Reset();
        _tutorialUIController.gameObject.SetActive(false);
    }

    public void SetVisibleTutorialOnPause(bool value)
    {
        if (!_tutorialUIController.gameObject.activeSelf)
        {
            return;
        }
        _tutorialUIController.SetVisible(value);
    }

    public void ShowActivatedTrick(TrickAreaRunner current)
    {
        _trickDescription.Show(current.itemName, current.score.ToString());
    }

    public void TrickNotBuy(TrickAreaRunner current)
    {
        _trickDescription.ShowTrickNotBuy();
    }

    public void MessageOnScreen(string text, int timeInFrame, Color color, TA_MessageOnScreen.Animation appearAnimation, TA_MessageOnScreen.Animation disappearAnimation)
    {
        _messageOnScreen.Show(text, timeInFrame, color, appearAnimation, disappearAnimation);
    }

    public void Reset()
    {
        SetVisibleTutorialOnPause(true);
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (!LevelMainController.current.CanPauseOrReload || LevelMainController.current.pauseRender || Game.Instance.Snail)
        {
            return;
        }
        Game.Instance.ScreenManager.Show<GameplayPauseScreen>(false, false);
        LevelMainController.current.pauseRender = true;
    }
}
