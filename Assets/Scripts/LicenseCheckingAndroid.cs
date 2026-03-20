using UnityEngine;

public class LicenseCheckingAndroid : LicenseCheckingAbstract
{
	private class LicensingServiceCallback : AndroidJavaProxy
	{
		private LicenseCheckingAndroid _licenseCheckingManager;

		public LicensingServiceCallback(LicenseCheckingAndroid licenseChecking)
			: base((string)null)
		{
		}

		public void allow(string payloadJson)
		{
		}

		public void dontAllow(AndroidJavaObject pendingIntent)
		{
		}

		public void applicationError(string errorMessage)
		{
		}
	}

	private AndroidJavaObject m_Activity;

	private AndroidJavaObject m_LicensingHelper;

	private string m_PublicKey_Base64;

	private bool m_RunningOnAndroid;

	private LicenseCheckingManager _manager;

	public LicenseCheckingAndroid(string publicKey_Base64)
	{
	}

	public override void RunCheck(LicenseCheckingManager manager)
	{
	}

	public void Allow(string payloadJson)
	{
	}

	private void DontAllow(AndroidJavaObject pendingIntent)
	{
	}

	private void ApplicationError(string errorMessage)
	{
	}
}
