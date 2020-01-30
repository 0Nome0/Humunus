using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using NerScript.Resource;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(VoiceNameAttribute))]
    public sealed class VoiceNameDrawer : AudioNameDrawer
    {
        public VoiceNameDrawer() : base(AudioGroup.Voice)
        {
           
        }
    }
}
