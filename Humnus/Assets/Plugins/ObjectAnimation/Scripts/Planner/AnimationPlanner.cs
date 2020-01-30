using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using Object = UnityEngine.Object;

namespace NerScript.Anime
{
    [Serializable]
    public class AnimationPlanner
    {
        internal bool isStart = false;
        internal ObjectAnimationOption option = null;
        internal ObjectAnimationMemory memory = null;
        internal List<Pointer<ObjectAnimationController>> asyncAnims = new List<Pointer<ObjectAnimationController>>();
        [SerializeField] internal Queue<IObjectAnim> animations = new Queue<IObjectAnim>();
        [SerializeField] internal GameObject gameObject = null;



        public AnimationPlanner(GameObject gameObject)
        {
            this.gameObject = gameObject;
            option = new ObjectAnimationOption();
            memory = new ObjectAnimationMemory();
        }

        internal AnimationPlanner AddAnimation(IObjectAnim animation)
        {
            animations.Enqueue(animation.GetClone());
            return this;
        }

        public AnimationPlanner AddAnimation(AnimationPlanner animation)
        {
            Queue<IObjectAnim> anims = new Queue<IObjectAnim>(animation.animations);
            foreach (var a in anims)
            {
                AddAnimation(a);
            }
            return this;
        }

        public AnimationPlanner RemoveAllAnimation()
        {
            animations.Clear();
            return this;
        }

        public ObjectAnimationController AnimationStart()
        {
            ObjectAnimation anime = StartAnim();
            return new ObjectAnimationController(anime, memory);
        }

        public ObjectAnimationController AnimationStart(Action onEnd)
        {
            ObjectAnimation anime = StartAnim();
            anime?.OnAnimationEnd.Subscribe(_ => onEnd?.Invoke());
            return new ObjectAnimationController(anime, memory);
        }

        private ObjectAnimation StartAnim()
        {
            if (animations.Count == 0 || gameObject == null) return null;
            isStart = true;
            if (StartCheck())
            {
                return null;
            }
            ObjectAnimation anime = gameObject.AddComponent<ObjectAnimation>();
            anime.option = option;
            anime.memory = memory;
            anime.animations = animations;
            anime.asyncAnims = asyncAnims;
            return anime;
        }

        private bool StartCheck()
        {
            ObjectAnimation anim = gameObject.GetComponent<ObjectAnimation>();
            if (option != null && option.removeOthers)
            {
                anim?.Exit();
                Debug.Log("DESTROY!");
                Object.Destroy(gameObject.GetComponent<ObjectAnimation>());
            }
            if (anim == null) return false;
            if (anim.option.rejectOthers) return true;
            return false;
        }
    }
}
