using System;
using UnityEngine;
using NerScript;


namespace NerScript.RiValuer
{
    [Serializable]
    public class RiValuerValue : ValueData
    {

        public RiValuerValue() : base()
        {
        }
        public RiValuerValue(IRiValuerProvider prv) : base()
        {
            type = prv.providerInfo.ValueType;
        }
        public RiValuerValue(RiValuerValue value) : base(value)
        {
            type = value.type;
        }
    }
}