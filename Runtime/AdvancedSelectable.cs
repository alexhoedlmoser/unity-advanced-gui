using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.Serialization;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI
{
    public class AdvancedSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<AdvancedSelectable, bool> OnHover;
        public event Action<AdvancedSelectable, bool> OnPressed;

        [Header("Input")] 
        [SerializeField] private string labelText;
        [SerializeField] private Sprite iconSprite;
        
        [Header("References")] 
        [SerializeField] protected Image backgroundImage;
        [SerializeField] protected Image icon;
        [SerializeField] protected TMP_Text label;
        
        [Header("Settings")]
        [SerializeField] protected SelectableStylingObject overrideStylingObject;
        public bool useUniversalHighlight;
        [SerializeField] protected bool useIconInsteadOfLabel;

        #region Style Properties

        protected Color defaultColor;
        protected Color hoverColor;
        protected Color pressedColor;
        protected Color disabledColor;
        protected Color defaultContentColor;
        protected Color hoverContentColor;
        protected Color pressedContentColor;

        protected FontWeight defaultFontWeight;
        protected FontWeight hoverFontWeight;
        protected FontStyles defaultFontStyle;
        protected FontStyles hoverFontStyle;

        protected float defaultLabelCharacterSpacing;
        protected float hoverLabelCharacterSpacing;
        
        protected float hoverSizeDelta;
        protected float hoverTransitionDuration;
        protected float clickedSizeDelta = 50f;
        protected float clickedTransitionDuration = 0.05f;

        #endregion
        
        protected RectTransform rectTransform;
        protected RectTransform backgroundTransform;
        private Vector2 _defaultSize;
        
        private Sequence _currentSequence;
        private Coroutine _characterSpacingTween;

        protected SelectableStylingObject currentStyle;

        private bool _isPressed;

        #region Getter

        public Color GetDefaultColor()
        {
            return defaultColor;
        }
        
        public Color GetHoverColor()
        {
            return hoverColor;
        }
        
        public Color GetPressedColor()
        {
            return pressedColor;
        }

        #endregion
        
        public void OnBeforeSerialize()
        {
            LoadStyle();
            InitializeSelectable();
            EditorUtility.SetDirty(gameObject);
        }

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            backgroundTransform = backgroundImage.GetComponent<RectTransform>();
            _defaultSize = backgroundTransform.sizeDelta;
        }

        protected virtual void Start()
        {
            LoadStyle();
            InitializeSelectable();
        }
        
        protected virtual void LoadStyle()
        {
            #region Frame

            if (overrideStylingObject)
            {
                currentStyle = overrideStylingObject;
            }
            
            if (!currentStyle)
            {
                return;
            }

            if (currentStyle.useRoundedCorners)
            {
                backgroundImage.sprite = currentStyle.roundedCornersSprite;
                backgroundImage.type = Image.Type.Tiled;
                backgroundImage.pixelsPerUnitMultiplier = currentStyle.GetPixelMultiplierForRoundness();
            }
            else
            {
                backgroundImage.sprite = currentStyle.defaultSprite;
            }
            
            defaultColor = currentStyle.defaultColor;
            hoverColor = currentStyle.hoverColor;
            pressedColor = currentStyle.pressedColor;
            disabledColor = currentStyle.disabledColor;
            
            hoverSizeDelta = currentStyle.hoverSizeDelta;
            hoverTransitionDuration = currentStyle.hoverTransitionDuration;
            #endregion

            #region Label
            if (currentStyle.textFontAsset) {label.font = currentStyle.textFontAsset;}
            label.fontSizeMax = currentStyle.fontSize;
            defaultFontWeight = currentStyle.defaultFontWeight;
            hoverFontWeight = currentStyle.hoverFontWeight;
            defaultFontStyle = currentStyle.defaultFontStyle;
            hoverFontStyle = currentStyle.hoverFontStyle;
            defaultLabelCharacterSpacing = currentStyle.defaultCharacterSpacing;
            hoverLabelCharacterSpacing = currentStyle.hoverLabelCharacterSpacing;
            
            defaultContentColor = currentStyle.defaultContentColor;
            hoverContentColor = currentStyle.hoverContentColor;
            pressedContentColor = currentStyle.pressedContentColor;
            #endregion
        }

        public void SetStyle(SelectableStylingObject stylingObject)
        {
            currentStyle = stylingObject;
            LoadStyle();
            InitializeSelectable();
        }

        protected virtual void InitializeSelectable()
        {
            // icon or label
            label.gameObject.SetActive(!useIconInsteadOfLabel);
            icon.gameObject.SetActive(useIconInsteadOfLabel);

            label.text = labelText;
            icon.sprite = iconSprite;
            
            DefaultState();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHover?.Invoke(this, true);
            HoverState(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            OnHover?.Invoke(this, false);
           
            if (_isPressed)
            {
                PressedState(false);
                _isPressed = false;
            }
           
            HoverState(false);
            
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            
        }
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            OnPressed?.Invoke(this, true);

            PressedState(true);
            _isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isPressed) return;
            
            OnPressed?.Invoke(this, false);
            PressedState(false);
            _isPressed = false;

        }

        protected virtual void DefaultState()
        {
            backgroundImage.color = defaultColor;
            
            icon.color = defaultContentColor;
            
            label.color = defaultContentColor;
            label.characterSpacing = defaultLabelCharacterSpacing;
            label.fontWeight = defaultFontWeight;
            label.fontStyle = defaultFontStyle;
            
        }

        protected virtual void HoverState(bool hover)
        {
            if (hover)
            {
                icon.color =  hoverContentColor;
                label.color = hoverContentColor;
                label.fontWeight = hoverFontWeight;
                label.fontStyle = hoverFontStyle;
            }
            else
            {
                DefaultState();
            }
            
            _currentSequence?.Kill();
            _currentSequence = HoverSequence(hover);
        }

        protected virtual void PressedState(bool pressed)
        {

            if (pressed)
            {
                label.color = pressedContentColor;
                _currentSequence?.Kill();
                _currentSequence = PressedSequence();
            }
            else
            {
                HoverState(true);
            }
        }
        
        protected Sequence HoverSequence(bool hover)
        {
            Sequence sequence = DOTween.Sequence();
            if (hover)
            {
                sequence
                    .Append(backgroundTransform.DOSizeDelta(GetPaddedSize(_defaultSize, hoverSizeDelta), hoverTransitionDuration).SetEase(Ease.OutCubic))
                    .Join(backgroundImage.DOColor(hoverColor, hoverTransitionDuration).SetEase(Ease.Linear));

                // sequence.Join(useIconInsteadOfLabel
                //     ? icon.DOColor(_hoverLabelColor, hoverTransitionDuration).SetEase(Ease.Linear)
                //     : label.DOColor(_hoverLabelColor, hoverTransitionDuration).SetEase(Ease.Linear));

                if (_characterSpacingTween != null)
                {
                    StopCoroutine(_characterSpacingTween);
                }
                _characterSpacingTween = StartCoroutine(TweenCharacterSpacing(label, hoverLabelCharacterSpacing, hoverTransitionDuration));
            }
            else
            {
                sequence
                    .Append(backgroundTransform.DOSizeDelta(_defaultSize, hoverTransitionDuration).SetEase(Ease.OutCubic))
                    .Join(backgroundImage.DOColor(defaultColor, hoverTransitionDuration).SetEase(Ease.Linear));

                // sequence.Join(useIconInsteadOfLabel
                //     ? icon.DOColor(_defaultLabelColor, hoverTransitionDuration).SetEase(Ease.Linear)
                //     : label.DOColor(_defaultLabelColor, hoverTransitionDuration).SetEase(Ease.Linear));

                if (_characterSpacingTween != null)
                {
                    StopCoroutine(_characterSpacingTween);
                }
                _characterSpacingTween = StartCoroutine(TweenCharacterSpacing(label, defaultLabelCharacterSpacing, hoverTransitionDuration));
            }

            return sequence;
        }

        private Sequence PressedSequence()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(backgroundTransform
                    .DOSizeDelta(GetPaddedSize(_defaultSize, clickedSizeDelta), clickedTransitionDuration/2)
                    .SetEase(Ease.OutCubic))
                .Join(backgroundImage.DOColor(pressedColor, clickedTransitionDuration/2).SetEase(Ease.Linear));

            /*if (pressed)
            {
                
            }

            else
            {
                sequence
                    .Append(backgroundTransform
                        .DOSizeDelta(GetPaddedSize(_defaultSize, hoverSizeDelta), clickedTransitionDuration*2)
                        .SetEase(Ease.InQuart))
                    .Join(backgroundImage.DOColor(hoverColor, clickedTransitionDuration*2).SetEase(Ease.Linear));
            }*/

            return sequence;
        }
    }
}

