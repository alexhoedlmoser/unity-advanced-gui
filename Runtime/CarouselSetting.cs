using System.Collections.Generic;
using UnityEngine;

namespace AlexH.AdvancedGUI
{
    public class CarouselSetting : MonoBehaviour
    {
        [SerializeField] private AdvancedCarouselButton carouselButton;
        public List<string> options;
        public int defaultOptionIndex;

        public void TestIntEventMethod(int index)
        {
            //print("Current carousel setting index: " + index);
            carouselButton.UpdateCarouselDisplay(options[index]);
        }

        public void Start()
        {
            carouselButton.SetupCarousel(options.Count, defaultOptionIndex);
            TestIntEventMethod(defaultOptionIndex);
        }
    }
}

