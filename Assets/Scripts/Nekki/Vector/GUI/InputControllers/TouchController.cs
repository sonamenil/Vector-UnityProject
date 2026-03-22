using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Nekki.Vector.GUI.InputControllers
{
    public class TouchController : MonoBehaviour
    {
        public Action<int, Vector2, Vector2> OnSlide;
        public Action<int, Vector2, Vector2> OnDrag;

        private const float MinimumDistance = 30f;

        private readonly Dictionary<int, Vector2> _touches = new();

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            _touches.Clear();
        }

        public static void SetEnabledAll(bool value)
        {
            TouchController[] controllers =
                FindObjectsByType<TouchController>(FindObjectsSortMode.None);

            foreach (TouchController controller in controllers)
            {
                controller.enabled = value;
            }
        }

        private void Update()
        {
            HandleTouchControl();
            HandleMouseControl();
        }

        private void HandleMouseControl()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null)
                return;

            Vector2 currentPos = mouse.position.ReadValue();

            HandleMouseButton(0, mouse.leftButton, currentPos);
        }

        private void HandleMouseButton(int buttonId, ButtonControl button, Vector2 currentPos)
        {
            if (button.wasPressedThisFrame)
            {
                _touches[buttonId] = currentPos;
            }

            if (button.isPressed && _touches.TryGetValue(buttonId, out Vector2 startPos))
            {
                float distance = Vector2.Distance(startPos, currentPos);

                if (distance > MinimumDistance)
                {
                    OnSlide?.Invoke(buttonId, startPos, currentPos);
                    _touches[buttonId] = currentPos;
                }
            }

            if (button.wasReleasedThisFrame)
            {
                _touches.Remove(buttonId);
            }
        }

        private void HandleTouchControl()
        {
            foreach (var touch in Touch.activeTouches)
            {
                int fingerId = touch.touchId;
                Vector2 position = touch.screenPosition;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touches[fingerId] = position;
                        break;

                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (_touches.TryGetValue(fingerId, out Vector2 startPos))
                        {
                            float distance = Vector2.Distance(startPos, position);

                            if (distance > MinimumDistance)
                            {
                                OnSlide?.Invoke(fingerId, startPos, position);
                                _touches[fingerId] = position;
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        _touches.Remove(fingerId);
                        break;
                }
            }
        }
    }
}