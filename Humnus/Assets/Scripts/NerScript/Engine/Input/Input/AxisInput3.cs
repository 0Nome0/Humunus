using System;
using UniRx;
using UnityEngine;

namespace NerScript.Input
{
    public class AxisInput3 : AxisInputBase<Vector3>, IAxisInput<Vector3>
    {
        public override Vector3 Axis => new Vector3(this.GetXAxis(), this.GetYAxis(), this.GetZAxis());
        public override Vector3 Zero => Vector3.zero;
        public override bool[] Direction => new bool[] { 0 <= Axis.x, 0 <= Axis.y, 0 <= Axis.z };
        protected override Func<Pair<Vector3>, Vector3> GetDifference => p => p.Current - p.Previous;
    }
}