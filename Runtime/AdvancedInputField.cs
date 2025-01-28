using AlexH.AdvancedGUI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdvancedInputField : AdvancedSelectable
{
    [Header("--- InputField Specific ---")]
    
    [Header("References")]
    [SerializeField] private TMP_InputField _inputField;
    private TMP_Text _placeholderText;
    private TMP_Text _inputText;
    private MenuPage _menuPage;

    private bool _isInEditMode;
    

    protected override void Awake()
    {
        base.Awake();

        _menuPage = GetComponentInParent<MenuPage>();
        _placeholderText = _inputField.placeholder.GetComponent<TMP_Text>();
        _inputText = _inputField.textComponent;
        
        _inputField.onSelect.AddListener(OnSelectHandler);
        _inputField.onDeselect.AddListener(OnDeselectHandler);
        _inputField.onEndEdit.AddListener(OnEndEditHandler);
    }

    private void OnDeselectHandler(string _)
    {
        if (!_isInEditMode) return;
        
        print("deselect");
        if (_menuPage)
        {
            _menuPage.ActivateShortcuts();
        }
        
        _isInEditMode = false;
    }
    
    private void OnSelectHandler(string _)
    {
        if (_isInEditMode) return;
        
        print("select");
        if (_menuPage)
        {
            _menuPage.DeactivateShortcuts();
        }
        
        _isInEditMode = true;
    }
    
    private void OnEndEditHandler(string _)
    {
        OnDeselectHandler(_);
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
       base.OnPointerClick(eventData);
       OnSelectHandler(_inputField.text);
       _inputField.OnPointerClick(eventData);
    }

    protected override void LoadStyle()
    {
        base.LoadStyle();

        if (!currentStyle) return;
        if (!_inputField) return;

        if (!_placeholderText)
        {
            _placeholderText = _inputField.placeholder.GetComponent<TMP_Text>();
        }
        
        if (!_inputText)
        {
            _inputText = _inputField.textComponent;
        }
        
        _inputField.fontAsset = currentStyle.numbersFontAsset? currentStyle.numbersFontAsset : currentStyle.textFontAsset;

    }

    protected override void DefaultStateInstant()
    {
        base.DefaultStateInstant();

        if (!_inputText || !_placeholderText) return;
        
        _inputText.color = _placeholderText.color = defaultContentColor;
        _inputText.fontWeight = _placeholderText.fontWeight = defaultFontWeight;
    }

    protected override void DefaultState()
    {
        base.DefaultState();
        _inputText.color = _placeholderText.color = defaultContentColor;
        _inputText.fontWeight = _placeholderText.fontWeight = defaultFontWeight;
    }

    protected override void PressedState()
    {
        base.PressedState();
        _inputText.color = _placeholderText.color = hoverContentColor;
        _inputText.fontWeight = _placeholderText.fontWeight = hoverFontWeight;
    }

    protected override void HoverState()
    {
        base.HoverState();
        _inputText.color = _placeholderText.color = hoverContentColor;
        _inputText.fontWeight = _placeholderText.fontWeight = hoverFontWeight;
        
    }
}
