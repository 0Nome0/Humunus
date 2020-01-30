using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Omer = NerScript.Anime.ObjectAnimationMemoryUser;
using Omey = NerScript.Anime.ObjectAnimMemoryType;
using Ap = NerScript.Anime.AnimationPlanner;

namespace NerScript.Anime
{
    public enum ObjectAnimMemoryType
    {
        Position,
        _Position,
        LocalPosition,
        _LocalPosition,
        Rotation,
        _Rotation,
        LocalRotation,
        _LocalRotation,
        Scale,
        _Scale,
    }
    internal sealed class ObjectAnimationMemory
    {
        internal readonly Dictionary<Omey, dynamic> memory = new Dictionary<Omey, dynamic>();

        internal ObjectAnimationMemory() { }
        internal void Memorize(Omey type, dynamic value)
        {
            memory[type] = value;
        }
        internal dynamic GetMemory(Omey type) { return memory[type]; }
        internal dynamic this[Omey type] => GetMemory(type);
        internal bool HasKey(Omey type) => memory.ContainsKey(type);
    }

    public static partial class ObjectAnim
    {
        public static Ap MemoryPosition(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey.Position, memory?.memory);
        internal static Ap Memory_Position(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey._Position, memory?.memory);
        public static Ap MemoryLocalPosition(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey.LocalPosition, memory?.memory);
        internal static Ap Memory_LocalPosition(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey._LocalPosition, memory?.memory);
        public static Ap MemoryRotation(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey.Rotation, memory?.memory);
        internal static Ap Memory_Rotation(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey._Rotation, memory?.memory);
        public static Ap MemoryLocalRotation(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey.LocalRotation, memory?.memory);
        internal static Ap Memory_LocalRotation(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey._LocalRotation, memory?.memory);
        public static Ap MemoryScale(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey.Scale, memory?.memory);
        internal static Ap Memory_Scale(this Ap planner, Omer? memory = null) => planner.MemorizeAnim(Omey._Scale, memory?.memory);
    }
}
