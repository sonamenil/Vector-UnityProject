using System.Collections;
using UnityEngine;

namespace UI
{
	public class CoroutineRunner : MonoBehaviour
	{
		private static CoroutineRunner _instance;

		public static CoroutineRunner Instance
		{
			get
			{
				if (_instance == null)
				{
					var obj = new GameObject("CoroutineRunner");
					_instance = obj.AddComponent<CoroutineRunner>();
					DontDestroyOnLoad(obj);
                }
				return _instance;
			}
		}

		public Coroutine Run(IEnumerator coroutine)
		{
			return StartCoroutine(coroutine);
		}
	}
}
