using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace NerScript.Resource
{
    /// <summary>
    /// 音楽の種類
    /// </summary>
    public enum AudioGroup { None = -1, BGM, SE, Voice }

    // 設定を管理するクラス
    [Serializable]
    public class AudioDatabase : ScriptableObject
    {

    }
}