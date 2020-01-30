using System;
using UnityEngine;

namespace NerScript.Character
{
    [Serializable]
    public class HitPoint : Gauge
    {
        public int HP { get => Current; set => Current = value; }
        public int MaxHP { get => Max; set => Max = value; }
        public bool Dead => Empty;

        public HitPoint(int _maxHp) : base(_maxHp) { }

        public bool Damage(int damage) { Add(-damage); return Dead; }
        public bool Heal(int heal) { Add(heal); return Dead; }
        public bool Kill() { ToEmpty(); return Dead; }
        public bool FullHeal() { ToFull(); return Dead; }
    }
}