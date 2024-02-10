using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI{
    public class AdvancedButton : AdvancedSelectable
    {
        [Header("--- Button Specific ---")]
        [Space]
        
        public UnityEvent onClickEvent;

        [Header("Settings")] 
        [SerializeField] private float clickDelay;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            Invoke(nameof(OnClick), clickDelay);
        }

        public void OnClick()
        {
            onClickEvent.Invoke();
        }
    }
}

