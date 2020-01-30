using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class LookAtAnim : ObjectAnimBase
    {
        private new readonly Transform target = null;
        private Vector3Line line;

        internal LookAtAnim(int frame, Transform _target, EasingTypes easing)
            : base(frame, easing)
        {
            Name = "LookAt";
            target = _target;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                Vector3 look = target.transform.position - transform.position;
                line = new Vector3Line(transform.eulerAngles, Quaternion.LookRotation(look).eulerAngles);
                anim = GetUpdate(transform, () =>
                {
                    look = target.transform.position - transform.position;
                    line = new Vector3Line(transform.eulerAngles, Quaternion.LookRotation(look).eulerAngles);
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
            return new LookAtAnim(frame, target, easing);
        }


        protected override void ExitAnim(Transform tr)
        {
            tr.localEulerAngles = Vector3.LerpUnclamped(line.start, line.end, EasingLast);
        }
    }


    public static partial class ObjectAnim
    {

        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="rotation"></param>
        public static AnimationPlanner LookAt(
            this AnimationPlanner animation, Transform target, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new LookAtAnim(frame, target, easing));
        }
    }
}
