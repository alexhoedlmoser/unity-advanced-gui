using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AlexH.Helper;

namespace AlexH.AdvancedGUI{
    public class AdvancedButton : AdvancedSelectable
    {
        private Vector2 _defaultSize;
        
        [Header("Button specific")]
        [SerializeField] private float hoverSizeDelta;
        [SerializeField] private float hoverTransitionDuration = 0.1f;
        [SerializeField] private float hoverLabelCharacterSpacing = 15f;

        private Sequence _currentSequence;
        private Coroutine _characterSpacingTween;
        
        protected override void Start()
        {
            base.Start();
            _defaultSize = _backgroundTransform.sizeDelta;
        }

        protected override void InitializeSelectable()
        {
            base.InitializeSelectable();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _currentSequence?.Kill();
            _currentSequence = HoverSequence(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _currentSequence?.Kill();
            _currentSequence = HoverSequence(false);
        }

        private Sequence HoverSequence(bool hover)
        {
            Sequence sequence;
            if (hover)
            {
                sequence =  DOTween.Sequence()
                    .Append(_backgroundTransform.DOSizeDelta(GetPaddedSize(_defaultSize, hoverSizeDelta), hoverTransitionDuration).SetEase(Ease.OutCubic))
                    .Join(backgroundImage.DOColor(_hoverColor, hoverTransitionDuration).SetEase(Ease.Linear));

                // sequence.Join(useIconInsteadOfLabel
                //     ? icon.DOColor(_hoverLabelColor, hoverTransitionDuration).SetEase(Ease.Linear)
                //     : label.DOColor(_hoverLabelColor, hoverTransitionDuration).SetEase(Ease.Linear));

                icon.color =  _hoverLabelColor;
                label.color = _hoverLabelColor;

                if (_characterSpacingTween != null)
                {
                    StopCoroutine(_characterSpacingTween);
                }
                _characterSpacingTween = StartCoroutine(TweenCharacterSpacing(label, hoverLabelCharacterSpacing, hoverTransitionDuration));
            }
            else
            {
                sequence =  DOTween.Sequence()
                    .Append(_backgroundTransform.DOSizeDelta(_defaultSize, hoverTransitionDuration).SetEase(Ease.OutCubic))
                    .Join(backgroundImage.DOColor(_defaultColor, hoverTransitionDuration).SetEase(Ease.Linear));

                // sequence.Join(useIconInsteadOfLabel
                //     ? icon.DOColor(_defaultLabelColor, hoverTransitionDuration).SetEase(Ease.Linear)
                //     : label.DOColor(_defaultLabelColor, hoverTransitionDuration).SetEase(Ease.Linear));

                icon.color = _defaultLabelColor;
                label.color = _defaultLabelColor;

                if (_characterSpacingTween != null)
                {
                    StopCoroutine(_characterSpacingTween);
                }
                _characterSpacingTween = StartCoroutine(TweenCharacterSpacing(label, _defaultLabelCharacterSpacing, hoverTransitionDuration));
            }

            return sequence;
        }
    }
}

