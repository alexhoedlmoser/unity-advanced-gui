using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI
{
    [AddComponentMenu("Advanced GUI/Advanced Selectables/Advanced Button")]
    public class AdvancedButton : AdvancedSelectable
    {
        [Header("--- Button Specific ---")]
        [Space]
        public InputActionReference shortcutInputAction;
        public UnityEvent onClickEvent;

        [Header("Settings")] 
        [SerializeField] private float clickDelay;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            Invoke(nameof(OnClick), clickDelay);
        }

        public void OnClick()
        {
            onClickEvent.Invoke();
        }

        public void ActivateShortcut()
        {
            if (shortcutInputAction)
            {
                shortcutInputAction.action.performed += OnShortcutInputAction;
            }
        }
        
        public void DeactivateShortcut()
        {
            if (shortcutInputAction)
            {
                shortcutInputAction.action.performed -= OnShortcutInputAction;
            }
        }

        private void OnShortcutInputAction(InputAction.CallbackContext obj)
        {
            SimulateButtonClick(gameObject);
        }
    }
}

