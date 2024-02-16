using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AlexH.AdvancedGUI.Helper;


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
        public Color pressedContentColor = Color.white;
        
        [Header("Frame")]
        public Sprite sprite;
        public Image.Type imageMode = Image.Type.Tiled;
        public int spritePixelPerUnitMultiplier = 3;

        [Header("Text")] 
        public TMP_FontAsset textFontAsset;
        public TMP_FontAsset numbersFontAsset;
        public float fontSize = 36f;
        public FontWeight defaultFontWeight;
        public FontWeight hoverFontWeight;
        public FontWeight selectedFontWeight;
        public FontStyles defaultFontStyle;
        public FontStyles hoverFontStyle;
        public FontStyles selectedFontStyle;
        public float defaultCharacterSpacing;
        
        [Header("Animation Properties")]
        public TransitionType transitionType;
        [Space]
        public float hoverSizeDelta;
        public float hoverTransitionDuration = 0.1f;
        public float hoverLabelCharacterSpacing = 15f;
        public float pressedSizeDelta = 50f;
    }
}
