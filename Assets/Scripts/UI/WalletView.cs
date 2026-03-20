using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class WalletView : MonoBehaviour
	{
		public Text StarsText;

		public Text CoinsText;

		private UserDataManager _playerData;

		public bool _pauseAnimation;

		public int _lastValue;

		private void Awake()
		{
			_playerData = UserDataManager.Instance;
			_playerData.MainData.CoinsChangedEvent += RunAnimation;
			UiRoot.Instance.pauseAnimationWalletView += OnPauseAnimation;
			_lastValue = _playerData.MainData.GetCoins();
			StarsText.text = _playerData.GameStats.GetAllStars().ToString();
			CoinsText.text = _lastValue.ToString();
		}

		private void RunAnimation(int oldValue, int newValue)
		{
            int value = oldValue;

            var getter = new DOGetter<int>(() => value);
            var setter = new DOSetter<int>(x => value = x);

            var t = DOTween.To(getter, setter, newValue, 1f)
                .OnUpdate(() =>
                {
                    CoinsText.text = value.ToString();
                })
                .SetEase(Ease.InQuad);
        }

		private void OnPauseAnimation(bool value)
		{
			_pauseAnimation = value;
			if (value)
			{
				_lastValue = _playerData.MainData.GetCoins();
			}
			else
			{
				RunAnimation(_lastValue, _playerData.MainData.GetCoins());
			}
		}

		public void Update()
		{
			StarsText.text = _playerData.GameStats.GetAllStars().ToString();
		}
	}
}
