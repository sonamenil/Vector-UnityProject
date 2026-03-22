using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class GDPRPopupView : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Button _okButton;

        [SerializeField]
        private UnityEngine.UI.Button _termsOfUseButton;

        private Action onConfirm;

        private void Awake()
        {
            _okButton.onClick.AddListener(() => onConfirm?.Invoke());
            _termsOfUseButton.onClick.AddListener(() =>
            {
                Application.OpenURL("https://nekki.com/en/legal/privacy/");
            });
		}

		private void OnDestroy()
        {
            _okButton.onClick.RemoveAllListeners();
            _termsOfUseButton.onClick.RemoveAllListeners();
        }

        public void Init(Action confirmCB)
        {
            gameObject.SetActive(true);
            onConfirm = confirmCB;
            
            EventSystem.current.SetSelectedGameObject(_okButton.gameObject);
        }
    }
}
