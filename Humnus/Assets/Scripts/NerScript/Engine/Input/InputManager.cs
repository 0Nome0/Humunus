using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using NerScript.Attribute;
using UniRx;
using NerScript.Anime;
using UnityEngine.EventSystems;

namespace NerScript.Input
{
    using Input = UnityEngine.Input;

    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private static GameObject input = null;

        private static readonly Dictionary<KeyCode, bool> checkKey = new Dictionary<KeyCode, bool>();

        public static bool GetKeyDown(KeyCode keyCode)
        {
            if(!checkKey.ContainsKey(keyCode))
            {
                checkKey.Add(keyCode, false);
                return Input.GetKeyDown(keyCode);
            }
            bool result = Input.GetKeyDown(keyCode) && checkKey[keyCode];
            checkKey[keyCode] = false;
            return result;
        }


        public static void Create(GameObject prefab)
        {
            input = Instantiate(prefab);
            input.DontDestroyOnLoad();
            input.name = typeof(InputManager).Name;
            instance = input.GetComponent<InputManager>();
            instance.Initialize();
        }

        private void Initialize()
        {
            SetGameExit();
            //GlobalScreenInput.Instance.OnScreen.Subscribe(v => Debug.Log(v));
            Observable
            .EveryLateUpdate()
            .TakeUntilDestroy(instance.gameObject)
            .Subscribe(_ =>
            {
                foreach(var key in checkKey.Keys.ToList())
                {
                    checkKey[key] = true;
                }
            });
        }

        private void SetGameExit()
        {
            Observable
            .EveryUpdate()
            .TakeUntilDestroy(instance.gameObject)
            .Where(_ => Keyboard.current.escapeKey.isPressed)
            .Subscribe(_ => GameExit.Exit());
        }
    }
}
