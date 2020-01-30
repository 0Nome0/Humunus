using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using NerScript.Editor;
using Selection = NerScript.RiValuer.Editor.RiValuerSelection;
using NodeType = NerScript.RiValuer.RiValuerNodeType;
using NodeTypeInfo = NerScript.RiValuer.RiValuerNodeTypeInfo;

namespace NerScript.RiValuer.Editor
{

    public class RiValuerConnectionWindow : EditorWindowBase<RiValuerConnectionWindow>
    {
        public enum WindowState
        {
            None,
            MoveNode,
            Connect,
            NodeMenu,
            AddMenu,
        }
        private WindowState state = WindowState.None;
        public bool attachedFlame = false;
        private List<NodeGroup> nodes = null;
        public EventLib Event = null;

        protected override void Init(IEditorWindowMediator windowMediator)
        {
            Selection.window = this;
            RiValuerConnector connector = ((RiValuerWindowMadiator)windowMediator).connector;
            Selection.connector = connector;
            nodes = connector.nodes;
            connector.NodeReference();
            connector.windowStatus.showFlag = true;
        }


        private Memorable<Vector2> mousePos = new Memorable<Vector2>();
        private Vector2 deltaMousePos => mousePos.Value - mousePos.Previous;
        public float Zoom
        {
            get => Selection.connector?.windowStatus?.zoom ?? 1;
            private set => Selection.connector.windowStatus.zoom = value;
        }
        public Vector2 Pivot
        {
            get => Selection.connector?.windowStatus?.pivot ?? Vector2.zero;
            private set => Selection.connector.windowStatus.pivot = value;
        }

        public override string WindowTitle => "RiValuerConnection";

        [NonSerialized] private float pivotMovePower = 3.0f;
        private bool isDrawGrid = true;
        private bool isDrawInfos = true;
        public bool showFlowedValue = true;

