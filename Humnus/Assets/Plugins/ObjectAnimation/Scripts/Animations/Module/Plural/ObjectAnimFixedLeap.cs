using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimFixedLeap : ObjectFixedAnimBase
    {
        private readonly Action<float> action = null;


        internal ObjectAnimFixedLeap(float second, Action<float> act, EasingTypes easing)
            : base(second, easing)
        {
            Name = "FixedLeap";
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
            return new ObjectAnimFixedLeap(second, action, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            action(1);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner FixedFloatLeapAnim(
            this AnimationPlanner animation, float second, Action<float> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimFixedLeap(second, anim, easing));
        }
    }
}
