using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

namespace NerScript.Editor
{
    public class RestartUnityEditor
    {
        [MenuItem("File/Restart")]
        static void RestartUnity()
        {
            // 別のUnityを起動したあとに自身を終了
            Process.Start(EditorApplication.applicationPath);
            EditorApplication.Exit(0);
        }
    }
}