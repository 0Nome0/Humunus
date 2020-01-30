using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimPlayAction : ObjectAnimBase, IObjectAnimModule
    {
        public ObjectAnimModuleType ModuleType => ObjectAnimModuleType.Action;

        private readonly UnityEvent process = null;

        internal ObjectAnimPlayAction(Action act)
            : base()
        {
            Name = "PlayAction";
            process = new UnityEvent();
            process.AddListener(new UnityAction(act));
            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                process.Invoke();
                onAnimeEnd.OnNext();
            };
        }

        internal ObjectAnimPlayAction(UnityEvent eve)
           : base()
        {
            process = eve;
            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                process.Invoke();
                onAnimeEnd.OnNext();
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimPlayAction(process);
        }

        protected override void ExitAnim(Transform tr)
        {
            process.Invoke();
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner PlayActionAnim(this AnimationPlanner animation, Action act)
        {
            return animation.AddAnimation(new ObjectAnimPlayAction(act));
        }
        public static AnimationPlanner PlayActionAnim(this AnimationPlanner animation, UnityEvent eve)
        {
            return animation.AddAnimation(new ObjectAnimPlayAction(eve));
        }
    }
}
