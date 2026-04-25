using Nekki.Vector.Core.Controllers;
using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Trigger.Actions;
using Nekki.Vector.GUI.InputControllers;
using System.ComponentModel;
using UI;
using UnityEngine;
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

    private void Awake()
    {
        _debugMenu.Init();
        HideTutorialUIController();
        // _keyboardController.OnKeyDown.AddListener(OnKeyDown);
        _touchController.OnSlide += OnSlide;
        
        actions =  new PlayerInputActions();
    }

    private void OnEnable()
    {
        actions.Gameplay.Up.performed += _ => OnKeyDown(Key.UpArrow);
        actions.Gameplay.Down.performed += _ => OnKeyDown(Key.DownArrow);
        actions.Gameplay.Left.performed += _ => OnKeyDown(Key.LeftArrow);
        actions.Gameplay.Right.performed += _ => OnKeyDown(Key.RightArrow);
        
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void OnDestroy()
    {
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
            new GameObject("[QuadsRenderer]").AddComponent<QuadsRenderer>();
        }
    }

    private void FixedUpdate()
    {
        if (!_debugPause && _canRender)
        {
            LevelMainController.current.Render();
            _botIcon.Render();
        }
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
