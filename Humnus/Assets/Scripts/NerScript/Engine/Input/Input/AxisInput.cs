using System;
using UniRx;
using UnityEngine;

namespace NerScript.Input
{
    public class AxisInput : AxisInputBase<Vector>, IAxisInput<Vector>
    {
        public override Vector Axis => new Vector(this.GetXAxis());
        public override Vector Zero => Vector.zero;
        public override bool[] Direction => new bool[] { 0 <= Axis.x };
        protected override Func<Pair<Vector>, Vector> GetDifference => p => p.Current - p.Previous;
    }
}