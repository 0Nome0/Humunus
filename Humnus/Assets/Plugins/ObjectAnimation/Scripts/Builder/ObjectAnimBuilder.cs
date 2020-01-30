using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NerScript.Anime.Builder
{
    public class ObjectAnimBuilder : MonoBehaviour
    {
        public ObjectAnimBuilder buildedAnim = null;
        public string animName = "-null-";
        public bool showAnims = true;
        public BuilderObjectAnim onEnd = new BuilderObjectAnim(AnimationType.OnEnd) { enabled = false };
        private ObjectAnimationPlanner planner = null;
        public List<BuilderObjectAnim> anims = new List<BuilderObjectAnim>();
        public ObjectAnimationController controller = null;
        public BuilderObjectAnim this[int index]
        {
            get {
                if (index == anims.Count) return onEnd;
                return anims[index];
            }
            set {
                if (index == anims.Count) onEnd = value;
                anims[index] = value;
            }
        }

        private void Start()
        {


        }

        public void Init(string _name)
        {
            anims.Clear();
            animName = _name;
        }

        public ObjectAnimationPlanner GetPlanner()
        {
            planner = gameObject.ObjectAnimation();
            AddAnimToPlanner();
            return planner;
        }

        public void StartAnim()
        {
            planner = gameObject.ObjectAnimation();
            AddAnimToPlanner();
            controller = planner.AnimationStart();
        }

        public void Exit()
        {
            controller.Exit();
        }

        public void AddAnimToPlanner()
        {
            foreach (var anim in anims)
            {
                if (anim.enabled) anim.AddToPlanner(planner, this);
            }
            if (onEnd.enabled) onEnd.AddToPlanner(planner, this);
        }

        public void StartAnimWithName(string name)
        {
            List<ObjectAnimBuilder> builsers = GetComponents<ObjectAnimBuilder>().Where(b => b.animName == name).ToList();
            if (builsers.Count == 0) return;
            builsers[0].StartAnim();
        }
    }
}
