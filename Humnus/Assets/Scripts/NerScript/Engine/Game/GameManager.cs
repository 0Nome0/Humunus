using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Input;
using NerScript.Resource;
using UniRx;
using System.Threading;

namespace NerScript.Games
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public static readonly bool Debuging = true;

        public void FlagInit()
        {

        }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            GameManagerSetting setting = (GameManagerSetting)Resources.Load("GameManagerSetting");
            GameManagerInit(setting);
            FadeManagerInit(setting);
            AudioManagerInit(setting);
            InputSystemManagerInit(setting);
            DebugerInit(setting);

            SetTargetFPS();
            SetMouseDetail();
            SetScreenDetail();
        }

        private static void GameManagerInit(GameManagerSetting asset)
        {
            GameObject prefab = asset.GameManager;
            GameObject obj = Instantiate(prefab);
            obj.name = "GameManager";
            obj.DontDestroyOnLoad();
            instance = obj.GetComponentInChildren<GameManager>();
        }
        private static void SetTargetFPS()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
        }
        private static void SetMouseDetail()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        private static void SetScreenDetail()
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }
        private static void FadeManagerInit(GameManagerSetting asset)
        {
            ScreenManager.Create(asset.FadeManager);
        }
        private static void AudioManagerInit(GameManagerSetting asset)
        {
            AudioManager.Create(asset.AudioManager);
        }
        private static void InputSystemManagerInit(GameManagerSetting asset)
        {
            InputManager.Create(asset.InputSystemManager);
        }
        private static void DebugerInit(GameManagerSetting asset)
        {
            Debuger.Create(asset.Debuger);
        }
    }
}