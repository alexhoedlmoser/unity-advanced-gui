using System;
using System.Collections;
using System.Collections.Generic;
using AlexH.AdvancedGUI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace AlexH
{
    public class NavigationBar : MonoBehaviour
    {
        [SerializeField] private AdvancedNavButton[] navButtons;
        [SerializeField] private CanvasGroup[] menuPages;

        [SerializeField] private InputActionReference previousPageInputAction;
        [SerializeField] private InputActionReference nextPageInputAction;

        private int _currentNavIndex;

        // Start is called before the first frame update
        void Start()
        {
            navButtons[0].SelectButton();
        }

        private void OnEnable()
        {
            foreach (AdvancedNavButton navButton in navButtons)
            {
                navButton.OnSelected += OnButtonSelectedHandler;
            }

            previousPageInputAction.action.performed += OnPreviousPageInput;
            nextPageInputAction.action.performed += OnNextPageInput;
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
            
            previousPageInputAction.action.performed -= OnPreviousPageInput;
            nextPageInputAction.action.performed -= OnNextPageInput;
        }

        private void OnButtonSelectedHandler(AdvancedNavButton navButton)
        {
            int newNavIndex = Array.IndexOf(navButtons, navButton);

            if (newNavIndex != _currentNavIndex)
            {
                navButtons[_currentNavIndex].UnselectButton();
            }
            
            SwitchToPage(newNavIndex);
            _currentNavIndex = newNavIndex;
        }

        private void SwitchToPage(int menuIndex)
        {
            foreach (CanvasGroup page in menuPages)
            {
                page.blocksRaycasts = false;
                page.alpha = 0f;
            }

            menuPages[menuIndex].blocksRaycasts = true;
            menuPages[menuIndex].alpha = 1f;
        }

        public void NextPage()
        {
            if (_currentNavIndex+1 >= navButtons.Length)
            {
                navButtons[0].SelectButton();
            }
            else
            {
                navButtons[_currentNavIndex+1].SelectButton();
            }
        }
        
        public void PreviousPage()
        {
            if (_currentNavIndex-1 < 0)
            {
                navButtons[^1].SelectButton();
            }
            else
            {
                navButtons[_currentNavIndex - 1].SelectButton();
            }
        }
    }
}
