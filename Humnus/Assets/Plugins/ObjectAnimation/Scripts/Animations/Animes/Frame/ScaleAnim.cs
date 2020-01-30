using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ScaleAnim : ObjectAnimBase
    {
        private readonly Func<Transform, Vector3> endscl = null;
        private Vector3Line line;

        internal ScaleAnim(int frame, Func<Transform, Vector3> _endscl, EasingTypes easing)
            : base(frame, easing)
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
            return new ScaleAnim(frame, endscl, easing);
        }

        internal static ScaleAnim CreateAbs(Vector3 scale, int frame, EasingTypes easing)
        {
            return new ScaleAnim(frame, (t) => scale, easing) { Name = "ScaleAbs" };
        }

        internal static ScaleAnim CreateRel(Vector3 vector, int frame, EasingTypes easing)
        {
            return new ScaleAnim(frame, (tr) =>
            {
                Vector3 scale = tr.localScale;
                scale.x *= vector.x;
                scale.y *= vector.y;
                scale.z *= vector.z;
                return scale;
            }, easing)
            { Name = "ScaleRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.localScale = line.end;
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner ScaleAbs(
            this AnimationPlanner animation, Func<Vector3> scl, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new ScaleAnim(frame, (t) => scl(), easing) { Name = "ScaleAbs" });
        }
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner ScaleAbs(
            this AnimationPlanner animation, Vector3 scale, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(ScaleAnim.CreateAbs(scale, frame, easing));
        }
        public static AnimationPlanner ScaleAbs(
            this AnimationPlanner animation, float sclX, float sclY, float sclZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(ScaleAnim.CreateAbs(new Vector3(sclX, sclY, sclZ), frame, easing));
        }
        public static AnimationPlanner ScaleAbs(
            this AnimationPlanner animation, float scl, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(ScaleAnim.CreateAbs(new Vector3(scl, scl, scl), frame, easing));
        }

        public static AnimationPlanner ScaleRel(
            this AnimationPlanner animation, Vector3 vector, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(ScaleAnim.CreateRel(vector, frame, easing));
        }
        public static AnimationPlanner ScaleRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(ScaleAnim.CreateRel(new Vector3(vecX, vecY, vecZ), frame, easing));
        }
        public static AnimationPlanner ScaleRel(
            this AnimationPlanner animation, float vec, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(ScaleAnim.CreateRel(new Vector3(vec, vec, vec), frame, easing));
        }
    }
}
