using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal abstract class ObjectFixedAnimBase : ObjectAnimRoot
    {
        protected float second;
        private float count;

        protected override float target
        {
            get => second;
            set => second = value;
        }
        protected override float current
        {
            get => count;
            set => count = value;
        }

        protected ObjectFixedAnimBase(float _second = 1, EasingTypes easing = EasingTypes.None)
            : base(easing)
        {
            second = _second * Time.timeScale;
        }

        protected override void OnAnimed()
        {
            if (second <= count) count = second;
        }

        protected override float GetNextS() => Time.deltaTime;
    }
}
