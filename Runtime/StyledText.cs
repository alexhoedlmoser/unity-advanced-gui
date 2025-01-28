using AlexH.AdvancedGUI;
using UnityEditor;
using UnityEngine;

namespace AlexH
{
    public enum StyledTextType
    {
        Paragraph,
        Title,
        Headline01,
        Headline02,
        Headline03,
        Breadcrumb,
        Value
    }
    
    [AddComponentMenu("Advanced GUI/Styled Text")]
    [ExecuteInEditMode]
    public class StyledText : TMPro.TextMeshProUGUI, ISerializationCallbackReceiver
    {
        public TextStylingObject overrideStylingObject;
        public bool useAutoSize;
        
        public StyledTextType type;
        public bool onlyApplyColors;
        
        private TextStylingObject _currentStyle;
        
        protected override void Start()
        {
            base.Start();
            LoadStyle();
        }

        public void SetStyle(TextStylingObject style)
        {
            _currentStyle = style;
            LoadStyle();
        }

        private void LoadStyle()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            if (overrideStylingObject)
            {
                _currentStyle = overrideStylingObject;
            }
            
            if (!_currentStyle)
            {
                return;
            }
            
            color = _currentStyle.color;
            
            if (onlyApplyColors)
            {
                return;
            }
            
            enableAutoSizing = useAutoSize;

            if (useAutoSize)
            {
                fontSizeMax = _currentStyle.fontSize;
                fontSizeMin = 12;
            }
            else
            {
                fontSize = _currentStyle.fontSize;
            }
           
            font = _currentStyle.textFontAsset;
            fontStyle = _currentStyle.fontStyle;
            
            characterSpacing = _currentStyle.characterSpacing;
            fontWeight = _currentStyle.fontWeight;
        }
        
        

        public void OnBeforeSerialize()
        {
            
#if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isUpdating) return;
            
            LoadStyle();
            
            EditorUtility.SetDirty(gameObject);
#endif

        }

        public void OnAfterDeserialize()
        {
        }
        

    }
}
