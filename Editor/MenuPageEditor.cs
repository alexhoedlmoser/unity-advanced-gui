using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AlexH.AdvancedGUI;

namespace AlexH.AdvancedGUI.Editor
{
    [CustomEditor(typeof(MenuPage))]
    public class MenuPageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MenuPage menuPageScript = (MenuPage)target;

            GUILayout.Space(10);
            
            if (GUILayout.Button("Set current Position as Default"))
            {
                menuPageScript.SetDefaultPosition();
            }
            
            if (GUILayout.Button("Enable Page"))
            {
                menuPageScript.EnablePage();
            }
            
            if (GUILayout.Button("Disable Page"))
            {
                menuPageScript.DisablePage();
            }
        }
    }
}
