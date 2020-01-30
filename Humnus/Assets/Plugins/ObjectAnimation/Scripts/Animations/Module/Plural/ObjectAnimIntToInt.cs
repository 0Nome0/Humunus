using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimIntToInt : ObjectAnimBase
    {
        private readonly Action<int> action = null;
        private readonly Vector2Int line;


        internal ObjectAnimIntToInt(int frame, int start, int end, Action<int> act, EasingTypes easing)
            : base(frame, easing)
        {
            Name = "IntToInt";
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
            return new ObjectAnimIntToInt(frame, line.x, line.y, action, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            action(line.y);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner IntToIntAnim(
            this AnimationPlanner animation, int frame, int start, int end, Action<int> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimIntToInt(frame, start, end, anim, easing));
        }
        public static AnimationPlanner IntToIntAnim(
            this AnimationPlanner animation, int frame, Action<int> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ObjectAnimIntToInt(frame, 0, frame, anim, easing));
        }
    }
}
