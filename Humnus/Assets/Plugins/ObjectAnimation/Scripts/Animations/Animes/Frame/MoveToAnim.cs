using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class MoveToAnim : ObjectAnimBase
    {
        private readonly Transform targetTransform;
        private Vector3 position;

        internal MoveToAnim(Transform _target, int frame, EasingTypes easing)
            : base(frame, easing)
        {
            Name = "MoveToTransform";
            targetTransform = _target;
            position = Vector3.zero;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                position = transform.position;
                anim = GetUpdate(transform, () =>
                {
                    transform.position = Vector3.LerpUnclamped(position, targetTransform.position, TLeapC);
                });
            };
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.position = Vector3.LerpUnclamped(position, targetTransform.position, EasingLast);
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new MoveToAnim(targetTransform, frame, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.position = targetTransform.position;
        }
    }


    public static partial class ObjectAnim
    {
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner MoveTo(
            this AnimationPlanner animation, Transform transform, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new MoveToAnim(transform, frame, easing));
        }
    }
}
