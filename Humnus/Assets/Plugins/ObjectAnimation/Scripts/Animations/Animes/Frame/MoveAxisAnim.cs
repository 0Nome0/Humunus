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
    internal sealed class MoveAxisAnim : ObjectAnimBase
    {
        private readonly Surface surface;
        private readonly float velocity;
        private Vector3Line line;

        internal MoveAxisAnim(Surface _surface, float _velocity, int frame, EasingTypes easing)
            : base(frame, easing)
        {
            surface = _surface;
            velocity = _velocity;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                line = new Vector3Line();
                if (surface == Surface.Forward)
                    line = new Vector3Line(transform.position, transform.position + (transform.forward * velocity));
                else if (surface == Surface.Up)
                    line = new Vector3Line(transform.position, transform.position + (transform.up * velocity));
                else if (surface == Surface.Right)
                    line = new Vector3Line(transform.position, transform.position + (transform.right * velocity));

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
            return new MoveAxisAnim(surface, velocity, frame, easing);
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.position = line.end;
        }
    }


    public static partial class ObjectAnim
    {
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner MoveAxisFowerd(
            this AnimationPlanner animation, float velocity, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return
                animation.AddAnimation(new MoveAxisAnim(Surface.Forward, velocity, frame, easing) { Name = "MoveAxisFowerd" });
        }
        public static AnimationPlanner MoveAxisUp(
            this AnimationPlanner animation, float velocity, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new MoveAxisAnim(Surface.Up, velocity, frame, easing) { Name = "MoveAxisUp" });
        }
        public static AnimationPlanner MoveAxisRight(
            this AnimationPlanner animation, float velocity, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new MoveAxisAnim(Surface.Right, velocity, frame, easing) { Name = "MoveAxisRight" });
        }

    }
}
