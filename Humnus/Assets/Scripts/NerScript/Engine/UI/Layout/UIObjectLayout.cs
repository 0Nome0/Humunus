using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using NerScript.Attribute;
using NerScript.UI;
using UniRx.Async;


public class UIObjectLayout : MonoBehaviour
{
    private new RectTransform transform => (RectTransform)base.transform;

    [SerializeField] private GridLayoutGroup layoutGroup = null;
    [HideInInspector, SerializeField] private bool hasGridLayoutGroup = false;

    [ReadOnlyIf("hasGridLayoutGroup")] public RectTransform original = null;
    [ReadOnlyIf("hasGridLayoutGroup")] public Vector2Layout layout = null;
    public bool startWithLayout = false;

    public Vector2 WorldToLoacl => (Vector2)transform.lossyScale * original.localScale;
    [ReadOnlyOnInspector] public Vector2 size = Vector2.zero;
    [InspectorButton("Validate", "OnValidate"), SerializeField] private bool validate = false;


    [ContextMenu("Editable")]
    public void Editable()
    {
        hideFlags = HideFlags.None;
    }

    private void Start()
    {
        if (!startWithLayout) return;
        _Layout();
    }

    public void Layout(int? count, Action<GameObject, int> onCreated)
    {
        Validate();
        layout.Layout(count ?? 0, (v, i) =>
        {
            GameObject obj = Instantiate(original.gameObject, transform);
            obj.transform.localPosition = v;
            onCreated(obj, i);
        });
    }

    private void _Layout()
    {
        Layout(null, (g, i) => { });
    }

    public void OnValidate()
    {
        hasGridLayoutGroup = layoutGroup != null;
        if (!hasGridLayoutGroup) return;

        layout.origin = transform.sizeDelta - layoutGroup.cellSize;
        layout.origin *= new Vector2(-0.5f, 0.5f);
        layout.origin += new Vector2(layoutGroup.padding.left, layoutGroup.padding.top);
        size = layoutGroup.cellSize * WorldToLoacl;


        layout.horizontalLayout = layoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal;
    }

    public void Validate()
    {
        if (original == null) return;
        size = original.sizeDelta * WorldToLoacl;
        layout.elementLayoutCount.ClampMin(1);
    }

    public void SetSpace(Vector2 space)
    {
        layout.space = space;
    }
}
