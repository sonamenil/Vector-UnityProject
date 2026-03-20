using System;
using Newtonsoft.Json.Linq;

public abstract class BaseUserHolder
{
	public abstract void ParseData();

	public abstract void SaveData();
}
public abstract class BaseUserHolder<T> : BaseUserHolder where T : BaseUserHolder<T>, new()
{
	protected JToken _userjObject;

	protected abstract string JSONName
	{
		get;
	}

	public static T Create(JObject jObject)
	{
		var instance = Activator.CreateInstance<T>();
		instance.ParseInternal(jObject);
		return instance;
	}

	private void ParseInternal(JObject jObject)
	{
		_userjObject = jObject[JSONName];
		if (_userjObject == null)
		{
			var j = new JObject();
			jObject[JSONName] = j;
			_userjObject = j;
		}
		ParseData();
	}
}
