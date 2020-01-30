using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class RotateAnim : ObjectAnimBase
    {
        private readonly Func<Transform, Vector3> endrot = null;
        private Vector3Line line;

        internal RotateAnim(int frame, Func<Transform, Vector3> _endrot, EasingTypes easing)
            : base(frame, easing)
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
            return new RotateAnim(frame, endrot, easing);
        }

        internal static RotateAnim CreateAbs(Vector3 rotation, int frame, EasingTypes easing)
        {
            return new RotateAnim(frame, (t) => rotation, easing) { Name = "RotateAbs" };
        }

        internal static RotateAnim CreateRel(Vector3 vector, int frame, EasingTypes easing)
        {
            return new RotateAnim(frame, (tr) => (tr.eulerAngles + vector), easing) { Name = "RotateRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.eulerAngles = line.end;
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner RotateAbs(
            this AnimationPlanner animation, Func<Vector3> rot, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new RotateAnim(frame, (tr) => rot(), easing) { Name = "RotateAbs" });
        }

        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="rotation"></param>
        public static AnimationPlanner RotateAbs(
            this AnimationPlanner animation, Vector3 rotation, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(RotateAnim.CreateAbs(rotation, frame, easing));
        }
        public static AnimationPlanner RotateAbs(
            this AnimationPlanner animation, float rotX, float rotY, float rotZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(RotateAnim.CreateAbs(new Vector3(rotX, rotY, rotZ), frame, easing));
        }

        public static AnimationPlanner RotateRel(
            this AnimationPlanner animation, Vector3 vector, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(RotateAnim.CreateRel(vector, frame, easing));
        }
        public static AnimationPlanner RotateRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(RotateAnim.CreateRel(new Vector3(vecX, vecY, vecZ), frame, easing));
        }
    }
}
