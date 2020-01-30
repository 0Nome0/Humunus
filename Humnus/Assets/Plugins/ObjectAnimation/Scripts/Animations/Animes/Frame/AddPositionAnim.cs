using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class AddPositionAnim : ObjectAnimBase
    {
        private readonly Vector3 velocity;

        internal AddPositionAnim(int frame, Vector3 _velocity, EasingTypes easing)
            : base(frame, easing)
        {
            Name = "AddPosition";
            velocity = _velocity;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                anim = GetUpdate(transform, () =>
                {
                    transform.position += velocity * TLeapD;
                });
            };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.position += velocity * TLeapL;
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new AddPositionAnim(frame, velocity, easing);
        }
    }


    public static partial class ObjectAnim
    {
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner AddPosition(
            this AnimationPlanner animation, Vector3 velocity, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new AddPositionAnim(frame, velocity, easing));
        }
        public static AnimationPlanner AddPosition(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new AddPositionAnim(frame, new Vector3(vecX, vecY, vecZ), easing));
        }
    }
}
