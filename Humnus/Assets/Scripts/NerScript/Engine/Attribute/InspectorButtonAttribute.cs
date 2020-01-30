using UnityEngine;

namespace NerScript.Attribute
{
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public string name;
        public string method;
        /// <summary>
        /// boolでのみ動作(実行結果が帰ってくるので)
        /// </summary>
        public InspectorButtonAttribute(string _name, string _method)
        {
            name = _name;
            method = _method;
        }
    }
}
