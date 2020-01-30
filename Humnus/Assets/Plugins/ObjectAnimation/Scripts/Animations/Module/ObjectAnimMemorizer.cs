using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimMemorizer : ObjectAnimBase, IObjectAnimModule
    {
        public ObjectAnimModuleType ModuleType => ObjectAnimModuleType.Memorys;
        public ObjectAnimMemoryType memoryType;

        public ObjectAnimationMemory memory = null;

        internal ObjectAnimMemorizer(ObjectAnimMemoryType _memoryType, ObjectAnimationMemory _memory)
            : base()
        {
            Name = "Memorize";
            memoryType = _memoryType;
            memory = _memory;
            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                Memorize(transform);
            };
        }

        private void Memorize(Transform tr)
        {
            memory = memory ?? tr.GetComponent<ObjectAnimation>().memory;
            switch (memoryType)
            {
                case ObjectAnimMemoryType.Position: memory.Memorize(memoryType, tr.position); break;
                case ObjectAnimMemoryType._Position: memory.Memorize(memoryType, tr.position); break;
                case ObjectAnimMemoryType.LocalPosition: memory.Memorize(memoryType, tr.position); break;
                case ObjectAnimMemoryType._LocalPosition: memory.Memorize(memoryType, tr.localPosition); break;

                case ObjectAnimMemoryType.Rotation: memory.Memorize(memoryType, tr.eulerAngles); break;
                case ObjectAnimMemoryType._Rotation: memory.Memorize(memoryType, tr.eulerAngles); break;
                case ObjectAnimMemoryType.LocalRotation: memory.Memorize(memoryType, tr.localEulerAngles); break;
                case ObjectAnimMemoryType._LocalRotation: memory.Memorize(memoryType, tr.localEulerAngles); break;

                case ObjectAnimMemoryType.Scale: memory.Memorize(memoryType, tr.localScale); break;
                case ObjectAnimMemoryType._Scale: memory.Memorize(memoryType, tr.localScale); break;
            }
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return new ObjectAnimMemorizer(memoryType, memory);
        }

        protected override void ExitAnim(Transform tr)
        {
            Memorize(tr);
        }
    }


    public static partial class ObjectAnim
    {
        public static AnimationPlanner MemorizeAnim(this AnimationPlanner animation, ObjectAnimMemoryType type)
        {
            return animation.AddAnimation(new ObjectAnimMemorizer(type, null));
        }
        internal static AnimationPlanner MemorizeAnim(this AnimationPlanner animation, ObjectAnimMemoryType type, ObjectAnimationMemory memory)
        {
            return animation.AddAnimation(new ObjectAnimMemorizer(type, memory));
        }
    }
}
