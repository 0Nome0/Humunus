using UnityEngine;

namespace NerScript.Attribute
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string boolName;
        public bool show;

        public ShowIfAttribute(string _boolName)
        {
            boolName = _boolName;
            show = false;
        }
    }
}
