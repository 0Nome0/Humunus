using System;
using System.Collections.Generic;
using NerScript.Games;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


namespace NerScript
{
    public enum DebugType
    {
        Log,
        Info,
        Warning,
        Error,
        Fatal,
    }

    public class Debuger : SingletonMonoBehaviour<Debuger>
    {
        [SerializeField]
        private Text text = null;
        private Canvas canvas = null;

        public static void Create(GameObject prefab)
        {
            if(!GameManager.Debuging) return;
            return;
            GameObject obj = Instantiate(prefab);
            obj.DontDestroyOnLoad();
            instance = obj.GetComponent<Debuger>();
            instance.Initialize();
        }

        private void Initialize()
        {
            Application.logMessageReceived += LogCallBackHandler;
            canvas = GetComponent<Canvas>();
            this.ObserveEveryValueChanged(_ => Camera.main)
                .TakeUntilDestroy(gameObject)
                .Subscribe(cam =>
                {
                    canvas.worldCamera = cam;
                });
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogCallBackHandler;
        }

        private static void LogCallBackHandler(string condition, string stackTrace, LogType type)
        {
            System.Diagnostics.StackTrace systemStackTrace = new System.Diagnostics.StackTrace(true);
            string systemStackTraceStr = systemStackTrace.ToString();
            AddTextLine($"●{type}: " + condition);
            if (type == LogType.Error) AddTextLine(systemStackTraceStr);
        }
        public static void AddTextLine(string text)
        {
            Instance._AddText(text + "\n");
        }

        public static void AddText(string text)
        {
            Instance._AddText(text);
        }

        private void _AddText(string text)
        {
            if (10000 < this.text.text.Length) this.text.text = this.text.text.Substring(10000);
            this.text.text = text + this.text.text;
        }

        private void SetText(string text)
        {
            this.text.text = text;
        }

    }
}