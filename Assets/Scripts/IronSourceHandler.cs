using System;

public class IronSourceHandler
{
	public class EventsSyncHelper
	{
		public bool WasVideoAdClosed;

		public bool WasVideoAdRewarded;

		//public IronSourcePlacement PlacementName;
	}

	private static IronSourceHandler _instance;

	private static bool? _isSupportedPlatform;

	private EventsSyncHelper _eventSyncHelper;

	//private UnityMainThreadDispatcher _dispatcher;

	private bool _ironSourceIninted;

	public static IronSourceHandler Instance => null;

	public static bool isSupportedPlatform => false;

	private bool MemoryLimit => false;

	public event Action OnVideoShownFailed;

	public event Action OnVideoClosed;

	public event Action OnVideoStarted;

	public event Action<string> OnVideoRewarded;

	public event Action OnShowVideoCalled;

	public event Action OnInterstitialAdReadyEvent;

	//public event Action<IronSourceError> OnInterstitialAdLoadFailedEvent;

	public event Action OnInterstitialAdShowSucceededEvent;

	//public event Action<IronSourceError> OnInterstitialAdShowFailedEvent;

	public event Action OnInterstitialAdClickedEvent;

	public event Action OnInterstitialAdOpenedEvent;

	public event Action OnInterstitialAdClosedEvent;

	private string GetApiKey()
	{
		return null;
	}

	public void Init()
	{
	}

	public RewardedVideoHandler.VideoAdsAvailability CanShowVideo(string placement)
	{
		return default(RewardedVideoHandler.VideoAdsAvailability);
	}

	public bool ShowVideoAd(string placement = null)
	{
		return false;
	}

	private void RecreateThreadDispatcher()
	{
	}

	private void DispatchInMainThread(Action action)
	{
	}

	public void LoadInterstitial()
	{
	}

	public void ShowInterstitial(string placementName)
	{
	}

	public bool IsInterstitialPlacementCapped(string placementName)
	{
		return false;
	}

	public bool IsInterstitialReady()
	{
		return false;
	}

	public void AddCallback()
	{
	}

	public void Restart()
	{
	}

	public void DisposeDispatcher()
	{
	}

	private void InterstitialAdClosedEvent()
	{
	}

	private void InterstitialAdOpenedEvent()
	{
	}

	private void InterstitialAdClickedEvent()
	{
	}

	//private void InterstitialAdShowFailedEvent(IronSourceError obj)
	//{
	//}

	private void InterstitialAdShowSucceededEvent()
	{
	}

	//private void InterstitialAdLoadFailedEvent(IronSourceError obj)
	//{
	//}

	private void InterstitialAdReadyEvent()
	{
	}

	public void RemoveCallback()
	{
	}

	public void OnApplicationPause(bool isPaused)
	{
	}

	//private void RewardedVideoAdShowFailedEvent(IronSourceError error)
	//{
	//}

	//private void RewardedVideoAdRewardedEvent(IronSourcePlacement p)
	//{
	//}

	private void RewardedVideoAvailabilityChangedEvent(bool available)
	{
	}

	private void RewardedVideoAdClosedEvent()
	{
	}

	private void RewardedVideoAdOpenedEvent()
	{
	}

	private void RewardedVideoAdStartedEvent()
	{
	}

	private void RewardedVideoAdEndedEvent()
	{
	}
}
