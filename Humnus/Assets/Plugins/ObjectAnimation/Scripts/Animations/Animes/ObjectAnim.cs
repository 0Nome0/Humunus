using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using NerScript;
namespace NerScript.Anime
{
    public static partial class ObjectAnim
    {

        public static ObjectAnimationPlanner ObjectAnime(GameObject gameObject)
        {
            return new ObjectAnimationPlanner(gameObject);
        }

        public static ObjectAnimationPlanner ObjectAnimation(this GameObject gameObject)
        {
            return new ObjectAnimationPlanner(gameObject);
        }

        public static bool HasObjectAnim(this GameObject gameObject)
        {
            return gameObject.GetComponent<ObjectAnimation>();
        }

    }
}
