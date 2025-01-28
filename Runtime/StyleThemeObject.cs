using UnityEngine;

namespace AlexH.AdvancedGUI
{
    
    [CreateAssetMenu(fileName = "New Style Theme", menuName = "AdvancedGUI/Themes/Style Theme")]
    public class StyleThemeObject : ScriptableObject
    {
        [Header("Selectables")] 
        public SelectableStylingObject buttonStyle;
        public SelectableStylingObject navButtonStyle;
        public SelectableStylingObject settingsButtonStyle;

        [Header("Text")] 
        public TextStylingObject titleTextStyle;
        public TextStylingObject headline01TextStyle;
        public TextStylingObject headline02TextStyle;
        public TextStylingObject headline03TextStyle;
        public TextStylingObject breadcrumbTextStyle;
        public TextStylingObject paragraphTextStyle;
        public TextStylingObject valueTextStyle;

        [Header("Images")]
        public ImageStylingObject backgroundImageStyle;

        [Header("Transitions")] 
        public TransitionType transitionType;
        public float transitionDuration;
    }
}
