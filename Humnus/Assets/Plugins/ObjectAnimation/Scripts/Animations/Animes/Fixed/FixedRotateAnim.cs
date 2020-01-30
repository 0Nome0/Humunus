using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class FixedRotateAnim : ObjectFixedAnimBase
    {
        private readonly Func<Transform, Vector3> endrot = null;
        private Vector3Line line;

        internal FixedRotateAnim(float second, Func<Transform, Vector3> _endrot, EasingTypes easing)
            : base(second, easing)
        {
            endrot = _endrot;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                line = new Vector3Line(transform.eulerAngles, endrot(transform));
                anim = GetUpdate(transform, () =>
                {
                    transform.eulerAngles = Vector3.LerpUnclamped(line.start, line.end, TLeapC);
                });
            };
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.eulerAngles = Vector3.LerpUnclamped(line.start, line.end, EasingLast);
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new FixedRotateAnim(second, endrot, easing);
        }

        internal static FixedRotateAnim CreateAbs(Vector3 rotation, float second, EasingTypes easing)
        {
            return new FixedRotateAnim(second, (t) => rotation, easing) { Name = "FixedRotateAbs" };
        }

        internal static FixedRotateAnim CreateRel(Vector3 vector, float second, EasingTypes easing)
        {
            return new FixedRotateAnim(second, (tr) => (tr.eulerAngles + vector), easing) { Name = "FixedRotateRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.localEulerAngles = endrot(tr);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner RotateAbs(
            this AnimationPlanner animation, Func<Vector3> rot, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new FixedRotateAnim(second, (tr) => rot(), easing) { Name = "FixedRotateAbs" });
        }

        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="rotation"></param>
        public static AnimationPlanner FixedRotateAbs(
            this AnimationPlanner animation, Vector3 rotation, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedRotateAnim.CreateAbs(rotation, second, easing));
        }
        public static AnimationPlanner FixedRotateAbs(
            this AnimationPlanner animation, float rotX, float rotY, float rotZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedRotateAnim.CreateAbs(new Vector3(rotX, rotY, rotZ), second, easing));
        }

        public static AnimationPlanner FixedRotateRel(
            this AnimationPlanner animation, Vector3 vector, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedRotateAnim.CreateRel(vector, second, easing));
        }
        public static AnimationPlanner FixedRotateRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedRotateAnim.CreateRel(new Vector3(vecX, vecY, vecZ), second, easing));
        }
    }
}