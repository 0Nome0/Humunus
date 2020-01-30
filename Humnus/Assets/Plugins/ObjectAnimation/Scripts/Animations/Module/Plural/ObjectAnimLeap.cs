using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimLeap : ObjectAnimBase
    {
        private readonly Action<float> action = null;


        internal ObjectAnimLeap(int frame, Action<float> act, EasingTypes easing)
            : base(frame, easing)
        {
            Name = "Leap";
            action = act;
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

        public void Lerp(float lerp)
        {
            action(Mathf.Lerp(0.0f, 1.0f, lerp));
        }


        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimLeap(frame, action, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            action(1);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner FloatLeapAnim(
            this AnimationPlanner animation, int frame, Action<float> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimLeap(frame, anim, easing));
        }
    }
}
