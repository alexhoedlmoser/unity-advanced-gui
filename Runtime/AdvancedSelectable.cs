using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace AlexH.AdvancedGUI
{
    public class AdvancedSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action<AdvancedSelectable, bool> OnHover;
        
        [Header("References")] 
        [SerializeField] protected Image backgroundImage;
        [SerializeField] protected Image icon;
        [SerializeField] protected TMP_Text label;
        
        [Header("Settings")]
        [SerializeField] private SelectableSettingsObject settingsObject;
        public bool useUniversalHighlight;
        [SerializeField] protected bool useIconInsteadOfLabel;
        
        protected Color _defaultColor;
        protected Color _hoverColor;
        protected Color _pressedColor;
        protected Color _disabledColor;
        protected Color _defaultLabelColor;
        protected Color _hoverLabelColor;

        protected float _defaultLabelCharacterSpacing;

        protected RectTransform _rectTransform;
        protected RectTransform _backgroundTransform;

        protected virtual void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _backgroundTransform = backgroundImage.GetComponent<RectTransform>();
            
            LoadSettings();
            InitializeSelectable();
        }
        
        private void LoadSettings()
        {
            //colors
            _defaultColor = settingsObject.defaultColor;
            _hoverColor = settingsObject.hoverColor;
            _pressedColor = settingsObject.pressedColor;
            _disabledColor = settingsObject.disabledColor;

            //text label
            if (settingsObject.fontAsset) {label.font = settingsObject.fontAsset;}
            label.fontSize = settingsObject.fontSize;
            _defaultLabelColor = settingsObject.defaultTextColor;
            _hoverLabelColor = settingsObject.hoverTextColor;

            _defaultLabelCharacterSpacing = settingsObject.characterSpacing;
            
            //frame
            if (settingsObject.useRoundedCorners)
            {
                backgroundImage.sprite = settingsObject.roundedCornersSprite;
                backgroundImage.type = Image.Type.Tiled;
                backgroundImage.pixelsPerUnitMultiplier = settingsObject.cornerRoundness;
            }
            else
            {
                backgroundImage.sprite = settingsObject.defaultSprite;
            }
            
            // icon or label
            label.gameObject.SetActive(!useIconInsteadOfLabel);
            icon.gameObject.SetActive(useIconInsteadOfLabel);
        }

        protected virtual void InitializeSelectable()
        {
            backgroundImage.color = _defaultColor;
            
            if (useIconInsteadOfLabel)
            {
                icon.color = _defaultLabelColor;
            }
            else
            {
                label.color = _defaultLabelColor;
                label.characterSpacing = _defaultLabelCharacterSpacing;
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            OnHover?.Invoke(this, true);
        }
        
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            OnHover?.Invoke(this, false);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}

