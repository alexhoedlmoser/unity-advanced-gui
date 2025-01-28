using System;
using System.Collections;
using System.Collections.Generic;
using AlexH.AdvancedGUI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI
{
    [AddComponentMenu("Advanced GUI/Menu Page")]
    [System.Serializable]
    public class MenuPageTransition
    {
        public string key;
        public MenuPage nextPage;
        public GradientType transitionDirection;
        public float transitionDuration = 0.5f;

        public void TransitionToPage(MenuPage currentPage)
        {
            if (!currentPage || !nextPage)
            {
                return;
            }

            currentPage.DeactivatePage();
            nextPage.ActivatePage();

            currentPage.PlayFadeOut(transitionDirection, transitionDuration);
            nextPage.PlayFadeIn(transitionDirection, transitionDuration);
        }
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuPage : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AdvancedButton backButton;

        [Header("Settings")]
        [SerializeField] private Vector3 defaultPosition = Vector3.zero;
        [Tooltip("If false, uses actual Screen resolution")] 
        [SerializeField] private bool useReferenceScreenSize = true;
        [SerializeField] private int referenceScreenWidth = 1920;
        [SerializeField] private int referenceScreenHeight = 1080;

        [Space]
        public MenuPageTransition[] transitions;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Sequence _currentSequence;
        private AdvancedButton[] _childrenButtons;
        private NavigationBar _navigationBar;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _childrenButtons = GetComponentsInChildren<AdvancedButton>(includeInactive: true);
            _navigationBar = GetComponentInChildren<NavigationBar>(includeInactive: true);
        }

        private void OnDestroy()
        {
            _currentSequence?.Kill();
        }

        #region Editor Methods

        [ContextMenu("Set new default Position")]
        public void SetDefaultPosition()
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            
            defaultPosition = _rectTransform.localPosition;
        }
        
        
        [ContextMenu("Enable Page")]
        public void EnablePage()
        {
            gameObject.SetActive(true);
            
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
            
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
        [ContextMenu("Disable Page")]
        public void DisablePage()
        {
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
            
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        #endregion
        
        private Vector3 GetPositionDeltaFromGradient(GradientType gradientType)
        {
            Vector3 deltaVector = Vector3.zero;
            
            int screenWidth = useReferenceScreenSize ? referenceScreenWidth : Screen.width;
            int screenHeight = useReferenceScreenSize ? referenceScreenHeight : Screen.height;

            switch (gradientType)
            {
                case GradientType.None:
                    break;
                case GradientType.LeftToRight:
                    deltaVector = new Vector3(-screenWidth, 0, 0);
                    break;
                case GradientType.RightToLeft:
                    deltaVector = new Vector3(screenWidth, 0, 0);
                    break;
                case GradientType.TopToBottom:
                    deltaVector = new Vector3(0, screenHeight, 0);
                    break;
                case GradientType.BottomToTop:
                    deltaVector = new Vector3(0, -screenHeight, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gradientType), gradientType, null);
            }

            return deltaVector;
        }

        public void ActivatePage()
        {
            gameObject.SetActive(true);
            
            ActivateShortcuts();
            
            _navigationBar?.ActivateShortcut();
            
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
        public void DeactivatePage()
        {
            DeactivateShortcuts();
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void ActivateShortcuts()
        {
            foreach (AdvancedButton button in _childrenButtons)
            {
                button.ActivateShortcut();
            }
            
            _navigationBar?.ActivateShortcut();
        }
        
        public void DeactivateShortcuts()
        {
            foreach (AdvancedButton button in _childrenButtons)
            {
                button.DeactivateShortcut();
            }
            
            _navigationBar?.DeactivateShortcut();
        }

        public Sequence PageFadeOutSequence(GradientType transitionDirection, float duration)
        {
            return DOTween.Sequence()
                .Append(_rectTransform.DOLocalMove(defaultPosition - GetPositionDeltaFromGradient(transitionDirection), duration).SetEase(Ease.OutQuint))
                .Join(_canvasGroup.DOFade(0f, duration));
        }

        public Sequence PageFadeInSequence(GradientType transitionDirection, float duration)
        {
            _rectTransform.localPosition = defaultPosition + GetPositionDeltaFromGradient(transitionDirection);
            
            return DOTween.Sequence()
                .Append(_rectTransform.DOLocalMove(defaultPosition, duration).SetEase(Ease.OutQuint))
                .Join(_canvasGroup.DOFade(1f, duration));
        }

        public void PlayFadeOut(GradientType transitionDirection, float duration)
        {
            _currentSequence?.Kill();
            _currentSequence = PageFadeOutSequence(transitionDirection, duration).OnComplete((() => gameObject.SetActive(false)));
        }
        
        public void PlayFadeIn(GradientType transitionDirection, float duration)
        {
            _currentSequence?.Kill();
            _currentSequence = PageFadeInSequence(transitionDirection, duration);
        }

        public void PlayTransitionByIndex(int index)
        {
            transitions[index].TransitionToPage(this);
        }

        public void PlayTransitionByKey(string key)
        {
            foreach (MenuPageTransition transition in transitions)
            {
                if (transition.key == key)
                {
                    transition.TransitionToPage(this);
                    return;
                }
            }
        }
        
    }
}
