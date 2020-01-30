using System;
using UnityEngine;

namespace NerScript.Attribute
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumFlagsAttribute : PropertyAttribute
    {
    }
}