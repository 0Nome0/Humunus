using System;
using UnityEngine;

namespace NerScript
{
    [Serializable]
    public class Gauge
    {
        [NonSerialized] public bool overflow = false;
        public int Current { get => current; set => current = overflow ? value : value.Clamp(0, max); }
        [SerializeField] protected int current = 0;
        public int Max { get => max; set => max = value.ClampMin(0); }
        [SerializeField] protected int max = 0;

        public bool Empty => Current <= 0;
        public bool Full => Max <= Current;
        public float Ratio => (float)Current / Max;

        public Gauge() { }
        public Gauge(int _max) { Max = _max; Current = Max; }
        public Gauge(int _current, int _max) { Max = _max; Current = _current; }

        public void Add(int qty) => Current += qty;

        public void ToEmpty() => current = 0;
        public void ToFull() => current = max;
    }
}