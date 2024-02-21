using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI
{
    public class AdvancedCarouselButton : AdvancedSelectable
    {
        [Header("--- Carousel Button Specific ---")]
        [Space]
        public UnityEvent<int> onOptionChanged;

        [Header("References")] 
        [SerializeField] private GameObject leftButtonRaycast;   
        [SerializeField] private GameObject rightButtonRaycast;
        [Space]
        [SerializeField] private RectTransform carouselBarTransform;
        [SerializeField] private RectTransform leftHoverHighlightTransform;
        [SerializeField] private RectTransform rightHoverHighlightTransform;
        [Space]
        [SerializeField] private Image leftArrowImage;
        [SerializeField] private Image rightArrowImage;
        [SerializeField] private Image leftArrowBackgroundImage;
        [SerializeField] private Image rightArrowBackgroundImage;
        [SerializeField] private Image[] hoverHighlightImages;
        [Space]
        [SerializeField] private TMP_Text valueText;

        [Header("Animation Properties")]
        [SerializeField] private float carouselBarHoverHeightDelta;
        [SerializeField] private float hoverHighlightAlpha = 1f;
        
        private Sequence _currentCarouselBarSequence;
        private Sequence _currentCarouselLeftSideSequence;
        private Sequence _currentCarouselRightSideSequence;

        private Vector2 _carouselBarDefaultSize;
        private RectTransform _rightArrowTransform;
        private RectTransform _leftArrowTransform;

        private bool _leftSideHovered;
        private bool _rightSideHovered;
        
        private int _currentOptionIndex;
        private int _optionCount;

        protected override void Awake()
        {
            
            _carouselBarDefaultSize = carouselBarTransform.sizeDelta;
            _rightArrowTransform = rightArrowImage.GetComponent<RectTransform>();
            _leftArrowTransform = leftArrowImage.GetComponent<RectTransform>();

            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _currentCarouselBarSequence?.Kill();
            _currentCarouselLeftSideSequence?.Kill();
            _currentCarouselRightSideSequence?.Kill();
        }
        
        protected override void LoadStyle()
        {
            base.LoadStyle();

            if (!currentStyle) return;

            if (valueText)
            {
                valueText.font = currentStyle.numbersFontAsset ? currentStyle.numbersFontAsset : currentStyle.textFontAsset;
                valueText.fontSizeMax = currentStyle.fontSize;
                valueText.characterSpacing = currentStyle.defaultCharacterSpacing;
            }
        }
        
        public void SetupCarousel(int optionCount, int defaultOptionIndex)
        {
            _optionCount = optionCount;
            _currentOptionIndex = defaultOptionIndex;
        }

        private void NextOption()
        {
            if (_currentOptionIndex < _optionCount - 1)
            {
                _currentOptionIndex++;
            }
            else
            {
                _currentOptionIndex = 0;
            }
            
            UpdateCarousel(_currentOptionIndex);
        }

        private void PreviousOption()
        {
            if (_currentOptionIndex > 0)
            {
                _currentOptionIndex--;
            }
            else
            {
                _currentOptionIndex = _optionCount - 1;
            }
            
            UpdateCarousel(_currentOptionIndex);
        }
        
        private void UpdateCarousel(int index)
        {
            if (_optionCount == 0)
            {
                return;
            }
            
            onOptionChanged?.Invoke(index);
        }
        
        public void UpdateCarouselDisplay(string value)
        {
            valueText.text = value;
        }
        
         public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerEnter == leftButtonRaycast)
            {
                base.OnPointerClick(eventData);
                PreviousOption();
            }
            else if (eventData.pointerEnter == rightButtonRaycast)
            {
                base.OnPointerClick(eventData);
                NextOption();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerEnter == leftButtonRaycast)
            {
                _currentCarouselLeftSideSequence?.Kill();
                _currentCarouselLeftSideSequence = CarouselLeftHoverSequence();

                if (_leftSideHovered)
                {
                    base.OnPointerEnter(eventData);
                }
                
                _leftSideHovered = true;
            }
            else if (eventData.pointerEnter == rightButtonRaycast)
            {
                _currentCarouselRightSideSequence?.Kill();
                _currentCarouselRightSideSequence = CarouselRightHoverSequence();

                if (_rightSideHovered)
                {
                    base.OnPointerEnter(eventData);
                }
                
                _rightSideHovered = true;
            }
            else 
            {
                base.OnPointerEnter(eventData);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerEnter == leftButtonRaycast)
            {
                _currentCarouselLeftSideSequence?.Kill();
                _currentCarouselLeftSideSequence = CarouselLeftDefaultSequence();
                _leftSideHovered = false;
            }
            else if (eventData.pointerEnter == rightButtonRaycast)
            {
                _currentCarouselRightSideSequence?.Kill();
                _currentCarouselRightSideSequence = CarouselRightDefaultSequence();
                _rightSideHovered = false;
            }
            else
            {
                base.OnPointerExit(eventData);
            }
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerEnter == leftButtonRaycast || eventData.pointerEnter == rightButtonRaycast)
            {
                base.OnPointerDown(eventData);
            }
        }

        protected override void DefaultState()
        {
            base.DefaultState();

            FadeImagesWithAlpha(hoverHighlightImages, defaultContentColor, hoverHighlightAlpha, hoverTransitionDuration);

            valueText.color = defaultContentColor;
            valueText.fontWeight = defaultFontWeight;
            
            _currentCarouselBarSequence?.Kill();
            _currentCarouselBarSequence = CarouselBarToDefaultSequence();
        }
        
        protected override void DefaultStateInstant()
        {
            base.DefaultStateInstant();

            if (!_leftSideHovered)
            {
                RecolorImageWithAlpha(leftArrowImage, defaultColor, 1f);
                RecolorImageWithAlpha(leftArrowBackgroundImage, defaultContentColor, 1f);
            }
            
            if (!_rightSideHovered)
            {
                RecolorImageWithAlpha(rightArrowImage, defaultColor, 1f);
                RecolorImageWithAlpha(rightArrowBackgroundImage, defaultContentColor, 1f);
            }
            
            RecolorImagesWithAlpha(hoverHighlightImages, defaultContentColor, hoverHighlightAlpha);

            if (valueText)
            {
                valueText.color = defaultContentColor;
                valueText.fontWeight = defaultFontWeight;
            }

            if (carouselBarTransform)
            {
                _carouselBarDefaultSize = carouselBarTransform.sizeDelta;
            }
        }

        protected override void HoverState()
        {
            base.HoverState();

            FadeImagesWithAlpha(hoverHighlightImages, hoverContentColor, hoverHighlightAlpha, hoverTransitionDuration);

            valueText.color = hoverContentColor;
            valueText.fontWeight = hoverFontWeight;
            
            _currentCarouselBarSequence?.Kill();
            _currentCarouselBarSequence = CarouselBarHoverSequence();
        }

        private Sequence CarouselBarHoverSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(carouselBarTransform.DOSizeDelta(
                    new Vector2(_carouselBarDefaultSize.x, _carouselBarDefaultSize.y + carouselBarHoverHeightDelta),
                    hoverTransitionDuration).SetEase(Ease.OutCubic));
                
            return sequence;
        }
        
        private Sequence CarouselBarToDefaultSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(carouselBarTransform.DOSizeDelta(
                    _carouselBarDefaultSize, hoverTransitionDuration).SetEase(Ease.OutCubic));
            
            return sequence;
        }
        
        private Sequence CarouselLeftHoverSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(leftHoverHighlightTransform.DOSizeDelta(
                    new Vector2(carouselBarTransform.sizeDelta.x, 0),
                    hoverTransitionDuration*2).SetEase(Ease.OutCubic))
                .Join(_leftArrowTransform.DOScale(1.25f, hoverTransitionDuration*2).SetEase(Ease.OutCubic))
                .Join(FadeImageWithAlpha(leftArrowImage, defaultContentColor, 1f, hoverTransitionDuration))
                .Join(FadeImageWithAlpha(leftArrowBackgroundImage, defaultColor, 1f, hoverTransitionDuration));
            
            return sequence;
        }
        
        private Sequence CarouselRightHoverSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(rightHoverHighlightTransform.DOSizeDelta(
                    new Vector2(carouselBarTransform.sizeDelta.x, 0),
                    hoverTransitionDuration*2).SetEase(Ease.OutCubic))
                .Join(_rightArrowTransform.DOScale(1.25f, hoverTransitionDuration*2).SetEase(Ease.OutCubic))
                .Join(FadeImageWithAlpha(rightArrowImage, defaultContentColor, 1f, hoverTransitionDuration))
                .Join(FadeImageWithAlpha(rightArrowBackgroundImage, defaultColor, 1f, hoverTransitionDuration));
            
            return sequence;
        }
        
        private Sequence CarouselLeftDefaultSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(leftHoverHighlightTransform.DOSizeDelta(
                    new Vector2(0, 0),
                    hoverTransitionDuration*2).SetEase(Ease.InCubic))
                .Join(_leftArrowTransform.DOScale(1f, hoverTransitionDuration*2).SetEase(Ease.InCubic))
                .Join(FadeImageWithAlpha(leftArrowImage, hoverContentColor, 1f, hoverTransitionDuration))
                .Join(FadeImageWithAlpha(leftArrowBackgroundImage, hoverColor, 1f, hoverTransitionDuration));
            
            return sequence;
        }
        
        private Sequence CarouselRightDefaultSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(rightHoverHighlightTransform.DOSizeDelta(
                    new Vector2(0, 0),
                    hoverTransitionDuration*2).SetEase(Ease.InCubic))
                .Join(_rightArrowTransform.DOScale(1f, hoverTransitionDuration*2).SetEase(Ease.InCubic))
                .Join(FadeImageWithAlpha(rightArrowImage, hoverContentColor, 1f, hoverTransitionDuration))
                .Join(FadeImageWithAlpha(rightArrowBackgroundImage, hoverColor, 1f, hoverTransitionDuration));
            
            return sequence;
        }
    }
}


