using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Image Style", menuName = "AdvancedGUI/Styles/Image Style")]
public class ImageStylingObject : ScriptableObject
{
    [Header("Colors")]
    public Color color = Color.white;

    [Header("Sprite")] 
    public Sprite sprite;
    public Image.Type imageMode = Image.Type.Tiled;
    public int spritePixelPerUnitMultiplier = 3;
}
