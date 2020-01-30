using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class FixedLocalMoveAnim : ObjectFixedAnimBase
    {
        private readonly Func<Transform, Vector3> endpos = null;
        private Vector3Line line;

        internal FixedLocalMoveAnim(float second, Func<Transform, Vector3> _endpos, EasingTypes easing)
            : base(second, easing)
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
            return new FixedLocalMoveAnim(second, endpos, easing);
        }

        internal static FixedLocalMoveAnim CreateAbs(Vector3 localPosition, float second, EasingTypes easing)
        {
            return new FixedLocalMoveAnim(second, (t) => localPosition, easing) { Name = "FixedLocalMoveAbs" };
        }

        internal static FixedLocalMoveAnim CreateRel(Vector3 vector, float second, EasingTypes easing)
        {
            return new FixedLocalMoveAnim(second, (tr) => (tr.localPosition + vector), easing) { Name = "FixedLocalMoveRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.localPosition = endpos(tr);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner FixedLclMoveAbs(
            this AnimationPlanner animation, Func<Vector3> pos, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new FixedLocalMoveAnim(second, (tr) => pos(), easing) { Name = "FixedLocalMoveAbs" });
        }
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="localPosition"></param>
        public static AnimationPlanner FixedLclMoveAbs(
            this AnimationPlanner animation, Vector3 localPosition, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedLocalMoveAnim.CreateAbs(localPosition, second, easing));
        }
        public static AnimationPlanner FixedLclMoveAbs(
            this AnimationPlanner animation, float posX, float posY, float posZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedLocalMoveAnim.CreateAbs(new Vector3(posX, posY, posZ), second, easing));
        }

        public static AnimationPlanner FixedLclMoveRel(
            this AnimationPlanner animation, Vector3 vector, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedLocalMoveAnim.CreateRel(vector, second, easing));
        }
        public static AnimationPlanner FixedLclMoveRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedLocalMoveAnim.CreateRel(new Vector3(vecX, vecY, vecZ), second, easing));
        }
    }
}