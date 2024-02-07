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
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image icon;
        [SerializeField] protected TMP_Text label;
        
        [Header("Settings")]
        [SerializeField] private SelectableSettingsObject settingsObject;
        public bool useUniversalHighlight;
        [SerializeField] private bool useIconInsteadOfLabel;
        
        private Color _defaultColor;
        private Color _hoverColor;
        private Color _pressedColor;
        private Color _disabledColor;
        private Color _defaultLabelColor;
        private Color _hoverLabelColor;

        private RectTransform _rectTransform;

        protected virtual void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
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
            _defaultLabelColor = settingsObject.hoverTextColor;
            
            //frame
            if (settingsObject.useRoundedCorners)
            {
                if (!backgroundImage)
                {
                    backgroundImage = GetComponent<Image>();
                }
                
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
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHover?.Invoke(this, true);
            backgroundImage.DOColor(_hoverColor, 0.1f);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            OnHover?.Invoke(this, false);
            backgroundImage.DOColor(_defaultColor, 0.1f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
        }
    }
}

