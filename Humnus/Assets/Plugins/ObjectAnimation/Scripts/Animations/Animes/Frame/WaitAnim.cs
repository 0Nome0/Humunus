using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class WaitAnim : ObjectAnimBase
    {
        internal WaitAnim(int frame)
            : base(frame)
        {
            Name = "Wait";
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
            return new WaitAnim(frame);
        }
    }



    public static partial class ObjectAnim
    {
        public static AnimationPlanner WaitFrame(this AnimationPlanner animation, int frame)
        {
            if (frame == 0) return animation;
            return animation.AddAnimation(new WaitAnim(frame));
        }
    }
}
