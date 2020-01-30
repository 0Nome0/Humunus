using System;
using UnityEngine;
using NerScript;


namespace NerScript.RiValuer
{
    [Serializable]
    public class RiValuerProviderInfo
    {
        public ValueDataType ValueType = ValueDataType.Multi;
        public NodeGroup ProviderNodeGroup = null;
        public bool UpdateFlow = false;
    }
}