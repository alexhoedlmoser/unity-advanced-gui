using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexH.AdvancedGUI
{
    public class UniversalHighlight : MonoBehaviour
    {
        [Tooltip("Every Selectable in the hierarchy of this object will use this Universal Highlight")]
        [SerializeField] private GameObject uiParent;
        
        private AdvancedSelectable[] _selectables;
    
        private void Awake()
        {
            _selectables = uiParent.GetComponentsInChildren<AdvancedSelectable>();
        }

        private void OnEnable()
        {
            foreach (AdvancedSelectable selectable in _selectables)
            {
                selectable.OnHover += OnHoverHandler;
            }
        }

        private void OnDisable()
        {
            foreach (AdvancedSelectable selectable in _selectables)
            {
                selectable.OnHover -= OnHoverHandler;
            }
        }

        private void OnHoverHandler(AdvancedSelectable selectable, bool hover)
        {
            print(selectable.name + " Hover: " + hover);
        }
    }
}


