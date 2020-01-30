using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal sealed class ObjectAnimRepeat : ObjectAnimBase, IObjectAnimModule
    {
        public ObjectAnimModuleType ModuleType => ObjectAnimModuleType.Sync;

        private readonly List<IObjectAnim> animations = null;
        private int _nowAnimation = 0;
        private int loopCount = 0;
        private IObjectAnim nowAnimation = null;

        private readonly Func<ObjectAnimRepeat> getClone = null;

        #region onAnimation
        private readonly Subject<Unit> onNowAnimationEnd = new Subject<Unit>();

        private readonly Subject<Unit> onNowAnimationStart = new Subject<Unit>();
        #endregion

        public ObjectAnimRepeat(List<IObjectAnim> repeatAnimations, int roop) : base()
        {
            Name = $"Repeat({roop})";
            animations = repeatAnimations;

            getClone = () => new ObjectAnimRepeat(repeatAnimations, roop);

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                onNowAnimationStart.Subscribe(_ =>
                {
                    nowAnimation = animations[_nowAnimation].GetClone();

                    if (nowAnimation is IObjectAnimModule &&
                        (nowAnimation as IObjectAnimModule).ModuleType == ObjectAnimModuleType.Async)
                    {
                        nowAnimation.animation(transform, animationPlayDetail);
                        nowAnimation = null;
                        onNowAnimationEnd.OnNext();
                        return;
                    }

                    nowAnimation.animation(transform, animationPlayDetail);

                    nowAnimation
                    .OnAnimeEnd
                    .Subscribe(__ =>
                    {
                        nowAnimation.Dispose();
                        nowAnimation = null;
                        onNowAnimationEnd.OnNext();
                    });
                });


                onNowAnimationEnd.Subscribe(_ =>
                {
                    if (NextAnim())
                    {
                        if (NextRoop(roop))
                        {
                            onAnimeEnd.OnNext();
                            return;
                        }
                    }
                    onNowAnimationStart.OnNext();
                });


                onNowAnimationStart.OnNext();
            };
        }

        /// <summary>
        /// animationend?
        /// </summary>
        private bool NextRoop(int roopMax)
        {
            _nowAnimation = 0;
            loopCount++;
            return (roopMax - 1 <= loopCount) && (roopMax != -1);
        }

        /// <summary>
        /// nextroop?
        /// </summary>
        /// <returns></returns>
        private bool NextAnim()
        {
            _nowAnimation++;
            return animations.Count <= _nowAnimation;
        }

        protected override ObjectAnimRoot GetAnimClone()
        {
            return getClone();
        }

        protected override void ExitAnim(Transform transform)
        {
            nowAnimation?.Exit(transform);
            for (int i = _nowAnimation + 1; i < animations.Count; i++)
            {
                var anim = animations[i];
                anim.animation(transform, animationPlayDetail);
                anim.Exit(transform);
            }
        }

        public override void Dispose()
        {
            nowAnimation?.Dispose();
        }
        public override void Stop()
        {
            nowAnimation?.Stop();
        }
        public override void Continue()
        {
            nowAnimation?.Continue();
        }

    }
    public static partial class ObjectAnim
    {
        public static AnimationPlanner RepeatAnim(this AnimationPlanner animation, int count)
        {
            if (animation.animations.Count == 0) return animation;
            var repeat = new ObjectAnimRepeat(animation.animations.ToList(), count);
            animation.RemoveAllAnimation();
            return animation.AddAnimation(repeat);
        }

        public static AnimationPlanner EndlessAnim(this AnimationPlanner animation)
        {
            if (animation.animations.Count == 0) return animation;
            var repeat = new ObjectAnimRepeat(animation.animations.ToList(), -1);
            animation.RemoveAllAnimation();
            return animation.AddAnimation(repeat);
        }
    }
}
