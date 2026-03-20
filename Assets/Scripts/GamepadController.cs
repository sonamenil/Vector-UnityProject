using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class GamepadController : MonoBehaviour
{
    public static GamepadController Instance { get; private set; }

    public event Action OnGamepadConnected;
    public event Action OnGamepadDisconnected;

    public bool IsGamepadConnected => Gamepad.all.Count > 0;

    public Gamepad CurrentGamepad => Gamepad.current;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        if (CurrentGamepad != null && CurrentGamepad is DualShockGamepad ds)
        {
            ds.SetLightBarColor(Color.blue);
        }
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is not Gamepad gamepad)
            return;
        
        switch (change)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Reconnected:
                Debug.Log($"Controller connected: {gamepad.displayName}");
                OnGamepadConnected?.Invoke();

                if (gamepad is DualShockGamepad ds)
                {
                    ds.SetLightBarColor(Color.blue);
                }
                
                break;

            case InputDeviceChange.Disconnected:
            case InputDeviceChange.Removed:
                Debug.Log($"Controller disconnected: {gamepad.displayName}");
                OnGamepadDisconnected?.Invoke();
                break;
        }
    }
    
    public void Rumble(float low, float high, float duration)
    {
        var pad = CurrentGamepad;
        if (pad == null)
            return;

        StopAllCoroutines();
        StartCoroutine(RumbleRoutine(pad, low, high, duration));
    }

    private IEnumerator RumbleRoutine(Gamepad pad, float low, float high, float duration)
    {
        pad.SetMotorSpeeds(low, high);
        yield return new WaitForSeconds(duration);

        if (pad != null)
            pad.SetMotorSpeeds(0f, 0f);
    }
}