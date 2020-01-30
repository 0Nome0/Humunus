using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{
    [Serializable]
    public class ObjectAnimationPlanner : AnimationPlanner
    {
        internal ObjectAnimationPlanner(GameObject gameObject)
            : base(gameObject)
        {

        }
    }
}
