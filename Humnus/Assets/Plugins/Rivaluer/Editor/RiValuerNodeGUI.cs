using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using NerScript.Editor;
using NodeType = NerScript.RiValuer.RiValuerNodeType;
using Object = UnityEngine.Object;
using Selection = NerScript.RiValuer.Editor.RiValuerSelection;


namespace NerScript.RiValuer.Editor
{
    public class RiValuerNodeGUI
    {
        public delegate void RVNodeGUI(int id, RiValuerConnectionWindow window, NodeGroup node);

        public static Dictionary<NodeType, RVNodeGUI> GUIs = new Dictionary<NodeType, RVNodeGUI>()
        {
            {NodeType.None, (i, w, n) => { }},
            {NodeType.Provider, (i, w, n) => Provider(i, w, n)},
            {NodeType.Demander, (i, w, n) => Demander(i, w, n)},
            {NodeType.Add, (i, w, n) => Value(i, w, n)},
            {NodeType.Multiple, (i, w, n) => Value(i, w, n)},
            {NodeType.Resource, (i, w, n) => Resource(i, w, n)},
            {NodeType.Convert, (i, w, n) => Convert(i, w, n)},
        };

        private static RiValuerNodeGUI instance = null;
        private static RiValuerNodeGUI Instance => instance ?? CreateInstance();

        private static RiValuerNodeGUI CreateInstance()
        {
            instance = new RiValuerNodeGUI();
            NameStyle = GUIStyle.none;
            instance.boxStyle = new GUIStyle(GUI.skin.box);
            return instance;
        }


        private static int fontSize = 11;
        private static GUIStyle NameStyle { get; set; }
        private GUIStyle boxStyle = null;
        private static GUIStyle GetBoxStyle(RiValuerConnectionWindow window)
        {
            Instance.boxStyle.fontSize = (int)(fontSize * window.Zoom);
            return Instance.boxStyle;
        }


        private static float height => 19 * Selection.window.Zoom;


        public static void onGUI(int id, RiValuerConnectionWindow window, NodeGroup node)
        {
            GUILib.BackgroundColoring(new Color(.7f, .7f, .7f, .9f), () =>
            {
                using(new GUILayout.VerticalScope("box"))
                {
                    Attaching(id, window, node);
                    GUIs[node.Self.nodeType](id, window, node);
                    FlowedValueGUI(window, node);
                }
            });
        }

        public static void Attaching(int id, RiValuerConnectionWindow window, NodeGroup node)
        {
            GUIStyle b = new GUIStyle(GUI.skin.button);
            b.fontSize = (int)(fontSize * window.Zoom);
            string valueType = node.Self.valueType.ToString();
            if(node.Self.nodeType == NodeType.Convert && node.HasPrevious)
                valueType = node.PreviousGroup.Self.valueType + " ▷ " + valueType;
            GUILayout.Box(valueType, GetBoxStyle(window), GUILayout.ExpandWidth(true), GUILayout.Height(height));
            EventLib e = window.Event;
            if(e.MouseLeftDown && GUILayoutUtility.GetLastRect().Contains(e.MousePos))
            {
                window.SetAttachingNode(node);
            }
        }

        public static void Provider(int id, RiValuerConnectionWindow window, NodeGroup node)
        {
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                GUILib.SetLabelWidth(50);
                Object prv = EditorGUILayout.ObjectField("Provider", node.Self.monobehaviour,
                    typeof(IRiValuerProvider), true);
                GUILib.ReSetLabelWidth();
                if(change.changed)
                {
                    Undo.RecordObject(Selection.connector, "Set Provider");
                    node.Self.SetProvider((MonoBehaviour)prv);
                    if(node.HasNext &&
                       node.Self.Provider.providerInfo.ValueType != node.NextGroup.Self.valueType)
                    {
                        node.NextGroup.PreviousGroup = null;
                        node.NextGroup = null;
                    }
                }
            }
        }

