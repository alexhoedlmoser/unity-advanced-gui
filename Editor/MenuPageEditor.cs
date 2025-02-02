using UnityEngine;
using UnityEditor;

namespace AlexH.AdvancedGUI.Editor
{
    [CustomEditor(typeof(MenuPage)), CanEditMultipleObjects]
    public class MenuPageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            GUILayout.Space(10);
                
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Set current position as default"))
            {
                foreach (Object target in targets)
                {
                    MenuPage menuPageScript = (MenuPage)target;
                    menuPageScript.SetDefaultPosition();
                }
                
            }
            GUILayout.Space(10);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Enable", GUILayout.Height(50)))
            {
                foreach (Object target in targets)
                {
                    MenuPage menuPageScript = (MenuPage)target;
                    menuPageScript.EnablePage();
                }
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Disable",GUILayout.Height(50)))
            {
                foreach (Object target in targets)
                {
                    MenuPage menuPageScript = (MenuPage)target;
                    menuPageScript.DisablePage();
                }
            }
            
        }
    }
}
