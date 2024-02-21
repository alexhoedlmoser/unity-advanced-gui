using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using static AlexH.AdvancedGUI.Helper;

namespace AlexH.AdvancedGUI.Editor
{
    [EditorWindowTitle(title = "Style Theme Creator")]
    public class StyleThemeCreator : EditorWindow
    {
        private string _path = "Assets/AdvancedGUI/Themes";
        private string _themeName = "";

        private Color _baseColor = new Color(0.1f, 0.1f, 0.1f);
        private Color _highlightColor = new Color(0.9f, 0.9f, 0.9f, 0.75f);
        private Color _contrastColor = new Color(0.15f, 0.15f, 0.15f);
        private Color _headlineColor = Color.white;
        private Color _paragraphColor = new Color(0.9f, 0.9f, 0.9f);

        private TMP_FontAsset _font;
        private TMP_FontAsset _numbersFont;
        private FontWeight _titleFontWeight = FontWeight.Black;
        private FontWeight _headlineFontWeight = FontWeight.Bold;

        private Sprite _frameSprite;
        private Image.Type _frameImageMode = Image.Type.Tiled;

        private Vector2 _scrollPos;

        [MenuItem("Tools/Advanced GUI/Style Theme Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<StyleThemeCreator>();
            window.titleContent = new GUIContent("Style Theme Creator");
            window.Show();
        }
        
        private void OnGUI()
        {
            // Texture2D headerTexture = (Texture2D)Resources.Load("TitleHeader");
            // GUILayout.Box(headerTexture, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            Texture2D cover = Resources.Load<Texture2D>("Images/TitleHeader");
            float imageWidth = EditorGUIUtility.currentViewWidth;
            float imageHeight = imageWidth * cover.height / cover.width;
            Rect rect = GUILayoutUtility.GetRect(imageWidth, imageHeight);
            GUI.DrawTexture(rect, cover, ScaleMode.ScaleToFit);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            
            GUILayout.Space(15);
            GUILayout.Label("Create a new Style Theme", EditorStyles.largeLabel);
            GUILayout.Space(30);
            _themeName = EditorGUILayout.TextField("Theme Name", _themeName);
            _path = EditorGUILayout.TextField("Path", _path);

            GUILayout.Space(15);
            GUILayout.Label("Colors", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            _baseColor = EditorGUILayout.ColorField("Base Color", _baseColor);
            _highlightColor = EditorGUILayout.ColorField("Highlight Color", _highlightColor);
            _contrastColor = EditorGUILayout.ColorField("Contrast Color", _contrastColor);
            _headlineColor = EditorGUILayout.ColorField("Headline Color", _headlineColor);
            _paragraphColor = EditorGUILayout.ColorField("Paragraph Color", _paragraphColor);
            
            GUILayout.Space(15);
            GUILayout.Label("Image Styling", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            _frameSprite = (Sprite) EditorGUILayout.ObjectField("Frame Sprite", _frameSprite, typeof(Sprite), false);
            _frameImageMode = (Image.Type) EditorGUILayout.EnumPopup("Frame Image Mode", _frameImageMode);
            
            GUILayout.Space(15);
            GUILayout.Label("Text Styling", EditorStyles.boldLabel);
            GUILayout.Space(10);

            _font = (TMP_FontAsset)EditorGUILayout.ObjectField("Font", _font, typeof(TMP_FontAsset), false);
            _numbersFont = (TMP_FontAsset) EditorGUILayout.ObjectField("Numbers Font", _numbersFont, typeof(TMP_FontAsset), false);
            _titleFontWeight = (FontWeight) EditorGUILayout.EnumPopup("Title Font Weight", _titleFontWeight);
            _headlineFontWeight = (FontWeight) EditorGUILayout.EnumPopup("Headline Font Weight", _headlineFontWeight);
            
            GUILayout.Space(15);
            
            if (GUILayout.Button("Create", GUILayout.Height(50)))
            {
                CreateTheme();
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void CreateStyles()
        {
            CreateDefaultSelectableStyle();
            CreateSettingsButtonStyle();
            CreateNavButtonStyle();
            
            CreateTextStylingObject("Title", _headlineColor, 72f, 10f, _titleFontWeight);
            CreateTextStylingObject("Headline01", _headlineColor,44f, 10f, _headlineFontWeight);
            CreateTextStylingObject("Headline02", _headlineColor,38f, 10f, _headlineFontWeight);
            CreateTextStylingObject("Headline03", _headlineColor,32f, 10f, _headlineFontWeight);
            CreateTextStylingObject("Breadcrumb", _paragraphColor,32f, 0f, FontWeight.Regular);
            CreateTextStylingObject("Paragraph", _paragraphColor, 20f, 0f, FontWeight.Regular);
            CreateTextStylingObject("Value", _highlightColor, 20f, 0f, FontWeight.Regular, true);
            
            CreateImageStylingObject("Background", _baseColor, 0.5f, 0.05f, _frameSprite, _frameImageMode);
        }

        private void CreateTheme()
        {

            if (_themeName == "")
            {
                Debug.LogError("Theme name cannot be empty");
                return;
            }

            if (_font == null)
            {
                Debug.LogError("Font cannot be empty");
                return;
            }

            StyleThemeObject styleThemeObject = CreateInstance<StyleThemeObject>();
            string fileName = _themeName + "_Theme";
            styleThemeObject.name = fileName;

            if (AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/" + fileName + ".asset", typeof(StyleThemeObject)))
            {
                if (EditorUtility.DisplayDialog("Are you sure?",
                        "The theme already exists. Do you want to overwrite it?", "Yes", "No"))
                {
                    AssetDatabase.DeleteAsset(_path + "/" + _themeName);
                }
                else
                {
                    return;
                }
            }

            CreateFolders();
            CreateStyles();

            styleThemeObject.buttonStyle =
                AssetDatabase.LoadAssetAtPath(
                    _path + "/" + _themeName + "/Selectables/" + _themeName + "_DefaultSelectable.asset",
                    typeof(SelectableStylingObject)) as SelectableStylingObject;
            styleThemeObject.settingsButtonStyle =
                AssetDatabase.LoadAssetAtPath(
                    _path + "/" + _themeName + "/Selectables/" + _themeName + "_SettingsSelectable.asset",
                    typeof(SelectableStylingObject)) as SelectableStylingObject;
            styleThemeObject.navButtonStyle =
                AssetDatabase.LoadAssetAtPath(
                    _path + "/" + _themeName + "/Selectables/" + _themeName + "_NavButton.asset",
                    typeof(SelectableStylingObject)) as SelectableStylingObject;

            styleThemeObject.titleTextStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Texts/" + _themeName + "_Title.asset",
                    typeof(TextStylingObject)) as TextStylingObject;
            styleThemeObject.headline01TextStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Texts/" + _themeName + "_Headline01.asset",
                    typeof(TextStylingObject)) as TextStylingObject;
            styleThemeObject.headline02TextStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Texts/" + _themeName + "_Headline02.asset",
                    typeof(TextStylingObject)) as TextStylingObject;
            styleThemeObject.headline03TextStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Texts/" + _themeName + "_Headline03.asset",
                    typeof(TextStylingObject)) as TextStylingObject;
            styleThemeObject.breadcrumbTextStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Texts/" + _themeName + "_Breadcrumb.asset",
                    typeof(TextStylingObject)) as TextStylingObject;
            styleThemeObject.paragraphTextStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Texts/" + _themeName + "_Paragraph.asset",
                    typeof(TextStylingObject)) as TextStylingObject;
            styleThemeObject.valueTextStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Texts/" + _themeName + "_Value.asset",
                    typeof(TextStylingObject)) as TextStylingObject;

