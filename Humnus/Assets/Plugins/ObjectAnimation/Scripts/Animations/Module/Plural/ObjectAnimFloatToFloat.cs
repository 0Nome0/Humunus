using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimFloatToFloat : ObjectAnimBase
    {
        private readonly Action<float> action = null;
        private readonly Vector2 line;


        internal ObjectAnimFloatToFloat(int frame, float start, float end, Action<float> act, EasingTypes easing)
            : base(frame, easing)
        {
            Name = "FloatToFloat";
            action = act;
            line = new Vector2(start, end);

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                anim = GetUpdate(transform, () =>
                {
                    Lerp(TLeapC);
                });
            };
        }
        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                Lerp(1.0f);
                onAnimeEnd.OnNext();
            };
        }

        private void Lerp(float lerp)
        {
            action(Mathf.LerpUnclamped(line.x, line.y, lerp));
        }


        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimFloatToFloat(frame, line.x, line.y, action, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            action(line.y);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner FloatToFloatAnim(
            this AnimationPlanner animation, int frame, float start, float end, Action<float> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimFloatToFloat(frame, start, end, anim, easing));
        }
        public static AnimationPlanner FloatToFloatAnim(
            this AnimationPlanner animation, int frame, Action<float> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimFloatToFloat(frame, 0, frame, anim, easing));
        }
    }
}
