using System;
using UnityEngine;

namespace NerScript.Attribute
{
    public class InterfaceAttribute : PropertyAttribute
    {
        public Type type;
        public InterfaceAttribute(Type type)
        {
            this.type = type;
        }
    }
}