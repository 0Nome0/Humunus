using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimVector3ToVector3 : ObjectAnimBase
    {
        private readonly Action<Vector3> action = null;
        private readonly Vector3Line line;


        internal ObjectAnimVector3ToVector3(int frame, Vector3 start, Vector3 end, Action<Vector3> act, EasingTypes easing)
            : base(frame, easing)
        {
            Name = "Vector3ToVector3";
            action = act;
            line = new Vector3Line(start, end);

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
            action(Vector3.LerpUnclamped(line.start, line.end, lerp));
        }


        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimVector3ToVector3(frame, line.start, line.end, action, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            action(line.end);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner Vec3ToVec3Anim(
            this AnimationPlanner animation, int frame,
            Vector3 start, Vector3 end, Action<Vector3> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(
                new ObjectAnimVector3ToVector3(frame, start, end, anim, easing));
        }
        public static AnimationPlanner Vec3ToVec3Anim(
            this AnimationPlanner animation, int frame,
            Action<Vector3> anim, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(
                new ObjectAnimVector3ToVector3(
                    frame, Vector3.zero, new Vector3(frame, frame, frame), anim, easing));
        }
    }
}
