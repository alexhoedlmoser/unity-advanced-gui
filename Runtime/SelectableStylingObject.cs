using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        public int spritePixelPerUnitMultiplier = 6;

        [Header("Text")] 
        public TMP_FontAsset textFontAsset;
        public TMP_FontAsset numbersFontAsset;
        public float fontSize = 22f;
        public FontWeight defaultFontWeight = FontWeight.Regular;
        public FontWeight hoverFontWeight = FontWeight.Black;
        public FontWeight selectedFontWeight = FontWeight.Black;
        public FontStyles defaultFontStyle = FontStyles.Normal;
        public FontStyles hoverFontStyle = FontStyles.Italic;
        public FontStyles selectedFontStyle = FontStyles.Normal;
        public float defaultCharacterSpacing = 5f;
        
        [Header("Animation Properties")]
        public TransitionType transitionType;
        [Space]
        public float hoverSizeDelta = -10f;
        public float hoverLabelCharacterSpacing = 15f;
        public float pressedSizeDelta = 50f;
        public float transitionDuration = 0.1f;
    }
}
