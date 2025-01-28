using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlexH.AdvancedGUI
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
        
        public static Sequence FadeImage(Image image, Color color, float duration)
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(image.DOColor(color, duration).SetEase(Ease.Linear));
                
            return sequence;
        }
        
        public static Sequence FadeImageWithAlpha(Image image, Color color, float alpha, float duration)
        {
            Sequence sequence = DOTween.Sequence();

            Color tmpColor = color;
            tmpColor.a = alpha;
            
            sequence.Join(image.DOColor(tmpColor, duration).SetEase(Ease.Linear));
            
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
                if (!image)
                {
                    continue;
                }
                
                image.color = tmpColor;
            }
        }

        public static void RecolorImageWithAlpha(Image image, Color color, float alpha)
        {
            if (!image)
            {
                return;
            }
            
            Color tmpColor = color;
            tmpColor.a = alpha;
            
            image.color = tmpColor;
        }

        public static void SetImageAlpha(Image image, float alpha)
        {
            if (!image)
            {
                return;
            }
            
            Color tmpColor = image.color;
            tmpColor.a = alpha;
            
            image.color = tmpColor;
        }
        
        public static Color ColorModifyAlpha(Color color, float alpha)
        {
            color.a = alpha;

            return color;
        }
        
        public static Color ColorModifyBrightness(Color color, float brightness, bool fullAlpha = true)
        {
            color.r *= brightness;
            color.g *= brightness;
            color.b *= brightness;

            if (fullAlpha)
            {
                color.a = 1f;
            }
            
            return color;
        }
        
        public static Vector4 GetPaddingFromGradient(GradientType gradient, float padding)
        {
            Vector4 paddingVector = Vector4.zero;

            switch (gradient)
            {
                case GradientType.LeftToRight:
                    paddingVector = new Vector4(padding, 0, 0, 0);
                    break;
                case GradientType.RightToLeft:
                    paddingVector = new Vector4(0, 0, padding, 0);
                    break;
                case GradientType.TopToBottom:
                    paddingVector = new Vector4(0, 0, 0, padding);
                    break;
                case GradientType.BottomToTop:
                    paddingVector = new Vector4(0, padding, 0, 0);
                    break;
                case GradientType.None:
                default:
                    break;
            }

            return paddingVector;
        }

        public static Vector2Int GetSoftnessFromGradient(GradientType gradientType, Vector2 objectSize, float softness0To1)
        {
            Vector2Int softnessVector = Vector2Int.zero;

            int softness;
            
            switch (gradientType)
            {
                case GradientType.LeftToRight:
                case GradientType.RightToLeft:
                    softness = Mathf.RoundToInt(RemapRange(softness0To1, 0, 1, 0, objectSize.x*2));
                    softnessVector = new Vector2Int(softness, 0);
                    break;
                case GradientType.BottomToTop:
                case GradientType.TopToBottom:
                    softness = Mathf.RoundToInt(RemapRange(softness0To1, 0, 1, 0, objectSize.y*2));
                    softnessVector = new Vector2Int(0, softness);
                    break;
                case GradientType.None:
                default:
                    break;
            }

            return softnessVector;
        }
        
        public static async void SimulateButtonClick(GameObject button)
        {
            ExecuteEvents.Execute (button, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            await Task.Delay(10);
            ExecuteEvents.Execute (button, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute (button, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            //ExecuteEvents.Execute (button, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
        }
        
        
        public static Sequence ScaleBounce(RectTransform transform, float scaleDelta, float duration)
        {
            return DOTween.Sequence()
                .Append(transform.DOScale(1f + scaleDelta, 0.15f).SetEase(Ease.OutCubic))
                .Append(transform.DOScale(1f, duration).SetEase(Ease.OutCubic));
        }
    }
}
