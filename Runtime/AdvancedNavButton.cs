using System;
using System.Collections;
using System.Collections.Generic;
using AlexH.AdvancedGUI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlexH.AdvancedGUI
{
    [AddComponentMenu("Advanced GUI/Advanced Selectables/Advanced Nav Button")]
    public class AdvancedNavButton : AdvancedSelectable
    {
        public event Action<AdvancedNavButton> OnSelected;
        //public bool isSelected;

        protected override void Start()
        {
            base.Start();
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            OnSelected?.Invoke(this);
            isSelected = true;
            
            base.OnPointerUp(eventData);
        }

        public void SelectButton()
        {
            OnSelected?.Invoke(this);
            isSelected = true;

            if (isPressed)
            {
                PressedState();
            }
            else if (isHovered)
            {
                HoverState();
            }
            else
            {
                SelectedState();
            }
        }

        public void SelectButtonOnStart()
        {
            OnSelected?.Invoke(this);
            isSelected = true;
            
            SelectedStateInstant();
        }

        public IEnumerator SelectButtonOnStartDelayed()
        {
            yield return new WaitForEndOfFrame();
            SelectButtonOnStart();
        }

        public void UnselectButton()
        {
            isSelected = false;

            if (isPressed)
            {
                PressedState();
            }
            else if (isHovered)
            {
                HoverState();
            }
            else
            {
                DefaultState();
            }
            
        }

    }
}
