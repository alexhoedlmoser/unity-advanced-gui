using DG.Tweening;
using TMPro;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI
{
    [AddComponentMenu("Advanced GUI/Advanced Selectables/Advanced Toggle")]
    public class AdvancedToggle : AdvancedSelectable
    {
        [Header("--- Toggle Specific ---")]
        [Space]
        public UnityEvent<bool> onToggleEvent;
        public bool isToggled;
        
        [Header("References")] 
        [SerializeField] private RectTransform toggleTransform;
        [SerializeField] private Image toggleFrameImage;
        [SerializeField] private Image toggleFillImage;
        [SerializeField] private Image toggleKnobImage;
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private TMP_Text _notificationText;

        [Header("Input")] 
        [SerializeField] private string enabledText = "Enabled";
        [SerializeField] private string disabledText = "Disabled";
        
        [Header("Animation Properties")]
        [SerializeField] private float toggleHoverHeightDelta;

        [Header("Misc")] 
        [SerializeField] private Color _notificationTint;
        
        private Sequence _currentToggleSequence;
        private Sequence _currentToggleFillSequence;
        
        private RectTransform _toggleFillTransform;
        private RectTransform _toggleKnobTransform;
        
        private Vector2 _toggleDefaultSize;
        private Vector2 _toggleKnobSize;
        private Vector2 _toggleFillSize;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClick(); 
        }

        public void OnClick()
        {
            Toggle();
        }

        private void Toggle()
        {
            isToggled = !isToggled;
            onToggleEvent.Invoke(isToggled);
            
            UpdateValueDisplay(isToggled);
            
            _currentToggleFillSequence?.Kill();
            _currentToggleFillSequence = ToggleFillSequence(isToggled);
        }
        
        public void Set(bool value)
        {
            isToggled = value;
            onToggleEvent.Invoke(isToggled);
            
            UpdateValueDisplay(isToggled);
            
            _currentToggleFillSequence?.Kill();
            _currentToggleFillSequence = ToggleFillSequence(isToggled);
        }
        
        public void SetNotification(string notification, Color notificationTint = default)
        {
            if (notificationTint != defaultColor)
            {
                _notificationTint = notificationTint;
            }

            UpdateNotificationDisplay(notification);
        }
        
        protected override void Awake()
        {
            
            _toggleFillTransform = toggleFillImage.GetComponent<RectTransform>();
            _toggleKnobTransform = toggleKnobImage.GetComponent<RectTransform>();
            
            _toggleDefaultSize = toggleTransform.sizeDelta;
            _toggleKnobSize = _toggleKnobTransform.sizeDelta;
            _toggleFillSize = _toggleFillTransform.sizeDelta;
            
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            
            UpdateValueDisplay(isToggled);
            
            _currentToggleFillSequence?.Kill();
            _currentToggleFillSequence = ToggleFillSequence(isToggled);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _currentToggleSequence?.Kill();
            _currentToggleFillSequence?.Kill();
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

        private void UpdateValueDisplay(bool value)
        {
            valueText.text = value ? enabledText : disabledText;
        }

        private void UpdateNotificationDisplay(string notification)
        {
            if (!_notificationText) return;
            
            _notificationText.text = notification;

            if (isHovered || isPressed)
            {
                _notificationText.color = hoverContentColor * _notificationTint;
            }
            else
            {
                _notificationText.color = defaultContentColor * _notificationTint;
            }
        }

        protected override void DefaultState()
        {
            base.DefaultState();
            FadeImageWithAlpha(toggleFrameImage, defaultContentColor, 1f, transitionDuration);
            FadeImageWithAlpha(toggleKnobImage, defaultContentColor, 1f, transitionDuration);
            FadeImageWithAlpha(toggleFillImage, defaultContentColor, 0.5f, transitionDuration);

            valueText.color = defaultContentColor;
            valueText.fontWeight = defaultFontWeight;

            if (_notificationText)
            {
                _notificationText.color = defaultContentColor * _notificationTint;
            }
            
            _currentToggleSequence?.Kill();
            _currentToggleSequence = ToggleToDefaultSequence();
        }
        
        protected override void DefaultStateInstant()
        {
            base.DefaultStateInstant();

            RecolorImageWithAlpha(toggleFrameImage, defaultContentColor, 1f);
            RecolorImageWithAlpha(toggleKnobImage, defaultContentColor, 1f);
            RecolorImageWithAlpha(toggleFillImage, defaultContentColor, 0.5f);
            
            if (valueText)
            {
                valueText.color = defaultContentColor;
                valueText.fontWeight = defaultFontWeight;
            }
            
            if (_notificationText)
            {
                _notificationText.color = defaultContentColor * _notificationTint;
            }

            if (toggleTransform)
            {
                _toggleDefaultSize = toggleTransform.sizeDelta;
            }
        }

        protected override void HoverState()
        {
            base.HoverState();
            
            FadeImageWithAlpha(toggleFrameImage, hoverContentColor, 1f, transitionDuration);
            FadeImageWithAlpha(toggleKnobImage, hoverContentColor, 1f, transitionDuration);
            FadeImageWithAlpha(toggleFillImage, hoverContentColor, 0.75f, transitionDuration);
            
            valueText.color = hoverContentColor;
            valueText.fontWeight = hoverFontWeight;
            
            if (_notificationText)
            {
                _notificationText.color = hoverContentColor * _notificationTint;
            }
            
            _currentToggleSequence?.Kill();
            _currentToggleSequence = ToggleHoverSequence();
        }

        protected override void PressedState()
        {
            base.PressedState();

            //RecolorImageWithAlpha(sliderHandleImage, pressedColor, 1f);
        }
        
        private Sequence ToggleHoverSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(toggleTransform.DOSizeDelta(
                    new Vector2(_toggleDefaultSize.x, _toggleDefaultSize.y + toggleHoverHeightDelta),
                    transitionDuration).SetEase(Ease.OutCubic));
                
            return sequence;
        }
        
        private Sequence ToggleToDefaultSequence()
        {
            Sequence sequence;
            
            sequence = DOTween.Sequence()
                .Append(toggleTransform.DOSizeDelta(
                    _toggleDefaultSize, transitionDuration).SetEase(Ease.OutCubic));
            
            return sequence;
        }

        private Sequence ToggleFillSequence(bool value)
        {
            Sequence sequence;

            if (value)
            {
                sequence = DOTween.Sequence()
                    .Append(_toggleFillTransform
                        .DOSizeDelta(_toggleFillSize, transitionDuration)
                        .SetEase(Ease.OutCubic));
            }
            else
            {
                sequence = DOTween.Sequence()
                    .Append(_toggleFillTransform
                        .DOSizeDelta(new Vector2(_toggleKnobSize.x, _toggleFillSize.y), transitionDuration)
                        .SetEase(Ease.OutCubic));
            }

            return sequence;
        }
    }
}


