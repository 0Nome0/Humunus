using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{

    internal static class GameObjectAnimationUtil
    {
        internal static T GetorAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component) return component;
            else return gameObject.AddComponent<T>();
        }
        internal static void OnNext(this Subject<Unit> subject)
        {
            subject.OnNext(Unit.Default);
        }

        internal static float VectorToRad(this Vector2 thisVec) => Mathf.Atan2(thisVec.y, thisVec.x);
        internal static Vector2 RadToVector(this float thisFloat) => new Vector2(Mathf.Cos(thisFloat), Mathf.Sin(thisFloat));
        internal static float VectorToDeg(this Vector2 thisVec) => Mathf.Atan2(thisVec.y, thisVec.x) * Mathf.Rad2Deg;
        internal static Vector2 DegToVector(this float thisFloat) => new Vector2(Mathf.Cos(thisFloat * Mathf.Deg2Rad), Mathf.Sin(thisFloat * Mathf.Deg2Rad));
    }
}
