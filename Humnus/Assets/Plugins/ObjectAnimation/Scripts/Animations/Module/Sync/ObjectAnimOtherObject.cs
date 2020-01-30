using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimOtherObject : ObjectAnimBase, IObjectAnimModule
    {
        public ObjectAnimModuleType ModuleType => ObjectAnimModuleType.Async;

        private readonly Action<AnimationPlanner, ObjectAnimationMemoryUser> action = null;
        internal readonly Pointer<ObjectAnimationController> controller;
        private readonly GameObject obj = null;

        internal ObjectAnimOtherObject(GameObject _obj, Action<AnimationPlanner, ObjectAnimationMemoryUser> act)
            : base()
        {
            controller = new Pointer<ObjectAnimationController>(null);
            Name = "OtherObject";
            action = act;
            obj = _obj;

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                AnimationPlanner planner = obj.ObjectAnimation();
                action(planner, new ObjectAnimationMemoryUser(planner.memory));
                if (planner.isStart) return;
                controller.Value = planner.AnimationStart();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimOtherObject(obj, action);
        }

        protected override void ExitAnim(Transform tr)
        {
            if (controller.Value == null)
            {
                AnimationPlanner planner = obj.ObjectAnimation();
                action(planner, new ObjectAnimationMemoryUser(planner.memory));
                if (planner.isStart) return;
                controller.Value = planner.RepeatAnim(1).AnimationStart();
                Observable.NextFrame().TakeUntilDestroy(tr.gameObject).Subscribe(_ => controller.Value.Exit());
            }
            else
            {
                controller.Value.Exit();
            }
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner OtherObject(this AnimationPlanner animation, GameObject obj,
            Action<AnimationPlanner, ObjectAnimationMemoryUser> act)
        {
            var anim = new ObjectAnimOtherObject(obj, act);
            animation.AddAnimation(anim);
            animation.asyncAnims.Add(anim.controller);
            return animation;
        }

        public static AnimationPlanner OtherObject(this AnimationPlanner animation, GameObject obj,
            Action<AnimationPlanner> act)
        {
            var anim = new ObjectAnimOtherObject(obj, (p, m) => act(p));
            animation.AddAnimation(anim);
            animation.asyncAnims.Add(anim.controller);
            return animation;
        }
    }
}
