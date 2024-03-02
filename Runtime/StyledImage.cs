using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace AlexH.AdvancedGUI
{
    public enum StyledImageType
    {
        None,
        BackgroundPanel,
    }
    
    [AddComponentMenu("Advanced GUI/Styled Image")]
    [ExecuteInEditMode]
    public class StyledImage : Image
    {
        public ImageStylingObject overrideStylingObject;
        public StyledImageType styleType;
        public bool onlyApplyColors;
        
        private ImageStylingObject _currentStyle;

        protected override void Start()
        {
            base.Start();
            LoadStyle();
        }

        public void SetStyle(ImageStylingObject style)
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

            if (_currentStyle.sprite)
            {
                sprite = _currentStyle.sprite;
                type = _currentStyle.imageMode;
                pixelsPerUnitMultiplier = _currentStyle.spritePixelPerUnitMultiplier;
            }
            else
            {
                sprite = null;
            }
        }
        
   
        public override void OnBeforeSerialize()
        {

            base.OnBeforeSerialize();
#if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isUpdating) return;
            
            LoadStyle();
            
            EditorUtility.SetDirty(gameObject);
#endif
        }

    }
}



