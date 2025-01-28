using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AlexH.AdvancedGUI
{
    [AddComponentMenu("Advanced GUI/Advanced GUI Manager")]
    [ExecuteInEditMode]
    public class AdvancedGUIManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        public StyleThemeObject theme;

        private StyledText[] _styledTexts;
        private StyledImage[] _styledImages;
        private AdvancedButton[] _advancedButtons;
        private AdvancedNavButton[] _advancedNavButtons;
        private AdvancedSlider[] _advancedSliders;
        private AdvancedToggle[] _advancedToggles;
        private AdvancedCarouselButton[] _advancedCarouselButtons;
        private AdvancedInputField[] _advancedInputFields;

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

        private void Awake()
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
                    case StyledTextType.Value:
                        styledText.SetStyle(styleTheme.valueTextStyle);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (StyledImage styledImage in _styledImages)
            {
                switch (styledImage.styleType)
                {
                    case StyledImageType.None:
                        break;
                    case StyledImageType.BackgroundPanel:
                        styledImage.SetStyle(styleTheme.backgroundImageStyle);
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
                slider.SetStyle(theme.settingsButtonStyle);
            }
            
            foreach (AdvancedToggle toggle in _advancedToggles)
            {
                toggle.SetStyle(theme.settingsButtonStyle);
            }
            
            foreach (AdvancedCarouselButton carouselButton in _advancedCarouselButtons)
            {
                carouselButton.SetStyle(theme.settingsButtonStyle);
            }
            
            foreach (AdvancedInputField inputField in _advancedInputFields)
            {
                inputField.SetStyle(theme.settingsButtonStyle);
            }
        }

        private void FindStyledObjects()
        {
            _styledTexts = FindObjectsByType<StyledText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _styledImages = FindObjectsByType<StyledImage>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedButtons = FindObjectsByType<AdvancedButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedNavButtons = FindObjectsByType<AdvancedNavButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedSliders = FindObjectsByType<AdvancedSlider>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedToggles = FindObjectsByType<AdvancedToggle>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedCarouselButtons = FindObjectsByType<AdvancedCarouselButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _advancedInputFields = FindObjectsByType<AdvancedInputField>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }
    }
    
    
}