            styleThemeObject.backgroundImageStyle =
                AssetDatabase.LoadAssetAtPath(_path + "/" + _themeName + "/Images/" + _themeName + "_Background.asset",
                    typeof(ImageStylingObject)) as ImageStylingObject;

            AssetDatabase.CreateAsset(styleThemeObject, _path + "/" + _themeName + "/" + fileName + ".asset");
            AssetDatabase.SaveAssets();
        }
        
        private void CreateFolders()
        {
            if (!AssetDatabase.IsValidFolder("Assets/AdvancedGUI"))
            {
                AssetDatabase.CreateFolder("Assets", "AdvancedGUI");
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/AdvancedGUI/Themes"))
            {
                AssetDatabase.CreateFolder("Assets/AdvancedGUI", "Themes");
            }
            
            AssetDatabase.CreateFolder("Assets/AdvancedGUI/Themes", _themeName);
            AssetDatabase.CreateFolder("Assets/AdvancedGUI/Themes/" + _themeName, "Selectables");
            AssetDatabase.CreateFolder("Assets/AdvancedGUI/Themes/" + _themeName, "Texts");
            AssetDatabase.CreateFolder("Assets/AdvancedGUI/Themes/" + _themeName, "Images");
            AssetDatabase.SaveAssets();
        }

        private void CreateDefaultSelectableStyle()
        {
            SelectableStylingObject defaultSelectableStyle = CreateInstance<SelectableStylingObject>();
            defaultSelectableStyle.name = _themeName + "_DefaultSelectable";
            
            defaultSelectableStyle.defaultColor = ColorModifyAlpha(_baseColor, 0.75f);
            defaultSelectableStyle.hoverColor = _highlightColor;
            defaultSelectableStyle.pressedColor = ColorModifyAlpha(_highlightColor, 1f);
            defaultSelectableStyle.disabledColor = ColorModifyAlpha(_baseColor, 0.25f);
            
            defaultSelectableStyle.defaultContentColor = _highlightColor;
            defaultSelectableStyle.hoverContentColor = _contrastColor;
            defaultSelectableStyle.pressedContentColor = ColorModifyAlpha(_contrastColor, 1f);
            
            defaultSelectableStyle.textFontAsset = _font;
            if (_numbersFont)
            {
                defaultSelectableStyle.numbersFontAsset = _numbersFont;
            }

            defaultSelectableStyle.sprite = _frameSprite;
            defaultSelectableStyle.imageMode = _frameImageMode;
            
            AssetDatabase.CreateAsset(defaultSelectableStyle, _path + "/" + _themeName + "/Selectables/" + _themeName + "_DefaultSelectable.asset");
            AssetDatabase.SaveAssets();
            
        }
        
        private void CreateSettingsButtonStyle()
        {
            SelectableStylingObject settingsSelectableStyle = CreateInstance<SelectableStylingObject>();
            settingsSelectableStyle.name = _themeName + "_SettingsSelectable";
            
            settingsSelectableStyle.defaultColor = ColorModifyAlpha(_baseColor, 0.75f);
            settingsSelectableStyle.hoverColor = _highlightColor;
            settingsSelectableStyle.pressedColor = ColorModifyAlpha(_highlightColor, 1f);
            settingsSelectableStyle.disabledColor = ColorModifyAlpha(_baseColor, 0.25f);

            settingsSelectableStyle.defaultContentColor = _highlightColor;
            settingsSelectableStyle.hoverContentColor = _contrastColor;
            settingsSelectableStyle.pressedContentColor = ColorModifyAlpha(_contrastColor, 1f);

            settingsSelectableStyle.textFontAsset = _font;
            
            if (_numbersFont)
            {
                settingsSelectableStyle.numbersFontAsset = _numbersFont;
            }
            
            settingsSelectableStyle.sprite = _frameSprite;
            settingsSelectableStyle.imageMode = _frameImageMode;

            AssetDatabase.CreateAsset(settingsSelectableStyle, _path + "/" + _themeName + "/" + "/Selectables/" + _themeName + "_SettingsSelectable.asset");
            AssetDatabase.SaveAssets();
        }

        private void CreateNavButtonStyle()
        {
            SelectableStylingObject navButtonStyle = CreateInstance<SelectableStylingObject>();
            navButtonStyle.name = _themeName + "_NavButton";
            
            navButtonStyle.defaultColor = ColorModifyAlpha(_baseColor, 0f);
            navButtonStyle.hoverColor = ColorModifyAlpha(_highlightColor, 0.15f);
            navButtonStyle.pressedColor = ColorModifyAlpha(_highlightColor, 1f);
            navButtonStyle.disabledColor = ColorModifyAlpha(_baseColor, 0f);

            navButtonStyle.defaultContentColor = _highlightColor;
            navButtonStyle.hoverContentColor = ColorModifyAlpha(_highlightColor, 1f);
            navButtonStyle.pressedContentColor = ColorModifyAlpha(_contrastColor, 1f);

            navButtonStyle.textFontAsset = _font;

            if (_numbersFont)
            {
                navButtonStyle.numbersFontAsset = _numbersFont;
            }
            
            navButtonStyle.sprite = _frameSprite;
            navButtonStyle.imageMode = _frameImageMode;

            AssetDatabase.CreateAsset(navButtonStyle, _path + "/" + _themeName + "/" + "/Selectables/" + _themeName + "_NavButton.asset");
            AssetDatabase.SaveAssets();
        }

        private void CreateTextStylingObject(string styleName, Color color, float fontSize, float characterSpacing, FontWeight fontWeight, bool useValueFont = false)
        {
            TextStylingObject textStylingObject = CreateInstance<TextStylingObject>();
            textStylingObject.name = _themeName + "_" + styleName;

            if (useValueFont && _numbersFont != null)
            {
                textStylingObject.textFontAsset = _numbersFont;
            }
            else
            {
                textStylingObject.textFontAsset = _font;
            }
            
            textStylingObject.color = color;
            textStylingObject.fontSize = fontSize;
            textStylingObject.characterSpacing = characterSpacing;
            textStylingObject.fontWeight = fontWeight;

            AssetDatabase.CreateAsset(textStylingObject, _path + "/" + _themeName + "/Texts/" + _themeName + "_" + styleName + ".asset");
            AssetDatabase.SaveAssets();
        }

        private void CreateImageStylingObject(string styleName, Color color, float alpha, float brightness, Sprite sprite, Image.Type imageMode)
        {
            ImageStylingObject imageStylingObject = CreateInstance<ImageStylingObject>();
            imageStylingObject.name = _themeName + "_" + styleName;

            Color tmpColor = color;
            //color = ColorModifyBrightness(tmpColor, brightness);
            color = ColorModifyAlpha(color, alpha);
            
            imageStylingObject.color = color;
            imageStylingObject.sprite = sprite;
            imageStylingObject.imageMode = imageMode;
            
            AssetDatabase.CreateAsset(imageStylingObject, _path + "/" + _themeName + "/Images/" + _themeName + "_" + styleName + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
    
    
}

