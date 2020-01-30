using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using static UnityEditorInternal.ReorderableList;

namespace NerScript.Editor
{
    public class ReorderableListHelper<T>
    {
        public ReorderableList list = null;
        public List<T> TList = null;

        public ReorderableListHelper(IList<T> _list,
            bool draggable = true, bool displayHeader = true, bool displayAddButton = true, bool displayRemoveButton = true)
        {
            list = new ReorderableList((IList)_list, typeof(T), draggable, displayHeader, displayAddButton, displayRemoveButton);
            TList = (List<T>)_list;
        }

        public void AddOnAddCallback(AddCallbackDelegate act) => list.onAddCallback += act;
        public void AddOnRemoveCallback(RemoveCallbackDelegate act) => list.onRemoveCallback += act;
        public void AddOnChangeCallback(ChangedCallbackDelegate act) => list.onChangedCallback += act;
        public void AddDrawCallback(ElementCallbackDelegate act) => list.drawElementCallback += act;
        public void AddElementBGCallback(ElementCallbackDelegate act) => list.drawElementBackgroundCallback += act;
        public void AddDrawHeaderCallback(HeaderCallbackDelegate act) => list.drawHeaderCallback += act;
        public void AddHeightCallback(ElementHeightCallbackDelegate act) => list.elementHeightCallback += act;
        public void AddReorderCallback(ReorderCallbackDelegateWithDetails act) => list.onReorderCallbackWithDetails += act;

        public void Draw(bool scopeVertical = false)
        {
            if (scopeVertical) GUILayout.BeginVertical();
            list.DoLayoutList();
            if (scopeVertical) GUILayout.EndVertical();
        }
    }
}
