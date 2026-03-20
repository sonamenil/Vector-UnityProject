using UnityEngine;
using UnityEngine.Purchasing;

public class AppsFlyerHandler
{
	public const string APPSFLYER_GO_NAME = "AppsFlyer";

	private bool useConversionData;

	private static AppsFlyerHandler _instance;

	private bool _inited;

	private string _cachedToken;

	private MonoBehaviour _trackerCallbacks;

	public static AppsFlyerHandler Instance => null;

	private bool isSupportedPlatform => false;

	public bool canTracking => false;

	private string appID => null;

	public void Init()
	{
	}

	public void SetToken(string token)
	{
	}

	private void SetUninstallToken()
	{
	}

	public void TrackEvent(string eventName)
	{
	}

	public void TrackEventPurchase(Product p)
	{
	}

	public void TrackEventAdWatched()
	{
	}

	public void RegisterForToken()
	{
	}
}
