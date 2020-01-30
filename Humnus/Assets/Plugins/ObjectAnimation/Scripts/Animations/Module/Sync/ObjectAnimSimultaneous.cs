using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimSimultaneous : ObjectAnimBase, IObjectAnimModule
    {
        public ObjectAnimModuleType ModuleType => ObjectAnimModuleType.Async;

        private readonly Action<AnimationPlanner, ObjectAnimationMemoryUser> process = null;
        internal Pointer<ObjectAnimationController> controller = new Pointer<ObjectAnimationController>(null);

        internal ObjectAnimSimultaneous(Action<AnimationPlanner, ObjectAnimationMemoryUser> act)
            : base()
        {
            Name = "Simultaneous";
            process = act;
            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                AnimationPlanner planner = transform.gameObject.ObjectAnimation();
                process(planner, new ObjectAnimationMemoryUser(planner.memory));
                if (planner.isStart) return;
                controller.Value = planner.AnimationStart();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimSimultaneous(process) { controller = controller };
        }

        protected override void ExitAnim(Transform tr)
        {
            controller.Value.Exit();
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner Simultaneous(this AnimationPlanner animation, Action<AnimationPlanner, ObjectAnimationMemoryUser> act)
        {
            var anim = new ObjectAnimSimultaneous(act);
            animation.AddAnimation(anim);
            animation.asyncAnims.Add(anim.controller);
            return animation;
        }

        public static AnimationPlanner Simultaneous(this AnimationPlanner animation, Action<AnimationPlanner> act)
        {
            var anim = new ObjectAnimSimultaneous((p, m) => act(p));
            animation.AddAnimation(anim);
            animation.asyncAnims.Add(anim.controller);
            return animation;
        }
    }
}
