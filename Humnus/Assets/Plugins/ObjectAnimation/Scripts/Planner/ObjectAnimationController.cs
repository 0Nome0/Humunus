using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    public class ObjectAnimationController
    {
        private readonly ObjectAnimation animation = null;
        private readonly ObjectAnimationMemory memory = null;

        internal ObjectAnimationController(ObjectAnimation animation, ObjectAnimationMemory _memory)
        {
            this.animation = animation;
            memory = _memory;
        }

        public ObjectAnimationMemoryUser GetMemory() => new ObjectAnimationMemoryUser(memory);

        public void Exit()
        {
            if (animation == null) return;
            animation.Exit();
        }

        public void Dispose()
        {
            if (animation == null) return;
            animation.Dispose();
        }

        public void Stop()
        {
            if (animation == null) return;
            animation.Stop();
        }

        public void Continue()
        {
            if (animation == null) return;
            animation.Continue();
        }

        public void Destroy()
        {
            if (animation == null) return;
            animation.Destroy();
        }
    }
}
