using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiRoot : MonoBehaviour
    {
        public static UiRoot Instance;

        public Image FadeInOUt;

        public GameObject ScreensParent;

        public GameObject PopupParent;

        public Action<bool> pauseAnimationWalletView;

        [SerializeField]
        private float _maxAspect = 1.7777778f;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void Start()
        {
            UpdateCanvasScaler();
        }

        private void UpdateCanvasScaler()
        {
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.matchWidthOrHeight = Camera.main.aspect < _maxAspect ? 0f : 1f;
        }

        public void OffWalletAnimation()
        {
            pauseAnimationWalletView?.Invoke(true);
        }

        public void OnWalletAnimation()
        {
            pauseAnimationWalletView?.Invoke(false);
        }
    }
}
