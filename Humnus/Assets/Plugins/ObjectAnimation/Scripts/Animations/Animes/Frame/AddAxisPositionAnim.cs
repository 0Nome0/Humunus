using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    /// <summary>
    /// 使用不可
    /// </summary>
    internal sealed class AddAxisPositionAnim : ObjectAnimBase
    {
        private readonly Surface surface;
        private readonly float vel;
        private Vector3 velocity;

        internal AddAxisPositionAnim(Surface _surface, float _velocity, int frame, EasingTypes easing)
            : base(frame, easing)
        {
            surface = _surface;
            vel = _velocity;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                anim = GetUpdate(transform, () =>
                {
                    if (surface == Surface.Forward)
                        velocity = transform.forward * vel;
                    else if (surface == Surface.Up)
                        velocity = transform.up * vel;
                    else if (surface == Surface.Right)
                        velocity = transform.right * vel;

                    transform.position += velocity * TLeapD;
                });
            };
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.position += velocity * TLeapL;
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new AddAxisPositionAnim(surface, vel, frame, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.position += velocity * TLeapL;
        }
    }


    public static partial class ObjectAnim
    {
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner AddAxisFowerd(
            this AnimationPlanner animation, float velocity, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new AddAxisPositionAnim(Surface.Forward, velocity, frame, easing) { Name = "AddAxisFowerd" });
        }
        public static AnimationPlanner AddAxisUp(
            this AnimationPlanner animation, float velocity, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new AddAxisPositionAnim(Surface.Up, velocity, frame, easing) { Name = "AddAxisUp" });
        }
        public static AnimationPlanner AddAxisRight(
            this AnimationPlanner animation, float velocity, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new AddAxisPositionAnim(Surface.Right, velocity, frame, easing) { Name = "AddAxisRight" });
        }

    }
}
