using UnityEngine;
using NerScript.Accessory;

namespace NerScript.Anime
{
    internal interface IObjectAnimAccessoryAttacher
    {

        ObjectAccessoryType AccessoryType { get; }
        bool isDetach { get; }
    }
}