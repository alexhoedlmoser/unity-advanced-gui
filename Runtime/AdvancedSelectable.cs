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

        [Header("Settings")] 
        [SerializeField] private bool useUniversalHighlight;
        
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

