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
        private const float MinimumDistanceSqr = MinimumDistance * MinimumDistance;

        private readonly Dictionary<int, Vector2> _touches = new();

        private bool _useTouch;
        private bool _useMouse;

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();

            _useTouch = Touchscreen.current != null;
            _useMouse = Mouse.current != null;
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
            if (_useTouch)
                HandleTouchControl();

            if (_useMouse)
                HandleMouseControl();
        }

        private void HandleMouseControl()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null)
                return;

            Vector2 currentPos = mouse.position.ReadValue();
            HandlePointer(0, mouse.leftButton, currentPos);
        }

        private void HandlePointer(int id, ButtonControl button, Vector2 currentPos)
        {
            if (button.wasPressedThisFrame)
            {
                _touches[id] = currentPos;
                return;
            }

            if (button.wasReleasedThisFrame)
            {
                _touches.Remove(id);
                return;
            }

            if (!button.isPressed || !_touches.TryGetValue(id, out Vector2 lastPos))
                return;

            Vector2 delta = currentPos - lastPos;

            if (delta.sqrMagnitude > MinimumDistanceSqr)
            {
                OnSlide?.Invoke(id, lastPos, currentPos);
                _touches[id] = currentPos;
            }

            OnDrag?.Invoke(id, lastPos, currentPos);
        }

        private void HandleTouchControl()
        {
            var activeTouches = Touch.activeTouches;

            for (int i = 0; i < activeTouches.Count; i++)
            {
                var touch = activeTouches[i];
                int fingerId = touch.touchId;
                Vector2 position = touch.screenPosition;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touches[fingerId] = position;
                        break;

                    case TouchPhase.Moved:
                        if (_touches.TryGetValue(fingerId, out Vector2 lastPos))
                        {
                            Vector2 delta = position - lastPos;

                            if (delta.sqrMagnitude > MinimumDistanceSqr)
                            {
                                OnSlide?.Invoke(fingerId, lastPos, position);
                                _touches[fingerId] = position;
                            }

                            OnDrag?.Invoke(fingerId, lastPos, position);
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