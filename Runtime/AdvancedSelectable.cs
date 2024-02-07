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
        [SerializeField] private Image hoverImage;
        [SerializeField] private TMP_Text label;

        [Header("Colors")]
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Color pressedColor;
        [SerializeField] private Color disabledColor;
        [Space] 
        [SerializeField] private Color defaultLabelColor;
        [SerializeField] private Color hoverLabelColor;

        [Header("Settings")] 
        [SerializeField] private bool useUniversalHighlight;
        [SerializeField] private bool useManagedColors = true;
        [SerializeField] private bool useManagedTextForLabel = true;

        private AdvancedGUIManager _manager;
        
        
        protected virtual void Start()
        {
            _manager = AdvancedGUIManager.Instance;
            LoadPropertiesFromManager();
            InitializeSelectable();
        }
        
        private void LoadPropertiesFromManager()
        {
            if (!_manager) return;
            
            if (useManagedColors)
            {
                defaultColor =_manager.defaultColor;
                hoverColor = _manager.hoverColor;
                pressedColor = _manager.pressedColor;
                disabledColor = _manager.disabledColor;
            }

            if (useManagedTextForLabel)
            {
                if (_manager.fontAsset) {label.font = _manager.fontAsset;}
                
                label.fontSize = _manager.fontSize;

                defaultLabelColor = _manager.defaultTextColor;
                defaultLabelColor = _manager.hoverTextColor;
            }
        }

        private void InitializeSelectable()
        {
            hoverImage.color = defaultColor;
            label.color = defaultLabelColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHover?.Invoke(this, true);
            hoverImage.DOColor(hoverColor, 0.1f);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            OnHover?.Invoke(this, false);
            hoverImage.DOColor(defaultColor, 0.1f);
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

