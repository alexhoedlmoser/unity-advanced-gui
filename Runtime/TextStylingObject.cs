using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AlexH.AdvancedGUI
{
    [CreateAssetMenu(fileName = "New Text Style", menuName = "AdvancedGUI/Styles/Text Style")]
    public class TextStylingObject : ScriptableObject
    {
        [Header("Colors")]
        public Color color = Color.white;

        [Header("Text")] 
        public TMP_FontAsset textFontAsset;
        public float fontSize = 20f;
        public FontWeight fontWeight = FontWeight.Regular;
        public FontStyles fontStyle = FontStyles.Normal;
        public float characterSpacing;
    }
}
