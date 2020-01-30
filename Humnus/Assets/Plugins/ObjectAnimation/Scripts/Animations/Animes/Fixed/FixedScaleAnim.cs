using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class FixedScaleAnim : ObjectFixedAnimBase
    {
        private readonly Func<Transform, Vector3> endscl = null;
        private Vector3Line line;

        internal FixedScaleAnim(float second, Func<Transform, Vector3> _endscl, EasingTypes easing)
            : base(second, easing)
        {
            endscl = _endscl;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                line = new Vector3Line(transform.localScale, endscl(transform));
                anim = GetUpdate(transform, () =>
                {
                    transform.localScale = Vector3.LerpUnclamped(line.start, line.end, TLeapC);
                });
            };
        }
        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.localScale = Vector3.LerpUnclamped(line.start, line.end, EasingLast);
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new FixedScaleAnim(second, endscl, easing);
        }

        internal static FixedScaleAnim CreateAbs(Vector3 scale, float second, EasingTypes easing)
        {
            return new FixedScaleAnim(second, (t) => scale, easing) { Name = "FixedScaleAbs" };
        }

        internal static FixedScaleAnim CreateRel(Vector3 vector, float second, EasingTypes easing)
        {
            return new FixedScaleAnim(second, (tr) =>
            {
                Vector3 scale = tr.localScale;
                scale.x *= vector.x;
                scale.y *= vector.y;
                scale.z *= vector.z;
                return scale;
            }, easing)
            { Name = "FixedScaleRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.localScale = endscl(tr);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner FixedScaleAbs(
            this AnimationPlanner animation, Func<Vector3> scl, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new FixedScaleAnim(second, (t) => scl(), easing) { Name = "FixedScaleAbs" });
        }
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner FixedScaleAbs(
            this AnimationPlanner animation, Vector3 scale, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedScaleAnim.CreateAbs(scale, second, easing));
        }
        public static AnimationPlanner FixedScaleAbs(
            this AnimationPlanner animation, float sclX, float sclY, float sclZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedScaleAnim.CreateAbs(new Vector3(sclX, sclY, sclZ), second, easing));
        }
        public static AnimationPlanner FixedScaleAbs(
            this AnimationPlanner animation, float scl, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedScaleAnim.CreateAbs(new Vector3(scl, scl, scl), second, easing));
        }

        public static AnimationPlanner FixedScaleRel(
            this AnimationPlanner animation, Vector3 vector, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedScaleAnim.CreateRel(vector, second, easing));
        }
        public static AnimationPlanner FixedScaleRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedScaleAnim.CreateRel(new Vector3(vecX, vecY, vecZ), second, easing));
        }
        public static AnimationPlanner FixedScaleRel(
            this AnimationPlanner animation, float vec, float second, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(FixedScaleAnim.CreateRel(new Vector3(vec, vec, vec), second, easing));
        }
    }
}
