using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace AlexH.AdvancedGUI
{
    public enum StyledImageType
    {
        None,
        BackgroundPanel,
    }
    [ExecuteInEditMode]
    public class StyledImage : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private ImageStylingObject overrideStylingObject;
        public StyledImageType type;
        public bool onlyApplyColors;

        private Image _image;
        private ImageStylingObject _currentStyle;
        
        
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
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
            
            if (_image == null)
            {
                _image = gameObject.GetComponent<Image>();
            }

            _image.color = _currentStyle.color;
            
            if (onlyApplyColors)
            {
                return;
            }

            if (_currentStyle.sprite)
            {
                _image.sprite = _currentStyle.sprite;
                _image.type = _currentStyle.imageMode;
                _image.pixelsPerUnitMultiplier = _currentStyle.spritePixelPerUnitMultiplier;
            }
            else
            {
                _image.sprite = null;
            }
        }
        
        
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isUpdating) return;

            if (_image == null)
            {
                _image = gameObject.GetComponent<Image>();
            }
            
            if (_image != null)
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



