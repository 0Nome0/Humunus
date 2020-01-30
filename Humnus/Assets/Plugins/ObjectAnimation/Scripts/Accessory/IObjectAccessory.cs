using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NerScript.Accessory
{
    internal enum ObjectAccessoryType
    {
        Mover,//移動
        Rotater,//回転
        LookAter,//指定座標を向く
    }

    internal interface IObjectAccessory
    {
        ObjectAccessoryType AccessoryType { get; }
    }
}
