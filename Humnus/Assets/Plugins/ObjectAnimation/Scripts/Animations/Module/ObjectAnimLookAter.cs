using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using NerScript.Accessory;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimLookAter : ObjectAnimBase, IObjectAnimModule, IObjectAnimAccessoryAttacher
    {
        public ObjectAnimModuleType ModuleType => ObjectAnimModuleType.Accessory;
        public ObjectAccessoryType AccessoryType => ObjectAccessoryType.LookAter;

        public bool isDetach { get; set; } = false;
        private readonly Transform targetTransform = null;
        private readonly Vector3 targetVector = new Vector3();

        internal ObjectAnimLookAter(bool _isDetach, Transform _tr)
            : base()
        {
            Name = "LookAt";
            isDetach = _isDetach;
            targetTransform = _tr;
            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                if (isDetach) UnityEngine.Object.Destroy(transform.GetComponent<SimpleLookAter>());
                else
                {
                    var look = transform.gameObject.AddComponent<SimpleLookAter>();
                    look.SetTransform(targetTransform);
                }
            };
        }
        internal ObjectAnimLookAter(bool _isDetach, Vector3 _pos)
           : base()
        {
            isDetach = _isDetach;
            targetVector = _pos;
            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                if (isDetach) UnityEngine.Object.Destroy(transform.GetComponent<SimpleLookAter>());
                else
                {
                    var look = transform.gameObject.AddComponent<SimpleLookAter>();
                    look.SetVector3(targetVector);
                }
            };
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            if (targetTransform == null) return new ObjectAnimLookAter(isDetach, targetVector);
            else return new ObjectAnimLookAter(isDetach, targetTransform);
        }

        protected override void ExitAnim(Transform tr)
        {

        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner AttachLookAt(this AnimationPlanner animation, Transform tr)
        {
            return animation.AddAnimation(new ObjectAnimLookAter(false, tr));
        }
        public static AnimationPlanner AttachLookAt(this AnimationPlanner animation, Vector3 pos)
        {
            return animation.AddAnimation(new ObjectAnimLookAter(false, pos));
        }

        public static AnimationPlanner DetachLookAt(this AnimationPlanner animation)
        {
            return animation.AddAnimation(new ObjectAnimLookAter(true, null));
        }
    }
}
