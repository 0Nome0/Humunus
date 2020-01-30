using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using NerScript.Resource;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(BGMNameAttribute))]
    public sealed class BGMNameDrawer : AudioNameDrawer
    {
        public BGMNameDrawer() : base(AudioGroup.BGM)
        {

        }
    }
}
