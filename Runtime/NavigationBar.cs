using System;
using System.Collections;
using System.Collections.Generic;
using AlexH.AdvancedGUI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH
{
    public class NavigationBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AdvancedNavButton[] navButtons;
        [SerializeField] private CanvasGroup[] menuPages;

        [Header("Input Actions")]
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
    }
}
