using System;
using UnityEngine;
using UnityEngine.UI;
using NerScript.RiValuer;
using NerScript;
using NerScript.Anime;

public class DebugLoger : MonoBehaviour, IRiValuerDemander
{
    public NerScript.ValueDataType ValueType => NerScript.ValueDataType.Multi;

    public void Draw(RiValuerValue value)
    {
        Debug.Log(value.GetValue());
    }

    public void Log(string log) { Debug.Log(log); }
}
