public interface IFirebaseHandler
{
	void Init();

	void Clear();

	void LogException(string message);

	void Log(string message);

	void SetCustomKey(string key, string value);
}