        public static void Demander(int id, RiValuerConnectionWindow window, NodeGroup node)
        {
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                GUILib.SetLabelWidth(60);
                Object prv = EditorGUILayout.ObjectField("Demander", node.Self.monobehaviour,
                    typeof(IRiValuerDemander), true);
                GUILib.ReSetLabelWidth();
                if(change.changed)
                {
                    Undo.RecordObject(Selection.connector, "Set Demander");
                    node.Self.SetDemander((MonoBehaviour)prv);
                    if(node.HasPrevious &&
                       node.Self.Demander.ValueType != node.PreviousGroup.Self.valueType && node.HasPrevious)
                    {
                        node.PreviousGroup.NextGroup = null;
                        node.PreviousGroup = null;
                    }
                }
            }
        }

        public static void Resource(int id, RiValuerConnectionWindow window, NodeGroup node)
        {
            if(node.HasNext &&
               node.NextGroup.Self.nodeType == NodeType.Convert &&
               (node.NextGroup.Self.valueType == ValueDataType.Vector2 ||
                node.NextGroup.Self.valueType == ValueDataType.Vector3))
            {
                using(var change = new EditorGUI.ChangeCheckScope())
                {
                    RiValuerValue value = new RiValuerValue();
                    RiValuerValue nValue = node.Self.value;
                    GUILib.SetLabelWidth(60);
                    value.Int = EditorGUILayout.EnumPopup("AxisSort", (VectorLib.VectorAxisMatch)nValue.Int).Int();
                    GUILib.ReSetLabelWidth();
                    if(change.changed)
                    {
                        Undo.RecordObject(Selection.connector, "Change Value");
                        node.Self.value.SetValue(value);
                        if(node.Self.nodeType == NodeType.Resource
                           && node.HasNext)
                            node.NextGroup.Self.value.SetValue(value);
                    }
                }
            }
            else Value(id, window, node);
        }
        public static void Convert(int id, RiValuerConnectionWindow window, NodeGroup node)
        {

        }

        public static void Value(int id, RiValuerConnectionWindow window, NodeGroup node)
        {
            GUILib.SetLabelWidth(60);
            if(node.HasResource) EditorGUI.BeginDisabledGroup(true);
            ValueGUI(id, window, node);
            if(node.HasResource) EditorGUI.EndDisabledGroup();
            GUILib.ReSetLabelWidth();
        }

        private static void ValueGUI(int id, RiValuerConnectionWindow window, NodeGroup node)
        {
            if(window.attachedFlame) return;
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                RiValuerEditor.GUI.ValueField(node, out RiValuerValue value);
                if(change.changed)
                {
                    Undo.RecordObject(Selection.connector, "Change Value");
                    node.Self.value.SetValue(value);
                    if(node.Self.nodeType == NodeType.Resource &&
                       node.HasNext)
                        node.NextGroup.Self.value.SetValue(value);
                }
            }
        }
        private static void FlowedValueGUI(RiValuerConnectionWindow window, NodeGroup node)
        {
            if(!window.showFlowedValue) return;
            RiValuerValue value = node.Self.lastFlowedNode;
            if(value == null) return;
            string label = "";
            switch(node.Self.valueType)
            {
                case ValueDataType.Bool:
                    label = "Bool: " + value.Bool;
                    break;
                case ValueDataType.Int:
                    label = "Int: " + value.Int;
                    break;
                case ValueDataType.String:
                    label = "String: " + value.String;
                    break;
                case ValueDataType.Float:
                    label = "Float: " + value.Float;
                    break;
                case ValueDataType.Vector2:
                    label = "Vector2: " + value.Vector2;
                    break;
                case ValueDataType.Vector3:
                    label = "Vector3: " + value.Vector3;
                    break;
            }
            using(new EditorGUI.DisabledScope(true))
            {
                GUILayout.Label(label, GUIStyle.none);
            }
        }
    }
}
