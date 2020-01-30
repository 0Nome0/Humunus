using System;
using System.Collections.Generic;
using UnityEngine;
using AddAnimAction =
    System.Action<
        NerScript.Anime.Builder.BuilderObjectAnim,
        NerScript.Anime.AnimationPlanner,
        NerScript.Anime.Builder.ObjectAnimBuilder>;
using ObjAnim = NerScript.Anime.Builder.BuilderObjectAnim;
using Planner = NerScript.Anime.AnimationPlanner;
using AnimBuilder = NerScript.Anime.Builder.ObjectAnimBuilder;

namespace NerScript.Anime.Builder
{
    using AddAnimDictionary = Dictionary<AnimationType, AddAnimAction>;

    internal class BuilderAnimAdder
    {
        private static BuilderAnimAdder instance = null;
        private static BuilderAnimAdder Instance => instance ?? CreateInstance();
        private static BuilderAnimAdder CreateInstance() => instance = new BuilderAnimAdder();

        internal static void AddAnimation(ObjAnim anim, Planner planner, AnimBuilder builder)
        {
            Instance.animDictionary[anim.type](anim, planner, builder);
        }

        internal AddAnimDictionary animDictionary = new AddAnimDictionary();

        private BuilderAnimAdder()
        {
            animDictionary[AnimationType.OnEnd] = (a, p, b) => p.PlayActionAnim(a.action);
            animDictionary[AnimationType.MoveAbs] = (a, p, b) => p.MoveAbs(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.MoveRel] = (a, p, b) => p.MoveRel(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.RotateAbs] = (a, p, b) => p.RotateAbs(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.RotateRel] = (a, p, b) => p.RotateRel(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.ScaleAbs] = (a, p, b) => p.ScaleAbs(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.ScaleRel] = (a, p, b) => p.ScaleRel(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.LclMoveAbs] = (a, p, b) => p.LclMoveAbs(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.LclMoveRel] = (a, p, b) => p.LclMoveRel(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.LclRotateAbs] = (a, p, b) => p.LclRotateAbs(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.LclRotateRel] = (a, p, b) => p.LclRotateRel(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.AddPosition] = (a, p, b) => p.AddPosition(a.vector, a.frame, a.easing);
            animDictionary[AnimationType.RectSizeAbs] = (a, p, b) =>
            {
                RectTransform rect = b.GetComponent<RectTransform>();
                p
               .Simultaneous(pp =>
                    pp.FloatToFloatAnim(a.frame, rect.sizeDelta.x, a.vector.x,
                    (f) => rect.sizeDelta = new Vector2(f, rect.sizeDelta.y), a.easing))
               .FloatToFloatAnim(a.frame, rect.sizeDelta.y, a.vector.y,
                    (f) => rect.sizeDelta = new Vector2(rect.sizeDelta.x, f), a.easing);
            };
            animDictionary[AnimationType.RectSizeRel] = (a, p, b) =>
            {
                RectTransform rect = b.GetComponent<RectTransform>();
                p
               .Simultaneous(pp =>
                    pp.FloatToFloatAnim(a.frame, rect.sizeDelta.x, rect.sizeDelta.x + a.vector.x,
                    (f) => rect.sizeDelta = new Vector2(f, rect.sizeDelta.y), a.easing))
               .FloatToFloatAnim(a.frame, rect.sizeDelta.y, rect.sizeDelta.y + a.vector.y,
                    (f) => rect.sizeDelta = new Vector2(rect.sizeDelta.x, f), a.easing);
            };
            animDictionary[AnimationType.WaitFrame] = (a, p, b) => p.WaitFrame(a.frame);
            animDictionary[AnimationType.PlayActionAnim] = (a, p, b) => p.PlayActionAnim(a.action);
            animDictionary[AnimationType.PlayBuildedAnim] = (a, p, b) => AddPlayBuildedAnim(a, p, b);
            animDictionary[AnimationType.Simultaneous] = (a, p, b) => p.Simultaneous(pl => pl = b.GetPlanner());
            animDictionary[AnimationType.AsSoonAnim] = (a, p, b) => p.Simultaneous(pl => animDictionary[a.soonAnimType](a, pl, b));
            animDictionary[AnimationType.Repeat] = (a, p, b) => p.RepeatAnim(a.frame);
            animDictionary[AnimationType.Endless] = (a, p, b) => p.EndlessAnim();
        }

        private void AddPlayBuildedAnim(ObjAnim anim, Planner planner, AnimBuilder builder)
        {
            for (int i = 0; i < anim.components.Count; i++)
            {
                if (anim.components[i] == null) continue;
                AnimBuilder b = (AnimBuilder)anim.components[i];
                string name = anim.strings[i];

                if (name == "") planner.PlayActionAnim(() => { b.StartAnim(); });
                else planner.PlayActionAnim(() => { b.StartAnimWithName(name); });

                if (anim.frame != 0) planner.WaitFrame(anim.frame);
            }
        }
    }
}