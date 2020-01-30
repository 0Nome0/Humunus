using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    internal abstract class ObjectAnimBase : ObjectAnimRoot
    {
        protected int frame;
        private int count;
        protected override float target
        {
            get => frame;
            set => frame = (int)value;
        }
        protected override float current
        {
            get => count;
            set => count = (int)value;
        }

        protected ObjectAnimBase(int _frame = 1, EasingTypes easing = EasingTypes.None)
            : base(easing)
        {
            frame = _frame;
        }

        protected override float GetNextS() => 1;
    }
}
