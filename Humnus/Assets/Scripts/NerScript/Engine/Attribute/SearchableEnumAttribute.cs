using System;
using UnityEngine;

namespace NerScript.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SearchableEnumAttribute : PropertyAttribute { }
}
