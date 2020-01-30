using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NerScript;
using NerScript.Editor;
using UnityEngine;
using UnityEditor;

namespace NerScript.RiValuer
{
    public class RiValuerEditor
    {

        public class GUI
        {
            public static void ValueField(NodeGroup node, out RiValuerValue value)
            {
                value = new RiValuerValue();
                RiValuerValue nvalue = node.Self.value;
                switch (node.Self.valueType)
                {
                    case ValueDataType.Multi: break;
                    case ValueDataType.Bool:
                        value.Bool = EditorGUILayout.Toggle("Bool", nvalue.Bool); break;
                    case ValueDataType.Int:
                        value.Int = EditorGUILayout.IntField("Int", nvalue.Int); break;
                    case ValueDataType.String:
                        value.String = EditorGUILayout.TextField("String", nvalue.String); break;
                    case ValueDataType.Float:
                        value.Float = EditorGUILayout.FloatField("Float", nvalue.Float); break;
                    case ValueDataType.Vector2:
                        value.Vector2 = EditorGUILayout.Vector2Field("Vector2", nvalue.Vector2); break;
                    case ValueDataType.Vector3:
                        value.Vector3 = EditorGUILayout.Vector3Field("Vector3", nvalue.Vector3); break;
                }
            }
        }





















    }
}
