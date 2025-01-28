using UnityEngine;
using UnityEngine.InputSystem;

namespace AlexH.AdvancedGUI
{
    [AddComponentMenu("Advanced GUI/Effects/Mouse Parallax")]
    public class MouseParallax : MonoBehaviour
    {
        private Vector3 _screenMiddle;
        private Resolution _currentResolution;
        private Mouse _currentMouse;

        private RectTransform _rectTransform;
        private Vector3 _defaultPosition;

        [Range(0, 1)]
        [SerializeField] private float parallaxStrength = 0.25f;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultPosition = _rectTransform.localPosition;
        }
        
        private void Update()
        {
            _rectTransform.localPosition = _defaultPosition - (ScreenCenterToMouse()*parallaxStrength/4);
        }

        private Vector3 ScreenCenterToMouse()
        {
            return Input.mousePosition - ScreenMiddle();
        }

        private Vector3 ScreenMiddle()
        {
            _currentResolution = Screen.currentResolution;
            return new Vector3(_currentResolution.width / 2f, _currentResolution.height / 2f, 0);
        }
    }
}
