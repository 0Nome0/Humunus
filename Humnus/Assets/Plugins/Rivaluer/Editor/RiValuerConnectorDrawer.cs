using UnityEngine;
using NerScript.Editor;

namespace NerScript.RiValuer.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(RiValuerConnector))]
    public class RiValuerConnectorDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            RiValuerConnector connector = (RiValuerConnector)target;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(connector), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            GUILib.Horizontal(() =>
            {
                if (GUILayout.Button("Setting"))
                {
                    RiValuerConnectionWindow.Open(new RiValuerWindowMadiator(connector));
                }
            }, 50);
        }
    }
    public class RiValuerWindowMadiator : IEditorWindowMediator
    {
        Object IEditorWindowMediator.recordable => connector;
        public RiValuerConnector connector = null;
        public RiValuerWindowMadiator(RiValuerConnector _connector)
        {
            connector = _connector;
        }

    }

}