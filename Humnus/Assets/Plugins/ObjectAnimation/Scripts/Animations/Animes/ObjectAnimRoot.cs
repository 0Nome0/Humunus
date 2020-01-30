using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal abstract class ObjectAnimRoot : IObjectAnim
    {
        public string Name { get; set; }
        public Action<Transform, AnimationPlayDetail> animation { get; protected set; }
        protected Action<Transform> animationLast { get; set; }
        public IOptimizedObservable<Unit> OnAnimeEnd { get { return onAnimeEnd; } }
        protected readonly Subject<Unit> onAnimeEnd = new Subject<Unit>();

        private bool isStop = false;
        protected EasingTypes easing;
        protected IDisposable anim = null;
        protected IObserver<Unit> _anim = null;

        protected AnimationPlayDetail animationPlayDetail = null;

        protected IDisposable GetUpdate(Transform tr, Action update)
        {
            return
            Observable
            .EveryUpdate()
            .TakeWhile(_ => !IsAnimEnd(tr))
            .Where(_ => CanAnimation(tr))
            .Subscribe(_ => update(), () => animationLast?.Invoke(tr));
        }
        protected IDisposable GetNextFrame(Transform tr, Action next)
        {
            return
            Observable
            .NextFrame()
            .TakeWhile(_ => !IsAnimEnd(tr))
            .Where(_ => CanAnimation(tr))
            .Subscribe(_ => next(), () => animationLast?.Invoke(tr));
        }

        protected abstract float target { get; set; }
        protected abstract float current { get; set; }
        // current
        protected float TLeapC
        {
            get
            {
                Anim();
                OnAnimed();
                return TLeapPC;
            }
        }
        //preCurrent
        protected float TLeapPC { get { return Easing.GetEasing(current / target, easing); } }
        // distance
        protected float TLeapD { get { return -TLeapPC + TLeapC; } }
        //leap
        protected float TLeapL { get { return (1.0f - TLeapPC); } }

        protected float EasingLast => easing.Easing(1);

        private bool CanAnimation(Transform tr)
        {
            return !(
                isStop ||
                !tr.gameObject.activeInHierarchy
            );
        }

        protected virtual bool IsAnimEnd(Transform tr)
        {
            return (
                tr == null ||
                target <= current + GetNextS()
            );
        }

        protected ObjectAnimRoot(EasingTypes _easing = EasingTypes.None)
        {
            easing = _easing;
            SetAnimationLast();
        }

        protected virtual void SetAnimationLast()
        {
            animationLast = (tr) => onAnimeEnd.OnNext();
        }

        protected void Anim()
        {
            current = current + GetNextS();
            animationPlayDetail.progress.Value = current / target;
        }

        protected virtual void OnAnimed()
        {

        }

        protected abstract float GetNextS();

        protected abstract ObjectAnimRoot GetAnimClone();

        public IObjectAnim GetClone()
        {
            return GetAnimClone().Clone(this);
        }

        private ObjectAnimRoot Clone(ObjectAnimRoot original)
        {
            Name = original.Name;
            easing = original.easing;
            return this;
        }

        public void Exit(Transform transform)
        {
            ExitAnim(transform);
            Dispose();
        }
        protected abstract void ExitAnim(Transform transform);

        public virtual void Dispose()
        {
            anim?.Dispose();
        }

        public virtual void Stop()
        {
            isStop = true;
        }

        public virtual void Continue()
        {
            isStop = false;
        }
    }
}
