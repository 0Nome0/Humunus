using System;

namespace NerScript.RiValuer
{
    public interface IRiValuerDemander
    {
        ValueDataType ValueType { get; }
        void Draw(RiValuerValue value);
    }
}
