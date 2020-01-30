using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using NerScript.Attribute;
using NerScript.UI;
using UniRx.Async;

[RequireComponent(typeof(UIObjectLayout))]
public class UIObjectLayoutGizmo : MonoBehaviour
{
    [SerializeField] private bool useGizmo = false;
    [SerializeField] private Color gizmoColor = Color.white;
    private UIObjectLayout layout = null;
    private UIObjectLayout Layout => layout ?? (layout = GetComponent<UIObjectLayout>());

    private void OnDrawGizmosSelected()
    {
        if (!useGizmo) return;
        if (Layout.original == null) return;
        Layout.Validate();
        using (new GizmosLib.ColoringScop(gizmoColor))
        {
            Layout.layout.Layout(null, (v, i) =>
            {
                v *= Layout.WorldToLoacl;
                v += (Vector2)transform.position;
                GizmosLib.DrawRect(v, Layout.size, Quaternion.Euler(90, 0, 0));
            });
        }
    }


}
