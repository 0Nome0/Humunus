using System;

namespace NerScript.RiValuer
{
    public interface IRiValuerResource
    {
        ValueDataType ValueType { get; }
        RiValuerValue Mine();
    }
}
