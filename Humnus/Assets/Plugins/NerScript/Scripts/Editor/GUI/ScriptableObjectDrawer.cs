using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using System.Text.RegularExpressions;

namespace NerScript.Editor
{
    public abstract class ScriptableObjectDrawer<T> : UnityEditor.Editor where T : ScriptableObject
    {
        protected virtual string assetName => "AssetData";
        protected T data = null;

        public ScriptableObjectDrawer() { }

        public sealed override void OnInspectorGUI()
        {
            data = (T)target;
            AssetField();
            OnGUI();
        }
        public abstract void OnGUI();
        private void AssetField()
        {
            GUILib.ScriptableObjectField(data);
            GUILib.AssetField(data, assetName);
        }
        protected void UndoRecord(string msg) { Undo.RecordObject(data, msg); EditorUtility.SetDirty(data); }
    }
}
