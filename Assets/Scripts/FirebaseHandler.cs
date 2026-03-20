using System.Collections;

public class FirebaseHandler : IFirebaseHandler
{
	private static FirebaseHandler _instance;

	//private FirebaseApp _firebaseApp;

	private bool _isInit;

	private bool _checkAndFixDependenciesProcess;

	private string _token;

	private FirebaseUnsendData _unsendData;

	private static bool? _isSupported;

	public static FirebaseHandler instance => null;

	public string Token => null;

	private bool isFullInit => false;

	public static bool IsSuported => false;

	private FirebaseHandler()
	{
	}

	public void Init()
	{
	}

	public void Clear()
	{
	}

	private void RegisterFirebase()
	{
	}

	//private void InitFirebase(DependencyStatus taskResult)
	//{
	//}

	private IEnumerator AddExtraDataToCrashlytics()
	{
		return null;
	}

	public void LogException(string message)
	{
	}

	public void Log(string message)
	{
	}

	public void SetCustomKey(string key, string value)
	{
	}

	public void AnalyticLogEvent(string name, string parameterName, string value)
	{
	}

	public void AnalyticLogEvent(string name, (string, string) param1, (string, string) param2)
	{
	}

	protected void SetToken(string token)
	{
	}

	//private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
	//{
	//}

	//private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
	//{
	//}

	private void OnApplicationQuit()
	{
	}
}
