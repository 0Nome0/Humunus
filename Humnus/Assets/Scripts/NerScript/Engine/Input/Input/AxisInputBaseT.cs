using System;
using UniRx;

namespace NerScript.Input
{
    using UnityEngine;
    public abstract class AxisInputBase<T> : AxisInputBase
    {
        private IDisposable change = null;

        public abstract T Axis { get; }
        protected T difference = default;
        public T Difference => change == null ? MoveCheckStart<T>() : difference;
        protected abstract Func<Pair<T>, T> GetDifference { get; }

        private bool move = false;
        public abstract T Zero { get; }
        public bool Down => !Axis.Equals(Zero);
        public bool Up => Axis.Equals(Zero);
        public bool Move => change == null ? MoveCheckStart<bool>() : move;
        protected Func<Pair<T>, bool> GetEquals { get; } = pair => !pair.Current.Equals(pair.Previous);

        private U MoveCheckStart<U>()
        {
            change =
            this.ObserveEveryValueChanged(i => i.Axis)
            .Pairwise()
            .Subscribe(i =>
            {
                move = GetEquals(i);
                difference = GetDifference(i);
            });
            return default;
        }
    }
}