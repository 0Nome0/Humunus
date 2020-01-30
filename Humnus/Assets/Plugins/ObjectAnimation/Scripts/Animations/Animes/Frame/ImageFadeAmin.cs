using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace NerScript.Anime
{
    internal enum FadeType { In, Out, }

    internal sealed class ImageFadeAnim : ObjectAnimBase
    {
        private Image image = null;
        private readonly FadeType fadeType;
        private Line line;

        internal ImageFadeAnim(int frame, FadeType _fadeType, EasingTypes easing)
            : base(frame, easing)
        {
            fadeType = _fadeType;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                image = transform.GetComponent<Image>();
                line = new Line(image.color.a, 1);
                if (fadeType == FadeType.Out) line.end = 0;
                anim = GetUpdate(transform, () =>
                {
                    Color c = image.color;
                    image.color = new Color(c.r, c.g, c.b, Mathf.LerpUnclamped(line.start, line.end, TLeapC));
                });
            };
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                Color c = image.color;
                image.color = new Color(c.r, c.g, c.b, Mathf.LerpUnclamped(line.start, line.end, EasingLast));
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ImageFadeAnim(frame, fadeType, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            image = tr.GetComponent<Image>();
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, line.end);
        }
    }

    public static partial class ObjectAnim
    {

        public static AnimationPlanner ImageFadeOut(
            this AnimationPlanner animation, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ImageFadeAnim(frame, FadeType.Out, easing) { Name = "ImageFadeIn" });
        }
        public static AnimationPlanner ImageFadeIn(
            this AnimationPlanner animation, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ImageFadeAnim(frame, FadeType.In, easing) { Name = "ImageFadeOut" });
        }
    }
}
