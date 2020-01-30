using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NerScript.Anime
{

    internal enum ObjectAnimModuleType
    {
        Sync, //同期
        Async, //非同期
        Action, //処理
        Memorys, //変数記憶
        Accessory,//アクセサリー関連
    }

    internal interface IObjectAnimModule
    {
        ObjectAnimModuleType ModuleType { get; }
    }
}
