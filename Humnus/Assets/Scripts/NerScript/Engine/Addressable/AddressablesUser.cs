using System;

namespace NerScript.Resource
{
    public class AddressablesUser<T>
    {
        public IObservable<T> Load(string address)
        {
            return AddressablesLoader.Load<T>(address);
        }
    }
}
