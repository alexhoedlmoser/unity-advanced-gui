/*
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AlexH.AdvancedGUI.Editor
{
    public class GameObjectMenu : MonoBehaviour
    {
        private const int PRIORITY = 100;

        [MenuItem("GameObject/Advanced GUI/Advanced Selectables/Advanced Button", false, PRIORITY)]
        private static void CreateAdvancedButton()
        {
            var instance = Instantiate(Resources.Load("Prefabs/AdvancedButton"), Selection.activeTransform) as GameObject;  
            instance.name = "AdvancedButton";
            
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeGameObject = instance;
        }
        
        [MenuItem("GameObject/Advanced GUI/Advanced Selectables/Advanced Toggle", false, PRIORITY)]
        private static void CreateAdvancedToggle()
        {
            var instance = Instantiate(Resources.Load("Prefabs/AdvancedToggle"), Selection.activeTransform) as GameObject;
            instance.name = "AdvancedToggle";
            
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeGameObject = instance;
        }
        
        [MenuItem("GameObject/Advanced GUI/Advanced Selectables/Advanced Slider", false, PRIORITY)]
        private static void CreateAdvancedSlider()
        {
            var instance = Instantiate(Resources.Load("Prefabs/AdvancedSlider"), Selection.activeTransform) as GameObject;   
            instance.name = "AdvancedSlider";
            
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeGameObject = instance;
        }
        
        [MenuItem("GameObject/Advanced GUI/Advanced Selectables/Advanced Carousel Button", false, PRIORITY)]
        private static void CreateAdvancedCarouselButton()
        {
            var instance = Instantiate(Resources.Load("Prefabs/AdvancedCarouselButton"), Selection.activeTransform) as GameObject;   
            instance.name = "AdvancedCarouselButton";
            
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeGameObject = instance;
        }
        
        [MenuItem("GameObject/Advanced GUI/Advanced Selectables/Advanced Nav Button", false, PRIORITY)]
        private static void CreateAdvancedNavButton()
        {
            var instance = Instantiate(Resources.Load("Prefabs/AdvancedNavButton"), Selection.activeTransform) as GameObject;   
            instance.name = "AdvancedNavButton";
            
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeGameObject = instance;
              
        }
        
    }
}
*/

