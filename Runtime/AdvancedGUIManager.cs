using System.Collections;
using System.Collections.Generic;
using LRS.Singleton;
using TMPro;
using UnityEngine;

namespace AlexH.AdvancedGUI
{
    public class AdvancedGUIManager : Singleton<AdvancedGUIManager>
    {
        [Header("Selectables")]
        public Color defaultColor = Color.gray;
        public Color hoverColor = Color.white;
        public Color pressedColor = Color.yellow;
        public Color disabledColor = Color.black;

        [Header("Text")] 
        public TMP_FontAsset fontAsset;
        public float fontSize = 36f;
        public Color defaultTextColor = Color.black;
        public Color hoverTextColor = Color.black;

        [Header("Transitions")] 
        public TransitionType transitionType;
        public float transitionDuration;
       
    }
}
