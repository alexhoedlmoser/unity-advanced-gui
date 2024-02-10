using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
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
        [SerializeField] protected SelectableStylingObject stylingObject;
        public bool useUniversalHighlight;
        [SerializeField] protected bool useIconInsteadOfLabel;

        #region Style Properties

        protected Color defaultColor;
        protected Color hoverColor;
        protected Color pressedColor;
        protected Color disabledColor;
        protected Color defaultContentColor;
        protected Color hoverContentColor;

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

        protected virtual void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            backgroundTransform = backgroundImage.GetComponent<RectTransform>();
            _defaultSize = backgroundTransform.sizeDelta;
            
            LoadStyle();
            InitializeSelectable();
        }
        
        protected virtual void LoadStyle()
        {
            #region Frame

            if (!stylingObject)
            {
                return;
            }
            
            if (stylingObject.useRoundedCorners)
            {
                backgroundImage.sprite = stylingObject.roundedCornersSprite;
                backgroundImage.type = Image.Type.Tiled;
                backgroundImage.pixelsPerUnitMultiplier = stylingObject.GetPixelMultiplierForRoundness();
            }
            else
            {
                backgroundImage.sprite = stylingObject.defaultSprite;
            }
            
            defaultColor = stylingObject.defaultColor;
            hoverColor = stylingObject.hoverColor;
            pressedColor = stylingObject.pressedColor;
            disabledColor = stylingObject.disabledColor;
            
            hoverSizeDelta = stylingObject.hoverSizeDelta;
            hoverTransitionDuration = stylingObject.hoverTransitionDuration;
            #endregion

            #region Label
            if (stylingObject.textFontAsset) {label.font = stylingObject.textFontAsset;}
            label.fontSizeMax = stylingObject.fontSize;
            defaultFontWeight = stylingObject.defaultFontWeight;
            hoverFontWeight = stylingObject.hoverFontWeight;
            defaultFontStyle = stylingObject.defaultFontStyle;
            hoverFontStyle = stylingObject.hoverFontStyle;
            defaultLabelCharacterSpacing = stylingObject.defaultCharacterSpacing;
            hoverLabelCharacterSpacing = stylingObject.hoverLabelCharacterSpacing;
            
            defaultContentColor = stylingObject.defaultContentColor;
            hoverContentColor = stylingObject.hoverContentColor;
            #endregion
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
                ClickedState(false);
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

            ClickedState(true);
            _isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isPressed) return;
            
            OnPressed?.Invoke(this, false);
            ClickedState(false);
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

        protected virtual void ClickedState(bool clicked)
        {
            _currentSequence?.Kill();
            _currentSequence = ClickedSequence(clicked);
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

        private Sequence ClickedSequence(bool clicked)
        {
            Sequence sequence = DOTween.Sequence();

            if (clicked)
            {
                sequence
                    .Append(backgroundTransform
                        .DOSizeDelta(GetPaddedSize(_defaultSize, clickedSizeDelta), clickedTransitionDuration/2)
                        .SetEase(Ease.OutCubic))
                    .Join(backgroundImage.DOColor(pressedColor, clickedTransitionDuration/2).SetEase(Ease.Linear));
            }

            else
            {
                sequence
                    .Append(backgroundTransform
                        .DOSizeDelta(GetPaddedSize(_defaultSize, hoverSizeDelta), clickedTransitionDuration*2)
                        .SetEase(Ease.InQuart))
                    .Join(backgroundImage.DOColor(hoverColor, clickedTransitionDuration*2).SetEase(Ease.Linear));
            }

            return sequence;
        }
    }
}

