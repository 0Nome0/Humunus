using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript.Attribute;
using UniRx;
using NerScript.Anime;
using UnityEngine.EventSystems;

namespace NerScript.Games
{
    using LSSS = LoadSceneScreenSystem;

    public class ScreenManager : SingletonMonoBehaviour<ScreenManager>
    {
        public static GameObject screen = null;


        public static class FadeSystem { public static Games.FadeSystem Instance => Games.FadeSystem.Instance; }
        public static class LoadSceneScreenSystem { public static LSSS Instance => LSSS.Instance; }

        public static void Create(GameObject prefab)
        {
            screen = Instantiate(prefab);
            screen.DontDestroyOnLoad();
            screen.name = "Screen";
            instance = screen.GetComponent<ScreenManager>();
            instance.Initialize();
        }

        public void Initialize()
        {
            gameObject.GetComponentInChildren<Games.FadeSystem>().Initialize();
            gameObject.GetComponentInChildren<LSSS>().Initialize();
        }


    }
}