        protected override void OnGUI()
        {
            DrawBackGround(new Color32(60, 40, 40, 180));

            if (Selection.connector == null)
            {
                RiValuerConnector connector = UnityEditor.Selection.activeGameObject?.GetComponent<RiValuerConnector>();
                if (connector != null)
                {
                    Open(new RiValuerWindowMadiator(connector));
                }
                else if ((connector = FindObjectsOfType<RiValuerConnector>().Where(c => c.windowStatus.showFlag).DefaultIfEmpty(null).ToArray()[0]) != null)
                {
                    Open(new RiValuerWindowMadiator(connector));
                }
                else
                {
                    DrawGrid();
                    DrawInfos();
                    return;
                }
            }

            Event = new EventLib();
            attachedFlame = false;

            if (isDrawGrid) DrawGrid();
            MouseInput();
            AddMenu();
            ScreenZoom();
            DrawNodes();



            DrawInfos();
        }
        private void DrawInfos()
        {
            string nameLabel = (isDrawInfos ? "▼" : "▶") + " RiValuerConnection " + RiValuer.version;
            isDrawInfos = GUILayout.Toggle(isDrawInfos, nameLabel, GUI.skin.box, GUILayout.Width(150));
            if (!isDrawInfos) return;

            GUILayout.Box(state.ToString(), GUILayout.Width(150));
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box, GUILayout.Width(150)))
            {
                isDrawGrid = GUILayout.Toggle(isDrawGrid, new GUIContent("", "Grid Draw"));
                showFlowedValue = GUILayout.Toggle(showFlowedValue, new GUIContent("", "Show Flowed Value"));
                GUILayout.FlexibleSpace();
            }
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box, GUILayout.Width(150)))
            {
                if (GUILayout.Button("Select", GUILayout.Width(50)) && Selection.connector != null)
                {
                    EditorLib.SelectObject(Selection.connector.gameObject);
                }
                if (GUILayout.Button("Find", GUILayout.Width(35)))
                {
                    GenericMenu menu = new GenericMenu();
                    foreach (var connector in FindObjectsOfType<RiValuerConnector>())
                    {
                        menu.AddItem(connector.gameObject.name, () =>
                        {
                            Open(new RiValuerWindowMadiator(connector));
                            EditorLib.SelectObject(Selection.connector.gameObject);
                        });
                    }
                    menu.ShowAsContext();
                }
            }
        }
        /// <summary>
        /// グリッド
        /// </summary>
        private void DrawGrid()
        {
            Handles.color = Colors.Black;//.SetAlpha(0.5f);

            int gridSize = (int)(20 * Zoom) + 1;
            int count = 0;

            for (float x = Pivot.x % gridSize, y = Pivot.y % gridSize;
                count < 500 && (x < EditorWindowData.Height || y < EditorWindowData.Width);
                x += gridSize, y += gridSize, count++)
            {
                Handles.DrawLine(new Vector2((int)x, 0), new Vector2((int)x, EditorWindowData.Height));
                Handles.DrawLine(new Vector2(0, (int)y), new Vector2(EditorWindowData.Width, (int)y));
            }
            Handles.color = Colors.White;
        }
        /// <summary>
        /// マウス入力
        /// </summary>
        private void MouseInput()
        {
            if (Event.MouseUp)
            {
                if (Selection.attachingNode != null) { ConnectAttachCheck(); }
                Selection.moveingNode = null;
                Selection.attachingNode = null;
                state = WindowState.None;
                return;
            }

            if (Event.MouseLeftDown) { SetClickedNode(); }
            else if (Event.MouseRightDown) { NodeMenu(); }


            mousePos.Value = Event.MousePos;
            if (Selection.moveingNode != null)
            {
                Selection.moveingNode.Self.rect.AddPos(deltaMousePos / Zoom);
                SetWindowDirty();
            }
            else if (Event.MouseBtn == 2)
            {
                Pivot += deltaMousePos / Zoom * pivotMovePower;
                SetWindowDirty();
            }
        }
        /// <summary>
        /// アタッチチェック（外枠）
        /// </summary>
        public void ConnectAttachCheck()
        {
            foreach (var node in nodes)
            {
                CheckAttach(node);
            }
        }
        /// <summary>
        /// アタッチチェック（内部）
        /// </summary>
        /// <param name="node"></param>
        private void CheckAttach(NodeGroup node)
        {
            if (!node.Self.rect.Contains(ScreenToWorld(Event.MousePos))) return;

            NodeGroup attach = Selection.attachingNode;
            if (!CanAttach(node, attach)) return;

            attachedFlame = true;
            RecordObject("Connect Node");

            if (attach.Self.nodeType == NodeType.Resource)
            {
                node.AdvSetResource(attach);
            }
            else if (attach.Self.nodeType == NodeType.Convert)
            {
                attach.AdvSetConvert(node);
            }
            else
            {
                attach.AdvSetNext(node);
            }
            return;
        }
        private bool CanAttach(NodeGroup node, NodeGroup attach)
        {
            return !node.Contains(attach) &&
                node.Self.nodeType != NodeType.Provider &&
                attach.Self.nodeType != NodeType.Demander &&
                ValueTypeConform(node, attach);
        }
        /// <summary>
        /// 値タイプの適合チェック
        /// </summary>
        private bool ValueTypeConform(NodeGroup node, NodeGroup attach)
        {
            if (node.IsMultiValueType) return !attach.IsMultiValueType;
            else
            {
                bool typeMatch = node.ValueTypeMatch(attach);
                if (attach.ResourceMatch(node)) return attach.IsMultiValueType || typeMatch;
                else if (attach.ConvertMatch(node)) return node.Self.valueType != ValueDataType.Multi;
                else if (!attach.IsMultiValueType) return typeMatch;
            }
            return false;
        }
        private void SetClickedNode()
        {
            Vector2 w_pos = ScreenToWorld(mousePos.Value);
            float nodeTitleHeight = 20 / Zoom;
            foreach (var node in nodes)
            {
                Rect titleRect = node.Self.rect.SetedHeight(nodeTitleHeight);
                if (titleRect.Contains(w_pos))
                {
                    Selection.moveingNode = node;
                    state = WindowState.MoveNode;
                    return;
                }
            }
            EditorGUI.FocusTextInControl("");
        }
        public void SetAttachingNode(NodeGroup node)
        {
            Selection.attachingNode = node;
            state = WindowState.Connect;
        }
        /// <summary>
        /// ノードのメニュー
        /// </summary>
        private void NodeMenu()
        {
            foreach (var node in nodes)
            {
                if (!node.Self.rect.Contains(ScreenToWorld(Event.MousePos))) continue;

                state = WindowState.NodeMenu;
                GenericMenu menu = new GenericMenu();

                menu.AddItem("Delete", () =>
                {
                    RecordObject("Delete Node");
                    node.Self.ID = -1;
                    nodes.Remove(node);
                    if (node.Self.nodeType == NodeType.Provider) Selection.connector.providers.Remove(node);
                    Selection.connector.NodeIDSync();
                });
                menu.AddItem("Reset", () =>
                {
                    RecordObject("Reset Node");
                    node.Reset(nodes);
                    Selection.connector.NodeIDSync();
                });
                menu.ShowAsContext();
                return;
            }
        }
        /// <summary>
        /// ノードの描画
        /// </summary>
        private void DrawNodes()
        {
            BeginWindows();


            foreach (var node in nodes)
            {
                Rect rect = node.Self.rect.AddedPos(-EditorWindowData.Center).AddedPos(Pivot).Scaled(Zoom).AddedPos(EditorWindowData.Center);
                if (node.HasNext)
                {
                    Handles.color = Colors.Red;
                    Handles.DrawLine(WorldToScreen(node.Self.rect.center), WorldToScreen(node.NextGroup.Self.rect.center));
                    Handles.color = Colors.White;
                    //変更予定
                    EditorLib.Handle.DrawArrow(WorldToScreen(node.Self.rect.center),
                        WorldToScreen(node.Self.rect.center +
                        (node.NextGroup.Self.rect.center - node.Self.rect.center) * 0.55f));
                }
                GUILib.Coloring(GUI.color, () =>
                {
                    GUI.Window(node.Self.ID,
                    rect,
                    (int id) => { RiValuerNodeGUI.onGUI(id, this, node); },
                    node.Self.name);
                });
            }

            if (Selection.attachingNode != null)
            {
                Rect rect = new Rect(0, 0, 100, 20);
                rect.position = Event.MousePos - rect.size / 2;

                GUILib.Coloring(GUI.color, () =>
                {
                    GUI.Box(rect, Selection.attachingNode.Self.nodeType.ToString());
                });
            }

            EndWindows();
        }
        private void ScreenZoom()
        {
            if (UnityEngine.Event.current.type == EventType.ScrollWheel)
            {
                Zoom += Zoom * 0.02f * -UnityEngine.Event.current.delta.y;
                if (Zoom <= 0.2f) Zoom = 0.2f; //縮小
                if (3 <= Zoom) Zoom = 3; //拡大
                dirty = true;
            }
        }
        private bool CanDrawAddMenu()
        {
            return
            Event.MouseRightDown &&
            state == WindowState.None
            ;
        }
        private void AddMenu()
        {
            if (!CanDrawAddMenu()) return;

            GenericMenu menu = new GenericMenu();
            Vector2 pos = ScreenToWorld(Event.MousePos);


            menu.AddItem("AddNode/Provider", () =>
            {
                RecordObject("Add New Node");
                RiValuerNode node = new RiValuerNode(nodes.Count, pos) { nodeType = NodeType.Provider };
                node.SetProvider((MonoBehaviour)Selection.connector.GetComponent<IRiValuerProvider>());
                AddNodes(node);
            });
            menu.AddItem("AddNode/Demander", () =>
            {
                RecordObject("Add New Node");
                RiValuerNode node = new RiValuerNode(nodes.Count, pos) { nodeType = NodeType.Demander };
                node.SetDemander((MonoBehaviour)Selection.connector.GetComponent<IRiValuerDemander>());
                AddNodes(node);
            });
            menu.AddItem("AddNode/Add", () =>
            {
                RecordObject("Add New Node");
                RiValuerNode node = new RiValuerNode(nodes.Count, pos)
                {
                    nodeType = NodeType.Add,
                    typeInfo = NodeTypeInfo.Resourceable,
                    valueType = ValueDataType.Multi,
                    name = "Add"
                };
                AddNodes(node);
            });
            menu.AddItem("AddNode/Multiple", () =>
            {
                RecordObject("Add New Node");
                RiValuerNode node = new RiValuerNode(nodes.Count, pos)
                {
                    nodeType = NodeType.Multiple,
                    typeInfo = NodeTypeInfo.Resourceable,
                    valueType = ValueDataType.Multi,
                    name = "Multiple"
                };
                AddNodes(node);
            });
            foreach (var type in (ValueDataType[])Enum.GetValues(typeof(ValueDataType)))
            {
                menu.AddItem("AddNode/Resource/" + type, () =>
                {
                    RecordObject("Add New Node");
                    RiValuerNode node = new RiValuerNode(nodes.Count, pos)
                    {
                        nodeType = NodeType.Resource,
                        typeInfo = NodeTypeInfo.Resourceable,
                        valueType = type,
                        name = "Resource"
                    };
                    AddNodes(node);
                });
            }
            menu.AddItem("AddNode/Convert", () =>
            {
                RecordObject("Add New Node");
                RiValuerNode node = new RiValuerNode(nodes.Count, pos)
                {
                    nodeType = NodeType.Convert,
                    typeInfo = NodeTypeInfo.Resourceable,
                    valueType = ValueDataType.Multi,
                    name = "Convert"
                };
                AddNodes(node);
            });
            menu.AddItem("ResetZoom", () =>
            {
                Zoom = 1; SetWindowDirty();
            });
            menu.AddItem("Move0", () =>
            {
                Pivot = Vector2.zero; SetWindowDirty();
            });
            menu.ShowAsContext();
        }
        private void AddNodes(RiValuerNode node)
        {
            NodeGroup group = new NodeGroup(node);
            nodes.Add(group);
            if (node.nodeType == NodeType.Provider) Selection.connector.providers.Add(group);
            Selection.connector.NodeIDSync();
        }
        private Vector2 ScreenToWorld(Vector2 point) { return (point - EditorWindowData.Center) / Zoom - Pivot + EditorWindowData.Center; }
        private Vector2 WorldToScreen(Vector2 point) { return (point - EditorWindowData.Center + Pivot) * Zoom + EditorWindowData.Center; }
        private NodeGroup GetNode(RiValuerNode node) { return nodes[node.ID]; }
        private void Update()
        {
            painted++;
            if (2 < painted)
            {
                painted = 0;
            }
            Repaint();
        }
        private int painted = 0;

        private void OnDestroy()
        {
            if (Selection.connector == null) return;
            Selection.connector.windowStatus.showFlag = false;
        }

    }
}