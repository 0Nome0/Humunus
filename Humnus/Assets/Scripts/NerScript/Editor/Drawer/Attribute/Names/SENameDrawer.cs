using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using NerScript.Resource;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SENameAttribute))]
    public sealed class SENameDrawer : AudioNameDrawer
    {
        public SENameDrawer() : base(AudioGroup.SE)
        {

        }
    }
}
