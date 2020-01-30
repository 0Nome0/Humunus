using System;

namespace NerScript.RiValuer
{
    public interface IRiValuerProvider
    {
        RiValuerProviderInfo providerInfo { get; set; }
        RiValuerValue Flow();
    }
}
