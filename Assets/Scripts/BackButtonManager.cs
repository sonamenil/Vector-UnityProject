using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackButtonManager : MonoBehaviour
{
	private static BackButtonManager _instance;

	public Action OnBackButton;
	
	public PlayerInputActions playerActions;

	public InputAction back;

	private void Awake()
	{
		playerActions = new PlayerInputActions();
	}

	private void OnEnable()
	{
		back = playerActions.UI.Back;
		back.Enable();
		
		back.performed += _ => OnBackButton?.Invoke();
	}

	private void OnDisable()
	{
		back.Disable();
	}

	public static BackButtonManager Instance
	{
		get
		{
			if (_instance == null)
			{
				var obj = new GameObject("BackButtonManager");
				_instance = obj.AddComponent<BackButtonManager>();
				DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}

	// private void Update()
	// {
	// 	if (Input.GetKeyDown(KeyCode.Escape))
	// 	{
 //            OnBackButton?.Invoke();
	// 	}
	// }
}
