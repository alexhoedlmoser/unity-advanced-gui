using System.Collections;
using System.Collections.Generic;
using AlexH.AdvancedGUI;
using UnityEditor;
using UnityEngine;


namespace AlexH.AdvancedGUI.Editor
{
    [CustomEditor(typeof(StyledImage))]
    public class StyledImageEditor : UnityEditor.UI.ImageEditor
    {
        public override void OnInspectorGUI()
        {
            StyledImage styledImage = (StyledImage)target;
       
            GUILayout.Label("Styling", EditorStyles.boldLabel);
       
            styledImage.overrideStylingObject = (ImageStylingObject)EditorGUILayout.ObjectField("Override Styling Object", styledImage.overrideStylingObject, typeof(ImageStylingObject), true);
            styledImage.styleType = (StyledImageType)EditorGUILayout.EnumPopup("Style Type", styledImage.styleType);
            styledImage.onlyApplyColors = EditorGUILayout.Toggle("Only Apply Colors", styledImage.onlyApplyColors);
       
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}

