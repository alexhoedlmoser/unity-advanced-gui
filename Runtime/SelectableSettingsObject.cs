using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace AlexH.AdvancedGUI
{
    [CreateAssetMenu(fileName = "New Selectable Settings", menuName = "AdvancedGUI/Settings/SelectableSettings")]
    public class SelectableSettingsObject : ScriptableObject
    {
        [Header("Colors")]
        public Color defaultColor = Color.gray;
        public Color hoverColor = Color.white;
        public Color pressedColor = Color.cyan;
        public Color disabledColor = Color.black;
        
        [Header("Frame")]
        public Sprite defaultSprite;
        public bool useRoundedCorners = true;
        public Sprite roundedCornersSprite;
        [Tooltip("Lower value means higher corner roundness")]
        public int cornerRoundness = 3;

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
