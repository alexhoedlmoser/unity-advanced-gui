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
        protected FontWeight selectedFontWeight;
        protected FontStyles defaultFontStyle;
        protected FontStyles hoverFontStyle;
        protected FontStyles selectedFontStyle;

        protected float defaultLabelCharacterSpacing;
        protected float hoverLabelCharacterSpacing;
        
        protected float hoverSizeDelta;
        protected float hoverTransitionDuration;
        protected float pressedSizeDelta;

        #endregion
        
        protected RectTransform rectTransform;
        protected RectTransform backgroundTransform;
        private Vector2 _defaultSize;
        
        protected Sequence currentSequence;
        private Coroutine _characterSpacingTween;

        protected SelectableStylingObject currentStyle;

        protected bool isHovered;
        protected bool isPressed;
        protected bool isSelected;

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
            
            LoadStyle();
            InitializeSelectable();
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnDestroy()
        {
            currentSequence?.Kill();
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
            pressedSizeDelta = currentStyle.pressedSizeDelta;
            #endregion

            #region Label
            if (currentStyle.textFontAsset) {label.font = currentStyle.textFontAsset;}
            label.fontSizeMax = currentStyle.fontSize;
            defaultFontWeight = currentStyle.defaultFontWeight;
            hoverFontWeight = currentStyle.hoverFontWeight;
            selectedFontWeight = currentStyle.selectedFontWeight;
            defaultFontStyle = currentStyle.defaultFontStyle;
            hoverFontStyle = currentStyle.hoverFontStyle;
            selectedFontStyle = currentStyle.selectedFontStyle;
            
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
            
            DefaultStateInstant();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            OnHover?.Invoke(this, true);

            isHovered = true;
            
            HoverState();
            
            if (isPressed)
            {
                OnPressed?.Invoke(this, true);
                PressedState();
            }
        }
        
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            OnHover?.Invoke(this, false);
            isHovered = false;

            if (isSelected)
            {
                SelectedState();
            }
            else
            {
                DefaultState();
            }

        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            
        }
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            OnPressed?.Invoke(this, true);

            isPressed = true;
            
            PressedState();
           
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            //if (!isPressed) return;
            
            OnPressed?.Invoke(this, false);
            isPressed = false;

            if (isHovered)
            {
                HoverState();
            }
            else if (isSelected)
            {
                SelectedState();
            }
            else
            {
                DefaultState();
            }
        }

        protected virtual void DefaultState()
        {
            icon.color = defaultContentColor;
            label.color = defaultContentColor;
            
            label.fontWeight = defaultFontWeight;
            label.fontStyle = defaultFontStyle;
            
            currentSequence?.Kill();
            currentSequence = ToDefaultSequence(hoverTransitionDuration);
        }
        
        protected virtual void DefaultStateInstant()
        {
            icon.color = defaultContentColor;
            label.color = defaultContentColor;
            
            label.fontWeight = defaultFontWeight;
            label.fontStyle = defaultFontStyle;

            backgroundImage.color = defaultColor;
            label.characterSpacing = defaultLabelCharacterSpacing;
        }

        protected virtual void HoverState()
        {
            if (isSelected)
            {
                icon.color =  pressedContentColor;
                label.color = pressedContentColor;
            }
            else
            {
                icon.color =  hoverContentColor;
                label.color = hoverContentColor;
            }

            label.fontWeight = hoverFontWeight;
            label.fontStyle = hoverFontStyle;
            
            currentSequence?.Kill();
            currentSequence = ToHoverSequence(hoverTransitionDuration, isSelected);
        }

        protected virtual void PressedState()
        {
            label.color = pressedContentColor;
            icon.color = pressedContentColor;
            
            label.fontWeight = hoverFontWeight;

            currentSequence?.Kill();
            currentSequence = ToPressedSequence(0);
        }

        protected virtual void SelectedState()
        {
            label.color = pressedContentColor;
            icon.color = pressedContentColor;
            
            label.fontWeight = selectedFontWeight;
            label.fontStyle = selectedFontStyle;
            
            currentSequence?.Kill();
            currentSequence = ToSelectedSequence(hoverTransitionDuration);
        }

        protected Sequence ToHoverSequence(float duration, bool whileSelected = false)
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                    .Append(backgroundTransform.DOSizeDelta(GetPaddedSize(_defaultSize, hoverSizeDelta), duration).SetEase(Ease.OutCubic))
                    .Join(backgroundImage.DOColor(whileSelected? pressedColor : hoverColor, duration).SetEase(Ease.Linear));
                
            if (_characterSpacingTween != null)
            {
                StopCoroutine(_characterSpacingTween);
            }
            _characterSpacingTween = StartCoroutine(TweenCharacterSpacing(label, hoverLabelCharacterSpacing, hoverTransitionDuration));

            return sequence;
        }
    
        protected Sequence ToDefaultSequence(float duration)
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(backgroundTransform.DOSizeDelta(_defaultSize, duration).SetEase(Ease.OutCubic))
                .Join(backgroundImage.DOColor(defaultColor, duration).SetEase(Ease.Linear));

            if (_characterSpacingTween != null)
            {
                StopCoroutine(_characterSpacingTween);
            }
            _characterSpacingTween = StartCoroutine(TweenCharacterSpacing(label, defaultLabelCharacterSpacing, hoverTransitionDuration));

            return sequence;
        }

        protected Sequence ToPressedSequence(float duration)
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(backgroundTransform
                    .DOSizeDelta(GetPaddedSize(_defaultSize, pressedSizeDelta), duration)
                    .SetEase(Ease.OutCubic))
                .Join(backgroundImage.DOColor(pressedColor, duration).SetEase(Ease.Linear));

            return sequence;
        }
        
        protected Sequence ToSelectedSequence(float duration)
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(backgroundTransform
                    .DOSizeDelta(_defaultSize, duration)
                    .SetEase(Ease.OutCubic))
                .Join(backgroundImage.DOColor(pressedColor, duration).SetEase(Ease.Linear));

            return sequence;
        }
    }
}

