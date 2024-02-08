using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AlexH.Helper;

namespace AlexH.AdvancedGUI{
    public class AdvancedButton : AdvancedSelectable
    {
        [Header("--- Button Specific ---")]

        [Header("References")] 
        [SerializeField] private Image clickImage;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

       
    }
}

