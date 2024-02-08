using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static AlexH.Helper;


namespace AlexH.AdvancedGUI
{
    [CreateAssetMenu(fileName = "New Selectable Style", menuName = "AdvancedGUI/Styles/Selectable Style")]
    public class SelectableStylingObject : ScriptableObject
    {
        [Header("Colors")]
        public Color defaultColor = Color.gray;
        public Color hoverColor = Color.white;
        public Color pressedColor = Color.cyan;
        public Color disabledColor = Color.black;
        
        public Color defaultContentColor = Color.black;
        public Color hoverContentColor = Color.black;
        
        [Header("Frame")]
        public Sprite defaultSprite;
        public bool useRoundedCorners = true;
        public Sprite roundedCornersSprite;
        [Range(1, 10)]
        public int cornerRoundness = 3;

        [Header("Text")] 
        public TMP_FontAsset textFontAsset;
        public TMP_FontAsset numbersFontAsset;
        public float fontSize = 36f;
        public FontWeight defaultFontWeight;
        public FontWeight hoverFontWeight;
        public FontWeight clickedFontWeight;
        public float defaultCharacterSpacing;
        
        [Header("Animation Properties")]
        public TransitionType transitionType;
        [Space]
        public float hoverSizeDelta;
        public float hoverTransitionDuration = 0.1f;
        public float hoverLabelCharacterSpacing = 15f;

        public float GetPixelMultiplierForRoundness()
        {
           return RemapRange(cornerRoundness, 1, 10, 20, 3);
        }
    }
}
