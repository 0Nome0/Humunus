using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class BezierMoveAnim : ObjectAnimBase
    {
        private readonly Func<Transform, Vector3> endpos = null;
        private Vector3Line line;
        private readonly List<Vector3> bezierPoints = null;

        internal BezierMoveAnim(List<Vector3> _bezierPoints, int frame, Func<Transform, Vector3> _endpos, EasingTypes easing)
            : base(frame, easing)
        {
            endpos = _endpos;
            bezierPoints = new List<Vector3>(_bezierPoints);

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                line = new Vector3Line(transform.position, endpos(transform));
                bezierPoints.Add(line.end);
                bezierPoints.Insert(0, line.start);

                anim = GetUpdate(transform, () =>
                {
                    transform.position = GetPosition(TLeapC);
                });
            };
        }


        private Vector3 GetPosition(float leap)
        {
            List<Vector3> poss = new List<Vector3>(bezierPoints);

            while (1 < poss.Count)
            {
                for (int i = 0; i < poss.Count - 1; i++)
                {
                    poss[i] = Vector3.LerpUnclamped(poss[i], poss[i + 1], leap);
                }
                poss.RemoveLast();
            }
            return poss[0];
        }

        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                tr.position = GetPosition(EasingLast);
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new BezierMoveAnim(bezierPoints, frame, endpos, easing);
        }

        internal static BezierMoveAnim CreateAbs(Vector3 position, List<Vector3> bezierPoints, int frame, EasingTypes easing)
        {
            return new BezierMoveAnim(bezierPoints, frame, (t) => position, easing) { Name = "MoveAbs" };
        }

        internal static BezierMoveAnim CreateRel(Vector3 vector, List<Vector3> bezierPoints, int frame, EasingTypes easing)
        {
            return new BezierMoveAnim(bezierPoints, frame, (tr) => (tr.position + vector), easing) { Name = "MoveRel" };
        }

        protected override void ExitAnim(Transform tr)
        {
            tr.position = line.end;
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner BezierMoveAbs(
            this AnimationPlanner animation, Func<Vector3> pos, List<Vector3> bezierPoints, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(new BezierMoveAnim(bezierPoints, frame, (tr) => pos(), easing) { Name = "MoveAbs" });
        }
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner BezierMoveAbs(
            this AnimationPlanner animation, Vector3 position, List<Vector3> bezierPoints, int frame,
            EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(BezierMoveAnim.CreateAbs(position, bezierPoints, frame, easing));
        }
        public static AnimationPlanner BezierMoveAbs(
            this AnimationPlanner animation, float posX, float posY, float posZ, List<Vector3> bezierPoints, int frame,
            EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(BezierMoveAnim.CreateAbs(new Vector3(posX, posY, posZ), bezierPoints, frame, easing));
        }

        public static AnimationPlanner BezierMoveRel(
            this AnimationPlanner animation, Vector3 vector, List<Vector3> bezierPoints, int frame,
            EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(BezierMoveAnim.CreateRel(vector, bezierPoints, frame, easing));
        }
        public static AnimationPlanner BezierMoveRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, List<Vector3> bezierPoints, int frame,
            EasingTypes easing = EasingTypes.LineIn)
        {
            return animation.AddAnimation(BezierMoveAnim.CreateRel(new Vector3(vecX, vecY, vecZ), bezierPoints, frame, easing));
        }
    }
}