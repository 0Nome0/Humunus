using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NerScript.Anime.Builder
{
    [Serializable]
    public partial class BuilderObjectAnim
    {
        public bool enabled = true;
        public AnimationType type;
        public Vector3 vector = new Vector3();
        public int frame = 1;
        public EasingTypes easing = EasingTypes.LineIn;
        public string str = "";
        public UnityEvent action = new UnityEvent();
        public AnimationType soonAnimType = AnimationType.None;
        [SerializeReference]
        public List<Component> components = null;
        public List<string> strings = null;

        public bool foldOut = false;
        public BuilderObjectAnim(AnimationType _type)
        {
            type = _type;
            components = new List<Component>() { default };
            strings = new List<string>() { default };
        }
        public BuilderObjectAnim(BuilderObjectAnim anim)
        {
            enabled = anim.enabled;
            type = anim.type;
            vector = anim.vector;
            frame = anim.frame;
            easing = anim.easing;
            str = anim.str;
            foldOut = anim.foldOut;
            action = new UnityEvent();
            soonAnimType = anim.soonAnimType;
            components = new List<Component>(anim.components);
            strings = new List<string>(anim.strings);
        }
        public BuilderObjectAnim GetCopy()
        {
            return new BuilderObjectAnim(this);
        }

        public void AddToPlanner(ObjectAnimationPlanner planner, ObjectAnimBuilder builder)
        {
            BuilderAnimAdder.AddAnimation(this, planner, builder);
        }
    }
}