using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.IO;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    public static class EditorLib
    {

        public const string DATA_OBJECT_NAME = "UnityEditorData";


        public static Assembly UnityEditorAssembly => unityEditorAssembly ?? (unityEditorAssembly = typeof(UnityEditor.Editor).Assembly);
        private static Assembly unityEditorAssembly = null;



        #region Path,Folder

        /// <summary>
        /// ファイル選択
        /// </summary>
        /// <param name="path"></param>
        public static void SendPing(string path)
        {
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);

            //存在すればPing
            if (obj) EditorGUIUtility.PingObject(obj);
        }
        public static void SendPingToSelectObject()
        {
            SelectObject(Selection.activeObject);
        }
        public static void SelectObject(Object obj)
        {
            if (obj == null) return;
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }
        /// <summary>
        /// オブジェクトのセーブパスを取得
        /// </summary>
        /// <param name="selectedObject"></param>
        /// <returns></returns>
        public static string GetSavePath(Object selectedObject)
        {
            string objectName = selectedObject.name;
            string dirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedObject));
            string path = $"{dirPath}/{objectName}.asset";

            if (!File.Exists(path)) return path;
            for (int i = 1; ; i++)
            {
                path = $"{dirPath}/{objectName}({i}).asset";
                if (!File.Exists(path)) break;
            }
            return path;
        }

        public static void MoveAsset(Object obj, string newPath)
            => AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(obj), newPath);


        #endregion


        public static class Handle
        {
            public static void DrawArrow(Vector2 p1, Vector2 p2)
            {
                Handles.DrawLine(p1, p2);
                float angle = (p1 - p2).VectorToDeg();
                Vector2 arrow = (angle + 30).DegToVector() * 20;
                Handles.DrawLine(p2, p2 + arrow);
                arrow = (angle - 30).DegToVector() * 20;
                Handles.DrawLine(p2, p2 + arrow);
            }
        }
    }
}