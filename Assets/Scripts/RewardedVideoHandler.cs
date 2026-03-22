using System;
using DG.Tweening;

public class RewardedVideoHandler
{
	public enum VideoAdsAvailability
	{
		PlatformIsNotSupported,
		LimitReached,
		VideoIsNotAvailable,
		CanShow
	}

	public enum OnVideoRewardFailsReasons
	{
		None,
		IronSourceFails,
		CallbackWaitTimeout,
		ShowAdTimeout
	}

	private static class VideoRewardChecker
	{
		public static bool VideoShowed
		{
			get;
			set;
		}

		public static void Reset()
		{
		}
	}

	private const float WaitRewardTimerValue = 5f;

	private const float WaitShowTimerValue = 5f;

	private const string PlacementBoostTrackCoins = "adRevardedTrailCoins";

	private const string PlacementCoinsByVideo = "adRevardedBonusCoins";

	private bool _soundMuted;

	private Sequence _waitRewardTimer;

	private Sequence _waitShowTimer;

	private bool _adRewardedEventWas;

	private RewardedVideoReward _rewardAction;

	private static RewardedVideoHandler _instance;

	public static RewardedVideoHandler Instance => null;

	public event Action<OnVideoRewardFailsReasons> OnVideoShown;

	public event Action OnVideoClosed;

	~RewardedVideoHandler()
	{
	}

	private void OnVideoShownCallBack(OnVideoRewardFailsReasons obj)
	{
	}

	public static void Restart()
	{
	}

	public bool CanShowCoinsByVideo()
	{
		return false;
	}

	public bool CanShowCoinsBoost()
	{
		return false;
	}

	public VideoAdsAvailability CanShowVideoResult(string placement)
	{
		return default(VideoAdsAvailability);
	}

	private bool ShowVideoAd(string placement = null)
	{
		return false;
	}

	private void OnVideoShownFailedCB()
	{
	}

	private void OnVideoClosedCB()
	{
	}

	private void OnShowVideoCalledCB()
	{
	}

	private void OnVideoStartedCB()
	{
	}

	private void OnVideoRewardedCB(string placement)
	{
	}

	private void RewardedVideoAdRewardedEvent(string placementName)
	{
	}

	private void RunWaitRewardTimer()
	{
	}

	private void RunWaitShowTimer()
	{
	}

	private void StopWaitShowTimer()
	{
	}

	private void HandleApplicationPause()
	{
	}

	private void HandleApplicationResume()
	{
	}

	public static void Reset()
	{
	}

	public bool ShowVideoAndGiveCoins(int value)
	{
		return false;
	}

	public bool ShowVideoAndBoostCoins()
	{
		return false;
	}
}
