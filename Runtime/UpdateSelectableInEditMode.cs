using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace AlexH.AdvancedGUI
{
    [ExecuteInEditMode]
    public class UpdateSelectableInEditMode: MonoBehaviour, ISerializationCallbackReceiver
    {
        private AdvancedSelectable selectable;

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isUpdating) return;

            if (selectable == null)
            {
                selectable = gameObject.GetComponent<AdvancedSelectable>();
            }
            
            if (selectable != null)
            {
                selectable.OnBeforeSerialize();
            }
#endif

        }

        public void OnAfterDeserialize()
        {
        }
    }
}
