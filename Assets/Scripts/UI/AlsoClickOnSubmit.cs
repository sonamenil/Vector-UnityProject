using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace UI
{
    public class AlsoClickOnSubmit : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Button _button;

        private void OnEnable()
        {
            EventSystem.current.GetComponent<InputSystemUIInputModule>().submit.action.performed += AlsoClickButton;
        }

        private void OnDisable()
        {
            EventSystem.current.GetComponent<InputSystemUIInputModule>().submit.action.performed -= AlsoClickButton;
        }

        void AlsoClickButton(InputAction.CallbackContext context)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                _button.onClick?.Invoke();
            }
        }
    }
}
