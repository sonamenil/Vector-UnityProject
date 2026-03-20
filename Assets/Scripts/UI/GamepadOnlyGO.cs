using UnityEngine;

namespace UI
{
    public class GamepadOnlyGO : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _gameObjects;
        
        private void OnEnable()
        {
            foreach (GameObject go in _gameObjects)
            {
                go.SetActive(GamepadController.Instance.IsGamepadConnected);
            }
            
            GamepadController.Instance.OnGamepadConnected += OnGamepadConnected;
            GamepadController.Instance.OnGamepadDisconnected += OnGamepadDisconnected;
        }

        private void OnDisable()
        {
            GamepadController.Instance.OnGamepadConnected -= OnGamepadConnected;
            GamepadController.Instance.OnGamepadDisconnected -= OnGamepadDisconnected;
        }

        void OnGamepadConnected()
        {
            foreach (GameObject go in _gameObjects)
            {
                go.SetActive(true);
            }
        }

        void OnGamepadDisconnected()
        {
            foreach (GameObject go in _gameObjects)
            {
                go.SetActive(false);
            }
        }
    }
}
