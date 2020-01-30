using System;
using UnityEngine;

namespace NerScript
{
    [Serializable]
    public class StockableGauge : Gauge
    {
        public StockableGauge() : base() { Init(); }
        public StockableGauge(int max) : base(max) { Init(); }
        public StockableGauge(int current, int max) : base(current, max) { Init(); }

        public void Init()
        {
            overflow = true;
        }

        public void RemoveStock()
        {
            if(max <= current) Current -= Max;
        }
        public int CurrentStock() => current / max;
    }
}
