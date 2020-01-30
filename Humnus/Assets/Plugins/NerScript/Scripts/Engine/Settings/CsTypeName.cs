using System;
using System.Reflection;

namespace NerScript
{
    [Serializable]
    public struct CsTypeName
    {
        public string typeName;
        public string assemblyName;
        public bool includeUnity;
        public CsTypeName(string _assemblyName, string _typeName)
        {
            assemblyName = _assemblyName;
            typeName = _typeName;
            includeUnity = false;
        }
    }
}
