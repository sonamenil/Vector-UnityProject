using System;

public class InterstitialAdsHandler
{
	private static InterstitialAdsHandler _instance;

	private const string _placement = "adInterstitial";

	private bool _checkCapped;

	private bool _interstitialAdsReady;

	public Action<bool> OnVideoShown;

	public static InterstitialAdsHandler Instance => null;

	public void Init(bool checkCapped = false)
	{
	}

	~InterstitialAdsHandler()
	{
	}

	//private void OnInterstitialAdShowFailedEvent(IronSourceError obj)
	//{
	//}

	//private void OnInterstitialAdLoadFailedEvent(IronSourceError obj)
	//{
	//}

	private void OnInterstitialAdShowSucceededEvent()
	{
	}

	private void OnInterstitialAdReadyEvent()
	{
	}

	private void OnInterstitialAdClosedEvent()
	{
	}

	private void OnInterstitialAdOpenedEvent()
	{
	}

	private bool CanShowInterstitialByLimits()
	{
		return false;
	}

	public void LoadInterstitial()
	{
	}

	public bool Show()
	{
		return false;
	}

	public bool CanShow()
	{
		return false;
	}

	private bool IsInterstitialPlacementCapped(string placementName)
	{
		return false;
	}

	private bool IsInterstitialReady()
	{
		return false;
	}
}
