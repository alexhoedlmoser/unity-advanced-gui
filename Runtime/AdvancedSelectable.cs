using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static AlexH.Helper;

namespace AlexH.AdvancedGUI
{
    public class AdvancedSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action<AdvancedSelectable, bool> OnHover;

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
        protected FontWeight clickedFontWeight;

        protected float defaultLabelCharacterSpacing;
        protected float hoverLabelCharacterSpacing;
        
        protected float hoverSizeDelta;
        protected float hoverTransitionDuration;

        #endregion
        
        protected RectTransform rectTransform;
        protected RectTransform backgroundTransform;
        private Vector2 _defaultSize;
        
        private Sequence _currentHoverSequence;
        private Coroutine _characterSpacingTween;

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
            clickedFontWeight = stylingObject.clickedFontWeight;
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

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            OnHover?.Invoke(this, true);
            HoverState(true);
        }
        
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            OnHover?.Invoke(this, false);
            HoverState(false);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
        }

        protected virtual void DefaultState()
        {
            backgroundImage.color = defaultColor;
            
            if (useIconInsteadOfLabel)
            {
                icon.color = defaultContentColor;
            }
            else
            {
                label.color = defaultContentColor;
                label.characterSpacing = defaultLabelCharacterSpacing;
                label.fontWeight = defaultFontWeight;
            }
        }

        protected virtual void HoverState(bool hover)
        {
            if (hover)
            {
                icon.color =  hoverContentColor;
                label.color = hoverContentColor;
                label.fontWeight = hoverFontWeight;
            }
            else
            {
                DefaultState();
            }
            
            _currentHoverSequence?.Kill();
            _currentHoverSequence = HoverSequence(hover);
        }
        
        protected Sequence HoverSequence(bool hover)
        {
            Sequence sequence;
            if (hover)
            {
                sequence =  DOTween.Sequence()
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
                sequence =  DOTween.Sequence()
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
    }
}

