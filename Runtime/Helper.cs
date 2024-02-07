using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace AlexH
{
    public static class Helper
    {
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
            Tween tween = DOTween.To(() => spacing, x => spacing = x, endSpacing, duration);

            while (tween.IsActive())
            {
                text.characterSpacing = spacing;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
