using UnityEngine;

namespace NerScript.Attribute
{
    public class ReadOnlyIfAttribute : PropertyAttribute
    {
        public string boolName;
        public bool readOnly;

        public ReadOnlyIfAttribute(string _boolName)
        {
            boolName = _boolName;
            readOnly = false;
        }
    }
}
