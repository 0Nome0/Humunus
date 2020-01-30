using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class LocalRotateAnim : ObjectAnimBase
    {
        private readonly Func<Transform, Vector3> endrot = null;
        private Vector3Line line;

        internal LocalRotateAnim(int frame, Func<Transform, Vector3> _endrot, EasingTypes easing)
            : base(frame, easing)
        {
            endrot = _endrot;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                line = new Vector3Line(transform.localEulerAngles, endrot(transform));
                anim = GetUpdate(transform, () =>
                {
                    transform.localEulerAngles = Vector3.LerpUnclamped(line.start, line.end, TLeapC);
                });
            };
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.localEulerAngles = Vector3.LerpUnclamped(line.start, line.end, EasingLast);
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new LocalRotateAnim(frame, endrot, easing);
        }

        internal static LocalRotateAnim CreateAbs(Vector3 rotation, int frame, EasingTypes easing)
        {
            return new LocalRotateAnim(frame, (t) => rotation, easing) { Name = "LocalRotateAbs" };
        }

        internal static LocalRotateAnim CreateRel(Vector3 vector, int frame, EasingTypes easing)
        {
            return new LocalRotateAnim(frame, (tr) => (tr.localEulerAngles + vector), easing) { Name = "LocalRotateRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.localEulerAngles = line.end;
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner LclRotateAbs(
            this AnimationPlanner animation, Func<Vector3> rot, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new LocalRotateAnim(frame, (tr) => rot(), easing) { Name = "LocalRotateAbs" });
        }

        /// <summary>
        /// Position of transform that gameObject has will move to position.
        /// </summary>
        public static AnimationPlanner LclRotateAbs(
            this AnimationPlanner animation, Vector3 rotation, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalRotateAnim.CreateAbs(rotation, frame, easing));
        }
        public static AnimationPlanner LclRotateAbs(
            this AnimationPlanner animation, float rotX, float rotY, float rotZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalRotateAnim.CreateAbs(new Vector3(rotX, rotY, rotZ), frame, easing));
        }

        public static AnimationPlanner LclRotateRel(
            this AnimationPlanner animation, Vector3 vector, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalRotateAnim.CreateRel(vector, frame, easing));
        }
        public static AnimationPlanner LclRotateRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(LocalRotateAnim.CreateRel(new Vector3(vecX, vecY, vecZ), frame, easing));
        }
    }
}
