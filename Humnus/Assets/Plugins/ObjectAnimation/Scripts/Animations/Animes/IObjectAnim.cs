using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{

    internal interface IObjectAnim
    {
        Action<Transform, AnimationPlayDetail> animation { get; }
        IOptimizedObservable<Unit> OnAnimeEnd { get; }

        string Name { get; }
        IObjectAnim GetClone();
        void Exit(Transform tr);
        void Dispose();
        void Stop();
        void Continue();
    }
}
