using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class FixedWaitAnim : ObjectFixedAnimBase
    {
        internal FixedWaitAnim(float second)
            : base(second)
        {
            Name = "FixedWait";
            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                anim = GetUpdate(transform, () => Anim());
            };
        }

        protected override void ExitAnim(Transform tr)
        {

        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new FixedWaitAnim(second);
        }
    }



    public static partial class ObjectAnim
    {
        public static AnimationPlanner FixedWait(this AnimationPlanner animation, float second)
        {
            if (second <= 0) return animation;
            return animation.AddAnimation(new FixedWaitAnim(second));
        }
    }
}