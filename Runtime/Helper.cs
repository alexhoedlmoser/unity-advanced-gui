using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AlexH
{
    public static class Helper
    {
        public static float RangeTo01(float value, float min, float max) => (value - min) / (max - min);
    
        //https://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
        /// <summary>
        /// Remaps a value from one range to another
        /// </summary>
        /// <param name="value">The value to be remapped</param>
        /// <param name="from1">Range start of the old value</param>
        /// <param name="to1">Range end of the old value</param>
        /// <param name="from2">New range start</param>
        /// <param name="to2">New range end</param>
        public static float RemapRange(float value, float from1, float to1, float from2, float to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        
        public static Vector2 GetPaddedSize(Vector2 size, float padding)
        {
            return new Vector2(size.x + padding, size.y + padding);
        }

        public static Sequence PlaySequenceSolo(Sequence currentSequence, Sequence sequenceToPlay)
        {
            currentSequence?.Kill();
            currentSequence = sequenceToPlay;
            return currentSequence;
        }

        public static IEnumerator TweenCharacterSpacing(TMP_Text text, float endSpacing, float duration)
        {
            float spacing = text.characterSpacing;
            Tween tween = DOTween.To(() => spacing, x => spacing = x, endSpacing, duration).SetEase(Ease.OutCubic);

            while (tween.IsActive())
            {
                text.characterSpacing = spacing;
                yield return new WaitForEndOfFrame();
            }
        }

        public static Sequence FadeImages(Image[] images, Color color, float duration)
        {
            Sequence sequence = DOTween.Sequence();

            foreach (Image image in images)
            {
                sequence.Join(image.DOColor(color, duration).SetEase(Ease.Linear));
            }

            return sequence;
        }
        
        public static Sequence FadeImagesWithAlpha(Image[] images, Color color, float alpha, float duration)
        {
            Sequence sequence = DOTween.Sequence();

            Color tmpColor = color;
            tmpColor.a = alpha;

            foreach (Image image in images)
            {
                sequence.Join(image.DOColor(tmpColor, duration).SetEase(Ease.Linear));
            }

            return sequence;
        }

        public static void RecolorImages(Image[] images, Color color)
        {
            foreach (Image image in images)
            {
                image.color = color;
            }
        }
        
        public static void RecolorImagesWithAlpha(Image[] images, Color color, float alpha)
        {
            Color tmpColor = color;
            tmpColor.a = alpha;
            
            foreach (Image image in images)
            {
                image.color = tmpColor;
            }
        }

        public static void RecolorImageWithAlpha(Image image, Color color, float alpha)
        {
            Color tmpColor = color;
            tmpColor.a = alpha;
            
            image.color = tmpColor;
        }
    }
}
