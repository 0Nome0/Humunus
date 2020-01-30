using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimFixedIntToInt : ObjectFixedAnimBase
    {
        private readonly Action<int> action = null;
        public Vector2Int line;


        internal ObjectAnimFixedIntToInt(float second, int start, int end, Action<int> act, EasingTypes easing)
            : base(second, easing)
        {
            Name = "FixedIntToInt";
            action = act;
            line = new Vector2Int(start, end);

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
            action((int)Mathf.LerpUnclamped(line.x, line.y, lerp));
        }


        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimFixedIntToInt(second, line.x, line.y, action, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            action(line.y);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner FixedIntToIntAnim(
            this AnimationPlanner animation, float second, int start, int end, Action<int> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimFixedIntToInt(second, start, end, anim, easing));
        }
        public static AnimationPlanner FixedIntToIntAnim(
            this AnimationPlanner animation, float second, Action<int> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimFixedIntToInt(second, 0, (int)second * 60, anim, easing));
        }
    }
}
