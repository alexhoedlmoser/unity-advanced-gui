using UnityEngine;
using UnityEngine.EventSystems;

namespace AlexH.AdvancedGUI
{
    [AddComponentMenu("Advanced GUI/Effects/Advanced Selectable Pointer Events")]
    public class AdvancedSelectableEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        [SerializeField] private AdvancedSelectable selectable;

        [SerializeField] private bool pointerEnter;
        [SerializeField] private bool pointerExit;
        [SerializeField] private bool pointerDown;
        [SerializeField] private bool pointerUp;
        [SerializeField] private bool pointerClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (pointerDown)
            {
                selectable.OnPointerDown(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (pointerUp)
            {
                selectable.OnPointerUp(eventData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (pointerEnter)
            {
                selectable.OnPointerEnter(eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (pointerExit)
            {
                selectable.OnPointerExit(eventData);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (pointerClick)
            {
                selectable.OnPointerClick(eventData);
            }
        }
    }
}
