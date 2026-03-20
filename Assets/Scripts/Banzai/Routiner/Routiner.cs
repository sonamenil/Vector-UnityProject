using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banzai.Routiner
{
	public class Routiner
	{
		public class RoutinerBehaviour : MonoBehaviour
		{
			private const float EPSILON = 0.0001f;

			private readonly List<Action> _everyUpdateActions = new List<Action>();

			private readonly List<Action> _everySecondActions = new List<Action>();

			private readonly List<Action> _tmpActionsList = new List<Action>();

			private float _lastTimeTick;

			private void OnDestroy()
			{
				Debug.Log("Routiner OnDestory");
				CancelInvoke();
				StopAllCoroutines();
			}

			public virtual void AddUpdate(Action action)
			{
				_everyUpdateActions.Add(action);
			}

			public virtual void RemoveUpdate(Action action)
			{
				_everyUpdateActions.Remove(action);
			}

			public virtual void AddSecondTick(Action action)
			{
				_everySecondActions.Add(action);
			}

			public virtual void RemoveSecondTick(Action action)
			{
				_everySecondActions.Remove(action);
			}

			protected virtual void Update()
			{
				if (_lastTimeTick + 1 < Time.unscaledTime)
				{
					_lastTimeTick = Time.unscaledTime;
					SafeIterateActionsList(_everySecondActions);
				}
				SafeIterateActionsList(_everyUpdateActions);
			}

			private void SafeIterateActionsList(IEnumerable<Action> actions)
			{
				_tmpActionsList.Clear();
				_tmpActionsList.AddRange(actions);
				foreach (var action in _tmpActionsList)
				{
					action.Invoke();
				}
			}
		}

		private static RoutinerBehaviour _instance;

		public static RoutinerBehaviour instance
		{
			get
			{
				if (Application.isPlaying)
				{
					if (_instance == null)
					{
						_instance = new GameObject("_routine").AddComponent<RoutinerBehaviour>();
						DontDestroy();
					}
					return _instance;
				}
				throw new InvalidOperationException("Routiner is not supported in Editor Mode");
			}
		}

		private static void DontDestroy()
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(instance);
			}
		}

		public static void Clear()
		{
			Debug.Log("Routiner Clear");
			if (instance != null)
			{
				UnityEngine.Object.DestroyImmediate(instance.gameObject);
			}
		}

		public static Coroutine Go(IEnumerator routine)
		{
			if (instance != null)
			{
				return instance.StartCoroutine(routine);
			}
			return null;
		}

		public static void AddUpdate(Action action)
		{
			if (instance != null)
			{
				instance.AddUpdate(action);
			}
		}

		public static void RemoveUpdate(Action action)
		{
			if (instance != null)
			{
				instance.RemoveUpdate(action);
			}
		}
	}
}
