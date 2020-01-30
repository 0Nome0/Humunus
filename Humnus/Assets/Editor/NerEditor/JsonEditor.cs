using System.Linq;
using MiniJSON;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class JsonEditor : EditorWindow
{
    public class JsonNode
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public JsonNode Parent { get; private set; }
        List<JsonNode> children = new List<JsonNode>();
        public List<JsonNode> Children => children;

        public IEnumerable<int> Ids
        {
            get
            {
                yield return Id;
                foreach (var child in children)
                {
                    foreach (var childId in child.Ids)
                    {
                        yield return childId;
                    }
                }
            }
        }

        public void AddChild(JsonNode child)
        {
            if (child.Parent != null)
            {
                child.Parent.RemoveChild(child);
            }
            Children.Add(child);
            child.Parent = this;
        }

        public void RemoveChild(JsonNode child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.Parent = null;
            }
        }
    }

    public class JsonTreeView : TreeView
    {
        class JsonTreeViewItem : TreeViewItem
        {
            public JsonNode Data { get; set; }
        }

        JsonNode[] baseElements;

        public JsonTreeView(TreeViewState treeViewState, MultiColumnHeader multiColumnHeader) : base(treeViewState, multiColumnHeader)
        {
        }

        public void Setup(JsonNode[] baseElements)
        {
            this.baseElements = baseElements;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            var rows = GetRows() ?? new List<TreeViewItem>();
            rows.Clear();

            foreach (var baseElement in baseElements)
            {
                var baseItem = CreateTreeViewItem(baseElement);
                root.AddChild(baseItem);
                rows.Add(baseItem);
                if (baseElement.Children.Count >= 1)
                {
                    if (IsExpanded(baseItem.id))
                    {
                        AddChildrenRecursive(baseElement, baseItem, rows);
                    }
                    else
                    {
                        baseItem.children = CreateChildListForCollapsedParent();
                    }
                }
            }
            SetupDepthsFromParentsAndChildren(root);

            return rows;
        }

        void AddChildrenRecursive(JsonNode element, TreeViewItem item, IList<TreeViewItem> rows)
        {
            foreach (var childElement in element.Children)
            {
                var childItem = CreateTreeViewItem(childElement);
                item.AddChild(childItem);
                rows.Add(childItem);
                if (childElement.Children.Count >= 1)
                {
                    if (IsExpanded(childElement.Id))
                    {
                        AddChildrenRecursive(childElement, childItem, rows);
                    }
                    else
                    {
                        childItem.children = CreateChildListForCollapsedParent();
                    }
                }
            }
        }

        JsonTreeViewItem CreateTreeViewItem(JsonNode model)
        {
            return new JsonTreeViewItem { id = model.Id, displayName = model.Key, Data = model };
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (JsonTreeViewItem)args.item;
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                var cellRect = args.GetCellRect(i);
                var columnIndex = args.GetColumn(i);
                if (columnIndex == 0)
                {
                    base.RowGUI(args);
                }
                else if (columnIndex == 1)
                {
                    GUI.TextField(cellRect, item.Data.Value);
                }
            }
        }
    }

    JsonTreeView treeView;
    TextField inputText;
    int currentId;
    List<JsonNode> roots = new List<JsonNode>();

    [MenuItem("Window/JsonEditor")]
    static void Open()
    {
        GetWindow<JsonEditor>();
    }

    void OnEnable()
    {
        currentId = 0;

        BuildToolbarUI();
        BuildTreeView();
        Refresh();

        var scroller = new ScrollView();
        rootVisualElement.Add(scroller);

        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;
        scroller.Add(container);

        inputText = new TextField();
        inputText.style.width = 200;
        inputText.style.paddingBottom = 10;
        inputText.style.paddingTop = 10;
        inputText.style.paddingLeft = 10;
        inputText.style.paddingRight = 10;
        inputText.style.height = Screen.height - 100;
        inputText.multiline = true;
        foreach (var child in inputText.Children())
        {
            child.style.unityTextAlign = TextAnchor.UpperLeft;
        }
        container.Add(inputText);

        var convertButton = new Button(() => Convert());
        convertButton.text = ">";
        convertButton.style.width = 44;
        convertButton.style.height = 44;

        var space = new ToolbarSpacer();
        space.style.width = 100;
        space.flex = false;
        space.style.backgroundColor = rootVisualElement.style.backgroundColor;
        space.style.paddingBottom = 10;
        space.style.paddingTop = 10;
        space.style.paddingLeft = 10;
        space.style.paddingRight = 10;
        space.Add(convertButton);
        container.Add(space);
        var treeContainer = new IMGUIContainer(() =>
        {
            var rect = EditorGUILayout.GetControlRect(false, Screen.height);
            treeView.OnGUI(rect);
        });
        treeContainer.style.width = Screen.width;
        treeContainer.style.height = Screen.height;
        container.Add(treeContainer);
    }

    void BuildToolbarUI()
    {
        var toolbar = new Toolbar();
        var clearButton = new ToolbarButton(() =>
        {
            roots.Clear();
            Refresh();
        });

        var expandAllButton = new ToolbarButton(() => treeView.SetExpanded(roots[0].Ids.ToArray()));
        expandAllButton.text = "ExpandAll";
        toolbar.Add(expandAllButton);

        var collapseAllButton = new ToolbarButton(() => treeView.CollapseAll());
        collapseAllButton.text = "CollapseAll";
        toolbar.Add(collapseAllButton);

        clearButton.text = "Clear";
        toolbar.Add(clearButton);
        rootVisualElement.Add(toolbar);
    }

    void BuildTreeView()
    {
        var nameColumn = new MultiColumnHeaderState.Column()
        {
            headerContent = new GUIContent("Key"),
            headerTextAlignment = TextAlignment.Center,
            canSort = false,
            width = 200,
            minWidth = 50,
            autoResize = true,
            allowToggleVisibility = false
        };
        var descriptionColumn = new MultiColumnHeaderState.Column()
        {
            headerContent = new GUIContent("Value"),
            headerTextAlignment = TextAlignment.Center,
            canSort = false,
            width = Screen.width / 2 - 100,
            minWidth = 50,
            autoResize = true,
            allowToggleVisibility = false
        };

        var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] { nameColumn, descriptionColumn });
        var multiColumnHeader = new MultiColumnHeader(headerState);
        var treeViewState = new TreeViewState();
        this.treeView = new JsonTreeView(treeViewState, multiColumnHeader);
    }

    void Convert()
    {
        roots.Clear();
        currentId = 0;

        var root = AddTree(inputText.value, string.Empty);
        roots.Add(root);

        treeView.SetExpanded(new int[] { root.Id });
        Refresh();
    }

    void Refresh()
    {
        treeView.Setup(roots.ToArray());
    }

    JsonNode AddTree(string json, string rootLabel)
    {
        var root = new JsonNode { Id = currentId++, Key = rootLabel };
        var dict = Json.Deserialize(json) as Dictionary<string, object>;
        AddNode(root, dict);
        return root;
    }

    void AddNode(JsonNode element, object node)
    {
        if (node == null) { return; }
        if (node.GetType() == typeof(Dictionary<string, object>))
        {
            var dict = node as Dictionary<string, object>;
            foreach (var childPair in dict)
            {
                AddNode(element, childPair);
            }
        }
        else if (node.GetType() == typeof(KeyValuePair<string, object>))
        {
            var pair = (KeyValuePair<string, object>)node;
            var child = new JsonNode { Id = currentId++, Key = pair.Key };
            if (pair.Value != null)
            {
                var valueType = pair.Value.GetType();
                if (valueType != typeof(Dictionary<string, object>) &&
                    valueType != typeof(List<object>)
                    )
                {
                    child.Value = pair.Value.ToString();
                }
            }
            element.AddChild(child);

            AddNode(child, pair.Value);
        }
        else if (node.GetType() == typeof(List<object>))
        {
            var list = node as List<object>;
            var index = 0;
            foreach (var item in list)
            {
                if (item != null)
                {
                    var val = item.GetType() == typeof(Dictionary<string, object>) ? string.Empty : item.ToString();
                    var child = new JsonNode { Id = currentId++, Key = (index++).ToString(), Value = val };
                    element.AddChild(child);
                    AddNode(child, item);
                }
                else
                {
                    var child = new JsonNode { Id = currentId++, Key = (index++).ToString(), Value = "null" };
                    element.AddChild(child);
                }
            }
        }
    }
}