using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class FixedMoveAnim : ObjectFixedAnimBase
    {
        private readonly Func<Transform, Vector3> endpos = null;
        private Vector3Line line;

        internal FixedMoveAnim(float second, Func<Transform, Vector3> _endpos, EasingTypes easing)
            : base(second, easing)
        {
            endpos = _endpos;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                line = new Vector3Line(transform.position, endpos(transform));
                anim = GetUpdate(transform, () =>
                {
                    transform.position = Vector3.LerpUnclamped(line.start, line.end, TLeapC);
                });
            };
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.position = Vector3.LerpUnclamped(line.start, line.end, EasingLast);
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new FixedMoveAnim(second, endpos, easing);
        }

        internal static FixedMoveAnim CreateAbs(Vector3 position, float second, EasingTypes easing)
        {
            return new FixedMoveAnim(second, (t) => position, easing) { Name = "FixedMoveAbs" };
        }

        internal static FixedMoveAnim CreateRel(Vector3 vector, float second, EasingTypes easing)
        {
            return new FixedMoveAnim(second, (tr) => (tr.position + vector), easing) { Name = "FixedMoveRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.position = endpos(tr);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner FixedMoveAbs(
            this AnimationPlanner animation, Func<Vector3> pos, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new FixedMoveAnim(second, (tr) => pos(), easing) { Name = "FixedMoveAbs" });
        }
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner FixedMoveAbs(
            this AnimationPlanner animation, Vector3 position, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedMoveAnim.CreateAbs(position, second, easing));
        }
        public static AnimationPlanner FixedMoveAbs(
            this AnimationPlanner animation, float posX, float posY, float posZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedMoveAnim.CreateAbs(new Vector3(posX, posY, posZ), second, easing));
        }

        public static AnimationPlanner FixedMoveRel(
            this AnimationPlanner animation, Vector3 vector, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedMoveAnim.CreateRel(vector, second, easing));
        }
        public static AnimationPlanner FixedMoveRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedMoveAnim.CreateRel(new Vector3(vecX, vecY, vecZ), second, easing));
        }
    }
}