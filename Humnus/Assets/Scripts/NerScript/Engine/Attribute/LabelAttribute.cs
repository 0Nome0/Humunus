using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NerScript.Attribute
{
    public class LabelAttribute : PropertyAttribute
    {
        public readonly string label;

        public LabelAttribute(string _label)
        {
            label = _label;
        }
    }
}
