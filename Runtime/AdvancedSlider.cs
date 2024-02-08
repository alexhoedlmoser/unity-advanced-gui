using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AlexH.AdvancedGUI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AlexH.Helper;

namespace AlexH
{
    public class AdvancedSlider : AdvancedSelectable
    {
        [Header("--- Slider Specific ---")]

        [Header("References")] 
        [SerializeField] private Slider slider;
        [SerializeField] private Image[] _sliderFrameImages;
        [SerializeField] private Image sliderHandleImage;
        [SerializeField] private TMP_Text valueText;

        [Header("Settings")] 
        [SerializeField] private string valueFormat = "0.0";
        [SerializeField] private bool displayValueInPercent;

        [Header("Animation Properties")]
        [SerializeField] private float sliderHoverHeightDelta;
        private Vector2 _sliderDefaultSize;
        
        private RectTransform _sliderTransform;
        private Sequence _currentSliderHoverSequence;

        protected override void Start()
        {
            //_sliderImages = slider.gameObject.GetComponentsInChildren<Image>();
            _sliderTransform = slider.gameObject.GetComponent<RectTransform>();
            _sliderDefaultSize = _sliderTransform.sizeDelta;
            UpdateValueText(slider.value);
            
            base.Start();
        }

        protected override void LoadStyle()
        {
            base.LoadStyle();

            valueText.font = stylingObject.numbersFontAsset ? stylingObject.numbersFontAsset : stylingObject.textFontAsset;
            valueText.fontSizeMax = stylingObject.fontSize;
        }

        public void UpdateValueText(float value)
        {
            if (displayValueInPercent)
            {
                valueText.text = (slider.normalizedValue * 100f).ToString(valueFormat, CultureInfo.InvariantCulture) + "%";
            }
            else
            {
                valueText.text = value.ToString(valueFormat, CultureInfo.InvariantCulture);
            }
           
        }

        protected override void DefaultState()
        {
            base.DefaultState();
            RecolorImagesWithAlpha(_sliderFrameImages, defaultContentColor, 1f);
            RecolorImageWithAlpha(sliderHandleImage, defaultColor, 1f);
            
            valueText.color = defaultContentColor;
            valueText.fontWeight = defaultFontWeight;
        }

        protected override void HoverState(bool hover)
        {
            base.HoverState(hover);

            if (hover)
            {
                RecolorImagesWithAlpha(_sliderFrameImages, hoverContentColor, 1f);
                RecolorImageWithAlpha(sliderHandleImage, hoverColor, 1f);
                valueText.color = hoverContentColor;
                valueText.fontWeight = hoverFontWeight;
            }
            else
            {
                DefaultState();
            }
            
            _currentSliderHoverSequence?.Kill();
            _currentSliderHoverSequence = SliderHoverSequence(hover);
        }

        private Sequence SliderHoverSequence(bool hover)
        {
            Sequence sequence;
            
            if (hover)
            {
                sequence = DOTween.Sequence()
                    .Append(_sliderTransform.DOSizeDelta(
                        new Vector2(_sliderDefaultSize.x, _sliderDefaultSize.y + sliderHoverHeightDelta),
                        hoverTransitionDuration).SetEase(Ease.OutCubic));
            }
            else
            {
                sequence = DOTween.Sequence()
                    .Append(_sliderTransform.DOSizeDelta(
                        _sliderDefaultSize, hoverTransitionDuration).SetEase(Ease.OutCubic));

            }

            return sequence;
        }
        
    }
}
