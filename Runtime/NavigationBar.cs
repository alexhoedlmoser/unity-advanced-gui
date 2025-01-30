using System;
using AlexH.AdvancedGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH
{
    [AddComponentMenu("Advanced GUI/Navigation Bar")]
    public class NavigationBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AdvancedNavButton[] navButtons;
        [SerializeField] private CanvasGroup[] menuPages;
        
        [Header("Events")]
        [SerializeField] private UnityEvent[] onButtonSelectedEvents;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference previousPageInputAction;
        [SerializeField] private InputActionReference nextPageInputAction;

        private int _currentNavIndex;

        private void OnEnable()
        {
            foreach (AdvancedNavButton navButton in navButtons)
            {
                navButton.OnSelected += OnButtonSelectedHandler;
            }
            
            StartCoroutine(navButtons[0].SelectButtonOnStartDelayed());
        }

        private void OnNextPageInput(InputAction.CallbackContext obj)
        {
            NextPage();
        }

        private void OnPreviousPageInput(InputAction.CallbackContext obj)
        {
            PreviousPage();
        }
        
        private void OnDisable()
        {
            foreach (AdvancedNavButton navButton in navButtons)
            {
                navButton.OnSelected -= OnButtonSelectedHandler;
            }
        }

        private void OnButtonSelectedHandler(AdvancedNavButton navButton)
        {
            int newNavIndex = Array.IndexOf(navButtons, navButton);

            if (newNavIndex != _currentNavIndex)
            {
                navButtons[_currentNavIndex].UnselectButton();
            }
            
            SwitchToPage(newNavIndex);

            onButtonSelectedEvents[newNavIndex]?.Invoke();
            
            _currentNavIndex = newNavIndex;
        }

        private void SwitchToPage(int menuIndex)
        {
            foreach (CanvasGroup page in menuPages)
            {
                page.blocksRaycasts = false;
                page.alpha = 0f;
                //page.gameObject.SetActive(false);
            }
            
            //menuPages[menuIndex].gameObject.SetActive(true);
            menuPages[menuIndex].blocksRaycasts = true;
            menuPages[menuIndex].alpha = 1f;
        }

        private void NextPage()
        {
            if (_currentNavIndex+1 >= navButtons.Length)
            {
                SimulateButtonClick(navButtons[0].gameObject);
                //navButtons[0].SelectButton();
            }
            else
            {
                SimulateButtonClick(navButtons[_currentNavIndex+1].gameObject);
                //navButtons[_currentNavIndex+1].SelectButton();
            }
        }

        private void PreviousPage()
        {
            if (_currentNavIndex-1 < 0)
            {
                SimulateButtonClick( navButtons[^1].gameObject);
                //navButtons[^1].SelectButton();
            }
            else
            {
                SimulateButtonClick(navButtons[_currentNavIndex - 1].gameObject);
                //navButtons[_currentNavIndex - 1].SelectButton();
            }
        }

        public void DeactivateShortcut()
        {
            if (previousPageInputAction == null || nextPageInputAction == null) return;
            
            previousPageInputAction.action.performed -= OnPreviousPageInput;
            nextPageInputAction.action.performed -= OnNextPageInput;
        }
        
        public void ActivateShortcut()
        {
            if (previousPageInputAction == null || nextPageInputAction == null) return;
            
            previousPageInputAction.action.performed += OnPreviousPageInput;
            nextPageInputAction.action.performed += OnNextPageInput;
        }
    }
}
