using System.Collections.Generic;
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
		private string _music;

		[SerializeField]
		private int _coins;

		[SerializeField]
		private string _objects;

		[SerializeField]
		private List<ChoiceContainer> _choices;

		[SerializeField, TextArea(10, 25)]
		private List<string> _models;

		[SerializeField]
		private List<Xml2PrefabObjectRunnerContainer> _runners;

		[SerializeField]
		private List<Xml2PrefabVisualContainer> _visuals;

		public string Sets => _sets;

		public string Music => _music;

		public int Coins => _coins;

		public string Objects => _objects;

		public List<ChoiceContainer> Choices => _choices;

		public List<string> Models => _models;

		public List<Xml2PrefabObjectRunnerContainer> Runners => _runners;

		public List<Xml2PrefabVisualContainer> Visuals => _visuals;

		public static bool LoadLevel;

#if UNITY_EDITOR
        private void Awake()
        {
			LoadLevel = false;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
			{
				DontDestroyOnLoad(this);
				LoadLevel = true;
				SceneManager.LoadScene("Scenes/Preloader");
            }
        }
#endif

        public void Init(string sets, string music, int coins, string objects, List<ChoiceContainer> choices, List<string> models, List<Xml2PrefabObjectRunnerContainer> runners, List<Xml2PrefabVisualContainer> visuals)
		{
            _sets = sets;
            _music = music;
            _coins = coins;
            _objects = objects;
            _choices = choices;
            _models = models;
            _runners = runners;
            _visuals = visuals;
        }
	}
}
