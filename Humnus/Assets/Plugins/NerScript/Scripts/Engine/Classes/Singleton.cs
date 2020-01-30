using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NerScript
{
    public interface ISingleton { }
    public interface ISingleton<T> : ISingleton
    {
        //T Instance { get; }
    }
    public interface ISingletonObject<T> : ISingleton where T : ISingletonObject<T>
    {
        //T Get();
    }

    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T instance;
    }
    public abstract class SingletonClass<T> : Singleton<T> where T : SingletonClass<T>, new()
    {
        public static T Instance => Get();

        private static T Get()
        {
            //return _instance ?? (_instance = new Td()); //プレビュー版
            if (instance == null) return instance = new T();
            else return instance;
        }
    }
    public abstract class SingletonMonoBehaviour<T>
        : MonoBehaviour, ISingleton where T : SingletonMonoBehaviour<T>
    {
        public static T Instance => instance;
        protected static T instance;
    }
    public abstract class SingletonScriptableObject<T>
        : ScriptableObject, ISingleton where T : SingletonScriptableObject<T>
    {
        public static T Instance => (T)instance;
        private static SingletonScriptableObject<T> instance;

        protected SingletonScriptableObject()
        {
            instance = this;
        }
    }
}
