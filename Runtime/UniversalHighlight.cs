using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static AlexH.Helper;

namespace AlexH.AdvancedGUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UniversalHighlight : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Every Selectable in the hierarchy of this object will use this Universal Highlight")]
        [SerializeField] private GameObject uiParent;

        [Header("Settings")]
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float bounceDuration = 0.1f;
        [Range(0.0f, 1f)] 
        [SerializeField] private float bounceStrength = 0.25f;
        [SerializeField] private float fadeDuration = 0.1f;
        
        [SerializeField] private float transitionSizeDelta = 50f;
        [SerializeField] private float endSizeDelta = 10f;
        [SerializeField] private float endAlpha = 0.75f;
        
        private Vector2 _currentSize;
        private Vector3 _currentPosition;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        
        private AdvancedSelectable[] _selectables;
        private RectTransform _currentSelectableTransform;

        private Sequence _currentSequence;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _selectables = uiParent.GetComponentsInChildren<AdvancedSelectable>();
        }

        private void Start()
        {
            InitializeHighlight();
        }

        private void InitializeHighlight()
        {
            _canvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            foreach (AdvancedSelectable selectable in _selectables)
            {
                selectable.OnHover += OnHoverHandler;
            }
        }

        private void OnDisable()
        {
            foreach (AdvancedSelectable selectable in _selectables)
            {
                selectable.OnHover -= OnHoverHandler;
            }
        }

        private void OnDestroy()
        {
            _currentSequence?.Kill();
        }

        private void OnHoverHandler(AdvancedSelectable selectable, bool hover)
        {
            //print(selectable.name + " Hover: " + hover);
            
            RectTransform selectableTransform = selectable.GetComponent<RectTransform>();

            if (!selectable.useUniversalHighlight)
            {
                SetTo(selectableTransform.position);
                _currentSelectableTransform = selectableTransform;
                return;
            }
            
            if (hover)
            {
                if (!_currentSelectableTransform || _currentSelectableTransform == selectableTransform)
                {
                    SetTo(selectableTransform.position);
                }
                
                _currentSequence?.Kill();
                _currentSequence = AnimateTo(selectableTransform);
                _currentSelectableTransform = selectableTransform;
            }

            else
            {
                _currentSequence?.Kill();
                _currentSequence = FadeOut();
            }
        }

        private Sequence AnimateTo(RectTransform rectTransform)
        {
            _rectTransform.sizeDelta = GetPaddedSize(rectTransform.sizeDelta, transitionSizeDelta);
            Vector2 endSize = GetPaddedSize(rectTransform.sizeDelta, endSizeDelta);

            return DOTween.Sequence()
                .Append(_canvasGroup.DOFade(endAlpha, animationDuration).SetEase(Ease.Linear))
                .Join(_rectTransform.DOSizeDelta(endSize, animationDuration).SetEase(Ease.OutCubic))
                .Join(_rectTransform.DOMove(rectTransform.position, animationDuration).SetEase(Ease.OutCubic))
                .Append(_rectTransform.DOSizeDelta(GetPaddedSize(rectTransform.sizeDelta, transitionSizeDelta * bounceStrength), bounceDuration/2).SetEase(Ease.OutQuint))
                .Append(_rectTransform.DOSizeDelta(endSize, bounceDuration/2).SetEase(Ease.InQuint));
        }

        private void SetTo(Vector3 position)
        {
            _rectTransform.position = position;
        }

        private Sequence FadeOut()
        {
            return DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0f, fadeDuration));
        }
    }
}


