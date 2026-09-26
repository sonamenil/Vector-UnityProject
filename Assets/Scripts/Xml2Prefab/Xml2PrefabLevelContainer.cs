using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Xml2Prefab
{
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    public class Xml2PrefabLevelContainer : MonoBehaviour
    {
        [SerializeField, TextArea(10, 25)]
        private string _sets;

        [SerializeField]
        private string _music = "menu";

        [SerializeField]
        private int _coins;

        [SerializeField]
        private string _objects;

        [SerializeField]
        private List<ChoiceContainer> _choices = new List<ChoiceContainer>();

        [SerializeField, TextArea(10, 25)]
        private List<ModelsContainer> _models = new List<ModelsContainer>();

        [SerializeField]
        private List<Xml2PrefabObjectRunnerContainer> _runners = new List<Xml2PrefabObjectRunnerContainer>();

        [SerializeField]
        private List<Xml2PrefabVisualContainer> _visuals = new List<Xml2PrefabVisualContainer>();

#if UNITY_EDITOR
        [Header("Editor Launch")]
        [SerializeField]
        private bool _launchThisLevelOnPlay = true;

        [SerializeField]
        private bool _showPlatforms = true;

        [SerializeField]
        private bool _showAreas = true;

        [SerializeField]
        private bool _showTriggers = true;

        [SerializeField]
        private bool _showDetectors = false;

        public static Xml2PrefabLevelContainer Active { get; private set; }
#endif

        public string Sets => _sets;
        public string Music => _music;
        public int Coins => _coins;
        public string Objects => _objects;
        public List<ChoiceContainer> Choices => _choices;
        public List<ModelsContainer> Models => _models;
        public List<Xml2PrefabObjectRunnerContainer> Runners => _runners;
        public List<Xml2PrefabVisualContainer> Visuals => _visuals;

        public static bool LoadLevel;

        public void Init(
            string sets,
            string music,
            int coins,
            string objects,
            List<ChoiceContainer> choices,
            List<ModelsContainer> models,
            List<Xml2PrefabObjectRunnerContainer> runners,
            List<Xml2PrefabVisualContainer> visuals)
        {
            _sets = sets;
            _music = music;
            _coins = coins;
            _objects = objects;
            _choices = choices ?? new List<ChoiceContainer>();
            _models = models ?? new List<ModelsContainer>();
            _runners = runners ?? new List<Xml2PrefabObjectRunnerContainer>();
            _visuals = visuals ?? new List<Xml2PrefabVisualContainer>();
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            Active = this;

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            if (!Application.isPlaying)
            {
                UpdateHierarchy();
            }
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            if (Active == this)
            {
                Active = null;
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                UpdateHierarchy();
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateHierarchy();
            }
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredPlayMode)
            {
                return;
            }

            if (!_launchThisLevelOnPlay)
            {
                return;
            }

            BeginEditorLevelLaunch();
        }

        private void BeginEditorLevelLaunch()
        {
            Active = this;
            LoadLevel = true;

            DontDestroyOnLoad(gameObject);

            Game.Instance.Snail = true;
            Game.Instance.SnailSett.SnailLevel = "__EDITOR_LEVEL__";
            Game.Instance.SnailSett.UsePrefab = false;
            Game.Instance.SnailSett.ShowPlatforms = _showPlatforms;
            Game.Instance.SnailSett.ShowAreas = _showAreas;
            Game.Instance.SnailSett.ShowTriggers = _showTriggers;
            Game.Instance.SnailSett.ShowDetectors = _showDetectors;

            SceneManager.LoadScene("Scenes/Preloader");
        }

        public void UpdateHierarchy()
        {
            if (_choices == null) _choices = new List<ChoiceContainer>();
            if (_models == null) _models = new List<ModelsContainer>();

            _visuals = GetComponentsInChildren<Xml2PrefabVisualContainer>(true)
                .Where(x => x.gameObject != gameObject)
                .ToList();

            _runners = GetComponentsInChildren<Xml2PrefabObjectRunnerContainer>(true)
                .Where(IsTopLevelObjectContainer)
                .ToList();

            foreach (var runner in _runners)
            {
                runner.UpdateHierarchy();
            }
        }

        private static bool IsTopLevelObjectContainer(Xml2PrefabObjectRunnerContainer container)
        {
            if (container == null || container.transform.parent == null)
            {
                return true;
            }

            return container.transform.parent.GetComponentInParent<Xml2PrefabObjectRunnerContainer>() == null;
        }
#endif
    }
}