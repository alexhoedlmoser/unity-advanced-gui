using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AlexH.AdvancedGUI
{
    [ExecuteInEditMode]
    public class AdvancedGUIManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        public StyleThemeObject theme;

        private StyledText[] _styledTexts;
        private AdvancedButton[] _advancedButtons;
        private AdvancedNavButton[] _advancedNavButtons;
        private AdvancedSlider[] _advancedSliders;
        
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isUpdating) return;
            
            FindStyledObjects();
            ApplyTheme(theme);
            
            EditorUtility.SetDirty(gameObject);
#endif
        }

        public void OnAfterDeserialize()
        {
        }

        private void Start()
        {
            FindStyledObjects();
            ApplyTheme(theme);
        }

        private void ApplyTheme(StyleThemeObject styleTheme) 
        {
            if (!styleTheme)
            {
                return;
            }
            
            foreach (StyledText styledText in _styledTexts)
            {
                switch (styledText.type)
                {
                    case StyledTextType.Paragraph:
                        styledText.SetStyle(styleTheme.paragraphTextStyle);
                        break;
                    case StyledTextType.Title:
                        styledText.SetStyle(styleTheme.titleTextStyle);
                        break;
                    case StyledTextType.Headline01:
                        styledText.SetStyle(styleTheme.headline01TextStyle);
                        break;
                    case StyledTextType.Headline02:
                        styledText.SetStyle(styleTheme.headline02TextStyle);
                        break;
                    case StyledTextType.Headline03:
                        styledText.SetStyle(styleTheme.headline03TextStyle);
                        break;
                    case StyledTextType.Breadcrumb:
                        styledText.SetStyle(styleTheme.breadcrumbTextStyle);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (AdvancedButton button in _advancedButtons)
            {
                button.SetStyle(theme.buttonStyle);
            }
            
            foreach (AdvancedNavButton button in _advancedNavButtons)
            {
                button.SetStyle(theme.navButtonStyle);
            }

            foreach (AdvancedSlider slider in _advancedSliders)
            {
                slider.SetStyle(theme.sliderStyle);
            }
        }

        private void FindStyledObjects()
        {
            _styledTexts = FindObjectsByType<StyledText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedButtons = FindObjectsByType<AdvancedButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedNavButtons = FindObjectsByType<AdvancedNavButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedSliders = FindObjectsByType<AdvancedSlider>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }
    }
    
    
}
