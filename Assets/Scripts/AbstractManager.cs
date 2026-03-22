using UnityEngine;

public abstract class AbstractManager<T> where T : AbstractManager<T>, new()
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (IsInited)
			{
				return _instance;
			}
			Debug.LogError("[" + typeof(T) + "] Run Init before use");
			return null;
		}
	}

	public static bool IsInited => _instance != null;

	public static void Init()
	{
		if (!IsInited)
		{
			_instance =  new T();
			_instance.InitInternal();
		}
		else
		{
			Debug.LogError("[" + typeof(T) + "] Already inited!");
		}
	}

	public static void Clear()
	{
		_instance = null;
	}

	protected abstract void InitInternal();
}
