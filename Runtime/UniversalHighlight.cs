using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UniversalHighlight : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Every Selectable in the hierarchy of this object will use this Universal Highlight")]
        [SerializeField] private GameObject uiParent;
        [Space]
        [SerializeField] private Image borderImage;
        [SerializeField] private Image fillImage;

        [Header("Colors and Alpha")]
        [SerializeField] private float generalAlpha = 0.75f;
        [Space]
        [SerializeField] private float borderAlpha = 1f;
        [SerializeField] private float fillAlpha = 0.1f;
        [Space]
        [SerializeField] private GradientType borderGradientType;
        [SerializeField] private GradientType fillGradientType;
        [Range(0, 1)]
        [SerializeField] private float borderGradientSoftness = 0.5f;
        [Range(0, 1)]
        [SerializeField] private float fillGradientSoftness = 0.5f;
        [Space]
        [Tooltip("Sets the colors to the default color of the highlighted Selectable, instead of the hover color of the highlighted Selectable")]
        [SerializeField] private bool matchSelectableDefaultColor;
        
        [Header("Animation Properties")]
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float bounceDuration = 0.1f;
        [Range(0.0f, 1f)] 
        [SerializeField] private float bounceStrength = 0.25f;
        [SerializeField] private float fadeDuration = 0.1f;
        
        [SerializeField] private float transitionSizeDelta = 50f;
        [SerializeField] private float endSizeDelta = 10f;
        
        private Vector2 _currentSize;
        private Vector3 _currentPosition;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        
        private AdvancedSelectable[] _selectables;
        private RectTransform _currentSelectableTransform;

        private RectMask2D _borderMask;
        private RectMask2D _fillMask;

        private Sequence _currentSequence;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _borderMask = borderImage.GetComponentInParent<RectMask2D>();
            _fillMask = fillImage.GetComponentInParent<RectMask2D>();
            
            _selectables = uiParent.GetComponentsInChildren<AdvancedSelectable>(includeInactive: true);
        }

        private void Start()
        {
            InitializeHighlight();
        }

        private void InitializeHighlight()
        {
            _canvasGroup.alpha = 0f;
            SetImageAlpha(borderImage, borderAlpha);
            SetImageAlpha(fillImage, fillAlpha);
            SetupGradients();
        }

        private void SetupGradients()
        {
            if (!_borderMask || !_fillMask) return;

            _borderMask.enabled = borderGradientType != GradientType.None;
            _fillMask.enabled = fillGradientType != GradientType.None;

            _borderMask.softness = GetSoftnessFromGradient(borderGradientType, _rectTransform.sizeDelta, borderGradientSoftness);
            _fillMask.softness = GetSoftnessFromGradient(fillGradientType, _rectTransform.sizeDelta, fillGradientSoftness);

            _borderMask.padding = GetPaddingFromGradient(borderGradientType, -1000);
            _fillMask.padding = GetPaddingFromGradient(fillGradientType, -1000);
        }

        private void UpdateGradients()
        {
            if (!_borderMask || !_fillMask) return;
            
            _borderMask.softness = GetSoftnessFromGradient(borderGradientType, _rectTransform.sizeDelta, borderGradientSoftness);
            _fillMask.softness = GetSoftnessFromGradient(fillGradientType, _rectTransform.sizeDelta, fillGradientSoftness);
        }

        private void UpdateColors(Color color)
        {
            RecolorImageWithAlpha(borderImage, color, borderAlpha);
            RecolorImageWithAlpha(fillImage, color, fillAlpha);
        }

        private void OnEnable()
        {
            foreach (AdvancedSelectable selectable in _selectables)
            {
                selectable.OnHover += OnHoverHandler;
                selectable.OnPressed += OnPressedHandler;
            }
        }

        private void OnDisable()
        {
            foreach (AdvancedSelectable selectable in _selectables)
            {
                selectable.OnHover -= OnHoverHandler;
                selectable.OnPressed -= OnPressedHandler;
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
                SetTo(selectableTransform.localPosition);
                _currentSelectableTransform = selectableTransform;
                return;
            }
            
            if (hover)
            {
                _rectTransform.SetParent(selectableTransform.parent, true);
                _rectTransform.localScale = Vector3.one;
                
                
                if (!_currentSelectableTransform || _currentSelectableTransform == selectableTransform)
                {
                    SetTo(selectableTransform.localPosition);
                }

                UpdateColors(matchSelectableDefaultColor ? selectable.GetDefaultColor() : selectable.GetHoverColor());
                
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

        private void OnPressedHandler(AdvancedSelectable selectable, bool pressed)
        {
            if (pressed)
            {
                UpdateColors(matchSelectableDefaultColor ? selectable.GetDefaultColor() : selectable.GetPressedColor());
            }

            else
            {
                UpdateColors(matchSelectableDefaultColor ? selectable.GetDefaultColor() : selectable.GetHoverColor());
            }
        }

        private Sequence AnimateTo(RectTransform rectTransform)
        {
            _rectTransform.sizeDelta = GetPaddedSize(rectTransform.sizeDelta, transitionSizeDelta);
            Vector2 endSize = GetPaddedSize(rectTransform.sizeDelta, endSizeDelta);
            _canvasGroup.alpha = 0f;

            return DOTween.Sequence()
                .Append(_canvasGroup.DOFade(generalAlpha, animationDuration).SetEase(Ease.Linear))
                .Join(_rectTransform.DOSizeDelta(endSize, animationDuration).SetEase(Ease.OutCubic))
                .Join(_rectTransform.DOLocalMove(rectTransform.localPosition, animationDuration).SetEase(Ease.OutCubic))
                .AppendCallback(UpdateGradients)
                .Append(_rectTransform.DOSizeDelta(GetPaddedSize(rectTransform.sizeDelta, endSizeDelta + transitionSizeDelta * bounceStrength), bounceDuration/2).SetEase(Ease.OutQuint))
                .Append(_rectTransform.DOSizeDelta(endSize, bounceDuration/2).SetEase(Ease.InQuint));
        }

        private void SetTo(Vector3 position)
        {
            _rectTransform.localPosition = position;
            UpdateGradients();
        }

        private Sequence FadeOut()
        {
            return DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0f, fadeDuration));
        }
    }
}


