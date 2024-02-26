using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AlexH.AdvancedGUI.Editor
{
    public abstract class ContextMenuConvert
    {
        [MenuItem("CONTEXT/Text/Convert To Styled Text")]
        [MenuItem("CONTEXT/Image/Convert To Styled Image")]
        [MenuItem("CONTEXT/TMP_Text/Convert To Styled Text")]

        static void ConvertToAdvancedGUI(MenuCommand command)
        {
            if (command.context.GetType() == typeof(Text))
            {
                Text text = (Text)command.context;
                StyleText(text);
            }
            else if (command.context.GetType() == typeof(Image))
            {
                Image image = (Image)command.context;
                StyleImage(image);
            }
            else if (command.context.GetType() == typeof(TextMeshProUGUI))
            {
                TMP_Text tmpText = (TMP_Text)command.context;
                StyleTextTMP(tmpText);
            }
        }
        
        private static StyledText StyleTextTMP(TMP_Text value)
        {
            GameObject gameObject = value.gameObject;
            string text = value.text;

            Object.DestroyImmediate(value);
            StyledText styledText = gameObject.AddComponent<StyledText>();
            styledText.text = text;

            return styledText;
        }

        private static StyledImage StyleImage(Image value)
        {
            GameObject gameObject = value.gameObject;
            Object.DestroyImmediate(value);
            return gameObject.AddComponent<StyledImage>();
        }
        
        private static StyledText StyleText(Text value)
        {
            GameObject gameObject = value.gameObject;
            string text = value.text;

            Object.DestroyImmediate(value);
            StyledText styledText = gameObject.AddComponent<StyledText>();
            styledText.text = text;

            return styledText;
        }

    }
}

