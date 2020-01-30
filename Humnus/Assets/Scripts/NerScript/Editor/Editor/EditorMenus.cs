using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace NerScript.Editor
{
    public class EditorMenus 
    {
        public const string menuPath = "GameObject/Transform/";
        public static Vector3 vec = new Vector3();
        private static Transform transform => Selection.activeGameObject?.transform;
        private static void Copy(Vector3? _vec) { vec = _vec ?? Vector3.zero; Clip(); }
        private static void Clip() => GUIUtility.systemCopyBuffer = $"{vec.x},{vec.y},{vec.z}";

        [MenuItem(menuPath + "Copy Position", false, 1)]
        public static void ClipPosition() => Copy(transform?.position);
        [MenuItem(menuPath + "Copy Rotation", false, 2)]
        public static void ClipRotation() => Copy(transform?.eulerAngles);
        [MenuItem(menuPath + "Copy Scale", false, 3)]
        public static void ClipScale() => Copy(transform?.lossyScale);
        [MenuItem(menuPath + "Copy LocalPosition", false, 16)]
        public static void ClipLclPosition() => Copy(transform?.localPosition);
        [MenuItem(menuPath + "Copy LocalRotation", false, 17)]
        public static void ClipLclRotation() => Copy(transform?.localEulerAngles);
        [MenuItem(menuPath + "Copy LocalScale", false, 18)]
        public static void ClipLclScale() => Copy(transform?.localScale);

        [MenuItem(menuPath + "Paste Position", false, 31)]
        public static void PastePosition()
        {
            if (transform == null) return;
            Undo.RecordObject(transform, "Paste Position");
            transform.position = vec;
        }

        [MenuItem(menuPath + "Paste Rotation", false, 32)]
        public static void PasteRotation()
        {
            if (transform == null) return;
            Undo.RecordObject(transform, "Paste Rotation");
            transform.eulerAngles = vec;
        }

        [MenuItem(menuPath + "Paste Scale")]
        public static void PasteScale()
        {
            if (transform == null) return;
            Undo.RecordObject(transform, "Paste Scale");
            transform.localScale = vec;
        }

        [MenuItem(menuPath + "Paste LocalPosition", false, 46)]
        public static void PasteLclPosition()
        {
            if (transform == null) return;
            Undo.RecordObject(transform, "Paste LocalPosition");
            transform.localPosition = vec;
        }

        [MenuItem(menuPath + "Paste LocalRotation", false, 47)]
        public static void PasteLclRotation()
        {
            if (transform == null) return;
            Undo.RecordObject(transform, "Paste LocalRotation");
            transform.localEulerAngles = vec;
        }

        [MenuItem(menuPath + "Paste LocalScale", false, 48)]
        public static void PasteLclScale()
        {
            if (transform == null) return;
            Undo.RecordObject(transform, "Paste LocalScale");
            transform.localScale = vec;
        }

        [MenuItem("CONTEXT/Transform/ScaleXtoScale", false)]
        [MenuItem("GameObject/Transform/ScaleXtoScale &s", false)]
        [MenuItem("Tools/CONTEXT/Transform/ScaleXtoScale", false)]
        public static void ScaleXtoScale()
        {
            foreach (var obj in Selection.gameObjects)
            {
                Transform tr = obj.transform;
                float scale = tr.localScale.x;
                Vector3 newScale = new Vector3(scale, scale, scale);
                if (newScale == tr.localScale) continue;

                Undo.RecordObject(tr, "ScaleXtoScale");
                tr.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}