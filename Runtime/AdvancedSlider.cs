using System.Globalization;
using System.Linq;
using AlexH.AdvancedGUI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH
{
    [AddComponentMenu("Advanced GUI/Advanced Selectables/Advanced Slider")]
    public class AdvancedSlider : AdvancedSelectable
    {
        [Header("--- Slider Specific ---")]

        [Header("References")] 
        [SerializeField] private Slider slider;
        [FormerlySerializedAs("_sliderFrameImages")] [SerializeField] private Image[] sliderFrameImages;
        [SerializeField] private Image sliderHandleImage;
        [SerializeField] private TMP_Text valueText;

        [Header("Settings")] 
        [SerializeField] private string valueFormat = "0.0";
        [SerializeField] private bool displayValueInPercent;

        [Header("Animation Properties")]
        [SerializeField] private float sliderHoverHeightDelta;
        private Vector2 _sliderDefaultSize;
        
        private RectTransform _sliderTransform;
        private Sequence _currentSliderSequence;
        private Image[] _allSliderImages;

        protected override void Awake()
        {
            _allSliderImages = slider.gameObject.GetComponentsInChildren<Image>();
            _sliderTransform = slider.gameObject.GetComponent<RectTransform>();
            _sliderDefaultSize = _sliderTransform.sizeDelta;
            
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            UpdateValueText(slider.value);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _currentSliderSequence?.Kill();
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
            FadeImagesWithAlpha(sliderFrameImages, defaultContentColor, 1f, transitionDuration);
            FadeImageWithAlpha(sliderHandleImage, defaultColor, 1f, transitionDuration);
            
            valueText.color = defaultContentColor;
            valueText.fontWeight = defaultFontWeight;
            
            _currentSliderSequence?.Kill();
            _currentSliderSequence = SliderToDefaultSequence();
        }

        protected override void DefaultStateInstant()
        {
            base.DefaultStateInstant();
            
            RecolorImagesWithAlpha(sliderFrameImages, defaultContentColor, 1f);
            RecolorImageWithAlpha(sliderHandleImage, defaultColor, 1f);
            
            if (valueText)
            {
                valueText.color = defaultContentColor;
                valueText.fontWeight = defaultFontWeight;
            }
            
            if (!_sliderTransform)
            {
                _sliderTransform = slider.gameObject.GetComponent<RectTransform>();
                _sliderDefaultSize = _sliderTransform.sizeDelta;
            }
        }

        protected override void HoverState()
        {
            base.HoverState();
            
            FadeImagesWithAlpha(sliderFrameImages, hoverContentColor, 1f, transitionDuration);
            FadeImageWithAlpha(sliderHandleImage, hoverColor, 1f, transitionDuration);
            valueText.color = hoverContentColor;
            valueText.fontWeight = hoverFontWeight;
            
            _currentSliderSequence?.Kill();
            _currentSliderSequence = SliderHoverSequence();
        }

        protected override void PressedState()
        {
            base.PressedState();

            RecolorImageWithAlpha(sliderHandleImage, pressedColor, 1f);
        }

        private Sequence SliderHoverSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(_sliderTransform.DOSizeDelta(
                    new Vector2(_sliderDefaultSize.x, _sliderDefaultSize.y + sliderHoverHeightDelta),
                    transitionDuration).SetEase(Ease.OutCubic));
                
            return sequence;
        }
        
        private Sequence SliderToDefaultSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(_sliderTransform.DOSizeDelta(
                    _sliderDefaultSize, transitionDuration).SetEase(Ease.OutCubic));
            
            return sequence;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_allSliderImages.All(image => eventData.pointerEnter != image.gameObject)) return;
            base.OnPointerDown(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_allSliderImages.All(image => eventData.pointerEnter != image.gameObject)) return;
            base.OnPointerClick(eventData);

        }
    }
}
