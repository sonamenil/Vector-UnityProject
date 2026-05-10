using UnityEngine;
using System.Collections.Generic;

public class SysDlg : MonoBehaviour
{
	private static List<string> _messages = new();

	private static string _message;

	private static bool _exit;

	private static SysDlg _instance;

	private static Texture2D _t;

	public static void Show(string message, bool exit)
	{
		if (!_t)
		{
			_t = new Texture2D(1, 1);
			_t.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.8f));
		}
		if (!_messages.Contains(message))
		{
			_messages.Add(message);
            _message = _message + "\n\n" + message;
        }
		
		_exit = exit;
		if (!_instance)
		{
			_instance = new GameObject("_message").AddComponent<SysDlg>();
			DontDestroyOnLoad(_instance);
		}
		Time.timeScale = 0f;
	}

	private void Update()
	{
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			if (touches[i].phase == TouchPhase.Ended && new Rect(Screen.width / 10f, Screen.height / 8f * 6f, Screen.width / 10f * 8f, Screen.height / 8f).Contains(touches[i].position))
			{
				Close();
			}
		}
	}

	private void Close()
	{
		if (_exit)
		{
			Application.Quit();
			return;
		}
		Time.timeScale = 1f;
		Destroy(gameObject);
	}

	private void OnGUI()
	{
		GUI.depth = -100000;
		TextAnchor alignment = GUI.skin.label.alignment;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _t);
		GUI.Label(new Rect(Screen.width / 10f, Screen.height / 8f, Screen.width / 10f * 8f, Screen.height / 8f * 8f), string.Format("<color=white><size={1}>{0}</size></color>", _message, (Screen.width <= Screen.height ? Screen.height : Screen.width) / 40));
		GUI.skin.label.alignment = alignment;
		if (GUI.Button(new Rect(Screen.width / 10f, Screen.height / 8f * 6f, Screen.width / 10f * 8f, Screen.height / 8f), "OK"))
		{
			Close();
		}
	}
}
