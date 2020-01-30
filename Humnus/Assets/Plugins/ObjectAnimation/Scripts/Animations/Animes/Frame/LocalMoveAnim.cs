using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class LocalMoveAnim : ObjectAnimBase
    {
        private readonly Func<Transform, Vector3> endpos = null;
        private Vector3Line line;

        internal LocalMoveAnim(int frame, Func<Transform, Vector3> _endpos, EasingTypes easing)
            : base(frame, easing)
        {
            endpos = _endpos;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                line = new Vector3Line(transform.localPosition, endpos(transform));
                anim = GetUpdate(transform, () =>
                {
                    transform.localPosition = Vector3.LerpUnclamped(line.start, line.end, TLeapC);
                });
            };
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.localPosition = Vector3.LerpUnclamped(line.start, line.end, EasingLast);
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new LocalMoveAnim(frame, endpos, easing);
        }

        internal static LocalMoveAnim CreateAbs(Vector3 localPosition, int frame, EasingTypes easing)
        {
            return new LocalMoveAnim(frame, (t) => localPosition, easing) { Name = "LocalMoveAbs" };
        }

        internal static LocalMoveAnim CreateRel(Vector3 vector, int frame, EasingTypes easing)
        {
            return new LocalMoveAnim(frame, (tr) => (tr.localPosition + vector), easing) { Name = "LocalMoveRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.localPosition = line.end;
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner LclMoveAbs(
            this AnimationPlanner animation, Func<Vector3> pos, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new LocalMoveAnim(frame, (tr) => pos(), easing) { Name = "LocalMoveAbs" });
        }
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="localPosition"></param>
        public static AnimationPlanner LclMoveAbs(
            this AnimationPlanner animation, Vector3 localPosition, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalMoveAnim.CreateAbs(localPosition, frame, easing));
        }
        public static AnimationPlanner LclMoveAbs(
            this AnimationPlanner animation, float posX, float posY, float posZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalMoveAnim.CreateAbs(new Vector3(posX, posY, posZ), frame, easing));
        }

        public static AnimationPlanner LclMoveRel(
            this AnimationPlanner animation, Vector3 vector, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalMoveAnim.CreateRel(vector, frame, easing));
        }
        public static AnimationPlanner LclMoveRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalMoveAnim.CreateRel(new Vector3(vecX, vecY, vecZ), frame, easing));
        }
    }
}
