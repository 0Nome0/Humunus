using System;
using UniRx;
using UnityEngine;

namespace NerScript.Input
{
    public class AxisInput2 : AxisInputBase<Vector2>, IAxisInput<Vector2>
    {
        public override Vector2 Axis => new Vector2(this.GetXAxis(), this.GetYAxis());
        public override Vector2 Zero => Vector2.zero;
        public override bool[] Direction => new bool[] { 0 <= Axis.x, 0 <= Axis.y };
        protected override Func<Pair<Vector2>, Vector2> GetDifference => p => p.Current - p.Previous;
    }
}