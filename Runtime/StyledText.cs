using System;
using System.Collections;
using System.Collections.Generic;
using AlexH.AdvancedGUI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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
    }
    
    [ExecuteInEditMode]
    public class StyledText : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private TextStylingObject overrideStylingObject;
        [SerializeField] private bool useAutoSize;
        public StyledTextType type;
        
        private TMP_Text _text;
        private TextStylingObject _currentStyle;


        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            LoadStyle();
        }

        public void SetStyle(TextStylingObject style)
        {
            _currentStyle = style;
            LoadStyle();
        }

        private void LoadStyle()
        {
            if (overrideStylingObject)
            {
                _currentStyle = overrideStylingObject;
            }
            
            if (!_currentStyle)
            {
                return;
            }
            
            _text.enableAutoSizing = useAutoSize;

            if (useAutoSize)
            {
                _text.fontSizeMax = _currentStyle.fontSize;
                _text.fontSizeMin = 12;
            }
            else
            {
                _text.fontSize = _currentStyle.fontSize;
            }
            _text.color = _currentStyle.color;
            _text.font = _currentStyle.textFontAsset;
            _text.fontStyle = _currentStyle.fontStyle;
            
            _text.characterSpacing = _currentStyle.characterSpacing;
            _text.fontWeight = _currentStyle.fontWeight;
        }
        
        
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isUpdating) return;

            if (_text == null)
            {
                _text = gameObject.GetComponent<TMP_Text>();
            }
            
            if (_text != null)
            {
                LoadStyle();
            }
            
            EditorUtility.SetDirty(gameObject);
#endif

        }

        public void OnAfterDeserialize()
        {
        }
    }
}
