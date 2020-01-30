using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class UIElementSample : EditorWindow
{
    private static readonly string UXMLPATH = "Assets/Editor/UIElement/Sample.uxml";


    [ContextMenu("Tools/UIElement/Sample")]
    public static void ShowWindow()
    {
        UIElementSample w = GetWindow<UIElementSample>();
    }


    private void OnEnable()
    {
        //Object a = AssetDatabase.LoadAssetAtPath(UXMLPATH);
        //VisualElement row = a.CloneTree();
        //var label = row.Q<Label>("random-explosion");
        //label.RegisterCallback<MouseUpEvent>(evt => evt.StopPropagation());
        //rootVisualElement.Add(row);
    }





}