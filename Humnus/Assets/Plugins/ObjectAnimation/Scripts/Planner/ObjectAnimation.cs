using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NerScript.Anime
{
    public class ObjectAnimation : MonoBehaviour
    {
        public static readonly string Version = "v2.2.0";


        [SerializeField] private string version = Version;
        [SerializeField] private int animCount = 0;
        [SerializeField] private string animName = "";
        [SerializeField, Range(0, 1)] private float progress = 0;
        private readonly AssignmentPointer<float> progressP = new AssignmentPointer<float>(0);



        //現在のアニメ
        private IObjectAnim nowAnimation = null;
        //これからするアニメのリスト
        internal Queue<IObjectAnim> animations = new Queue<IObjectAnim>();

        [SerializeField] internal ObjectAnimationOption option = null;
        internal List<Pointer<ObjectAnimationController>> asyncAnims = new List<Pointer<ObjectAnimationController>>();
        internal ObjectAnimationMemory memory = null;
        private bool startFlag = false;
        private bool endFlag = false;

        #region onAnimation
        private readonly Subject<Unit> onNowAnimationEnd = new Subject<Unit>();

        private readonly Subject<Unit> onNowAnimationStart = new Subject<Unit>();


        public IOptimizedObservable<Unit> OnAnimationEnd { get { return onAnimationEnd; } }
        private readonly Subject<Unit> onAnimationEnd = new Subject<Unit>();
        #endregion

        private void Start()
        {
            if (startFlag) return;
            startFlag = true;
            hideFlags = HideFlags.NotEditable;
            progressP.assignment += f => progress = f;

            onNowAnimationEnd.Subscribe(_ =>
            {
                if (0 < animations.Count)
                {
                    NextAnim();
                }
                else
                {
                    AnimEnd();
                    DestroyAnim();
                }
            });

            onNowAnimationStart.Subscribe(_ =>
            {
                if (nowAnimation is IObjectAnimModule &&
                    (nowAnimation as IObjectAnimModule).ModuleType == ObjectAnimModuleType.Async)
                {

                    nowAnimation.animation(transform, new AnimationPlayDetail() { progress = progressP });
                    onNowAnimationEnd.OnNext();
                    return;
                }

                nowAnimation.OnAnimeEnd
                .Subscribe(__ =>
                {
                    nowAnimation.Dispose();
                    onNowAnimationEnd.OnNext();
                });
                nowAnimation.animation(transform, new AnimationPlayDetail() { progress = progressP });
            });

            NextAnim();
        }

        private void NextAnim()
        {
            nowAnimation = animations.Dequeue();
            onNowAnimationStart.OnNext();
            animCount = animations.Count;
            animName = nowAnimation.Name;
        }

        private void AnimEnd()
        {
            onAnimationEnd.OnNext();
            endFlag = true;
            onNowAnimationStart.OnCompleted();
            onNowAnimationEnd.OnCompleted();
            onAnimationEnd.OnCompleted();
        }

        private void DestroyAnim()
        {
            if (option.rejectOthersForever) return;
            Destroy(this);
        }

        internal void Exit()
        {
            nowAnimation?.Exit(transform);
            foreach (var anim in asyncAnims)
            {
                anim.Value?.Exit();
            }
            Dispose();
            AnimEnd();
            DestroyAnim();
        }
        internal void Dispose()
        {
            nowAnimation?.Dispose();
            foreach (var anim in asyncAnims)
            {
                anim.Value?.Dispose();
            }
        }
        internal void Stop()
        {
            nowAnimation?.Stop();
            foreach (var anim in asyncAnims)
            {
                anim.Value?.Stop();
            }
        }
        internal void Continue()
        {
            nowAnimation?.Continue();
            foreach (var anim in asyncAnims)
            {
                anim.Value?.Continue();
            }
        }
        internal void Destroy()
        {
            endFlag = true;
            Dispose();
            foreach (var anim in asyncAnims)
            {
                anim.Value?.Destroy();
            }
            DestroyAnim();
        }
        private void OnDestroy()
        {
            if (endFlag) return;
            Stop();
            Dispose();
        }
    }
}
