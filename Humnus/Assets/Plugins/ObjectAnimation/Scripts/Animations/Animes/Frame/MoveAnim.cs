using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class MoveAnim : ObjectAnimBase
    {
        private readonly Func<Transform, Vector3> endpos = null;
        private Vector3Line line;

        internal MoveAnim(int frame, Func<Transform, Vector3> _endpos, EasingTypes easing)
            : base(frame, easing)
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
            return new MoveAnim(frame, endpos, easing);
        }

        internal static MoveAnim CreateAbs(Vector3 position, int frame, EasingTypes easing)
        {
            return new MoveAnim(frame, (t) => position, easing) { Name = "MoveAbs" };
        }

        internal static MoveAnim CreateRel(Vector3 vector, int frame, EasingTypes easing)
        {
            return new MoveAnim(frame, (tr) => (tr.position + vector), easing) { Name = "MoveRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.position = line.end;
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner MoveAbs(
            this AnimationPlanner animation, Func<Vector3> pos, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new MoveAnim(frame, (tr) => pos(), easing) { Name = "MoveAbs" });
        }
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner MoveAbs(
            this AnimationPlanner animation, Vector3 position, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(MoveAnim.CreateAbs(position, frame, easing));
        }
        public static AnimationPlanner MoveAbs(
            this AnimationPlanner animation, float posX, float posY, float posZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(MoveAnim.CreateAbs(new Vector3(posX, posY, posZ), frame, easing));
        }

        public static AnimationPlanner MoveRel(
            this AnimationPlanner animation, Vector3 vector, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(MoveAnim.CreateRel(vector, frame, easing));
        }
        public static AnimationPlanner MoveRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(MoveAnim.CreateRel(new Vector3(vecX, vecY, vecZ), frame, easing));
        }
    }
}