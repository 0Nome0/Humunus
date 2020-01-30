using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    public static class SelectionLib
    {
        public const string menuPath = "GameObject/Selection/";
        private static GameObject[] selectedGameObjects = new GameObject[0];
        public static GameObject[] SelectedGameObjects
        {
            get => selectedGameObjects ?? new GameObject[0];
            set => selectedGameObjects = value;
        }
        public static bool HasSelectedGameObjects => 0 < SelectedGameObjects.Length;
        public static Component component = null;


        [MenuItem(menuPath + "SelectObjects &c", false, 1)]
        public static void SelectObject() { selectedGameObjects = Selection.gameObjects; }
        [MenuItem(menuPath + "AddSelectObjects %&c", false, 1)]
        public static void AddSelectObject()
        {
            selectedGameObjects = selectedGameObjects.ToList().AddNonOverlap(Selection.activeGameObject).ToArray();
        }

        [MenuItem(menuPath + "SelectComponent", false, 1)]
        public static void SelectComponent()
        {
            if(Selection.activeGameObject == null) return;
            GenericMenu menu = new GenericMenu();
            Component[] components = Selection.activeGameObject.GetComponents<Component>();
            foreach(var c in components)
            {
                menu.AddItem(c.GetType().ToString(), () => { component = c; });
            }
            menu.ShowAsContext();
        }

        public static string PathOfSelectObject => AssetDatabase.GetAssetPath(Selection.activeObject);



        #region SelectParent/Child

        [MenuItem("Tools/Selection/SelectParent %[")]
        public static void SelectParent()
        {
            GameObject obj = Selection.activeGameObject;
            if(obj == null) return;

            Transform parent = Selection.activeGameObject.transform.parent;
            if(parent) Selection.activeGameObject = parent.gameObject;
        }
        [MenuItem("Tools/Selection/SelectChild %]")]
        public static void SelectChild()
        {
            GameObject obj = Selection.activeGameObject;
            if(obj != null && 0 < obj.transform.childCount)
            {
                Transform child = Selection.activeGameObject.transform.GetChild(0);
                Selection.activeGameObject = child.gameObject;
            }
            else if(obj.transform.childCount == 0)
            {
                Transform tr = obj.transform;
                Transform pTr = tr.parent;
                List<Transform> cs = pTr.GetChildren();
                int index = cs.IndexOf(tr);
                if(index == cs.Count - 1) return;
                Selection.activeGameObject = cs[index + 1].gameObject;
            }
        }
        [MenuItem("Tools/Selection/SelectParents #%[")]
        public static void SelectParents()
        {
            GameObject[] obj = Selection.gameObjects;
            Selection.objects =
                obj.ToList()
                   .Select(o => o.transform.parent)
                   .Where(tr => tr != null)
                   .Select(tr => tr.gameObject)
                   .ToArray();
        }
        [MenuItem("Tools/Selection/SelectChildren #%]")]
        public static void SelectChildren()
        {
            GameObject[] obj = Selection.gameObjects;
            Selection.objects =
                obj.ToList()
                   .Where(o => 0 < o.transform.childCount)
                   .Select(o => o.transform.GetChild(0).gameObject)
                   .ToArray();
        }

        [MenuItem("Tools/Selection/SelectAllChildren &]")]
        public static void SelectAllChildren()
        {
            GameObject obj = Selection.activeGameObject;
            Selection.objects = obj.transform.GetChildrenG().ToArray();
        }

        #endregion

    }
}
