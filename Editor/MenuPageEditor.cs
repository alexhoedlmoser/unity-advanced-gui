using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AlexH.AdvancedGUI;
using UnityEditor.Graphs;

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

            GUILayout.Space(10);
            
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Enable Page", GUILayout.Height(50)))
            {
                menuPageScript.EnablePage();
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Disable Page" ,GUILayout.Height(50)))
            {
                menuPageScript.DisablePage();
            }
        }
    }
}
