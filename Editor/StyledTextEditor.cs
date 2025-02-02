using UnityEditor;
using UnityEngine;

namespace AlexH.AdvancedGUI.Editor
{
    [CustomEditor(typeof(StyledText)), CanEditMultipleObjects]
    public class StyledTextEditor : TMPro.EditorUtilities.TMP_EditorPanelUI
    {
        public override void OnInspectorGUI()
        {
            StyledText styledText = (StyledText)target;
       
            GUILayout.Label("Styling", EditorStyles.boldLabel);
       
            styledText.overrideStylingObject = (TextStylingObject)EditorGUILayout.ObjectField("Override Styling Object", styledText.overrideStylingObject, typeof(TextStylingObject), true);
            styledText.type = (StyledTextType)EditorGUILayout.EnumPopup("Type", styledText.type);
            styledText.useAutoSize = EditorGUILayout.Toggle("Use Auto Size", styledText.useAutoSize);
            styledText.onlyApplyColors = EditorGUILayout.Toggle("Only Apply Colors", styledText.onlyApplyColors);
       
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}

