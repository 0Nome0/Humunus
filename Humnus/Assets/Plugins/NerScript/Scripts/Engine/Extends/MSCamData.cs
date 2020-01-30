using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// ビューカメラの位置、設定データ
/// </summary>
public class MSCamData : MonoBehaviour
{
    [SerializeField]
    public List<ViewData> viewData = new List<ViewData>()
    {
        new ViewData(null, "Zero", new Vector3()         , 0.1f, new Quaternion(0,0,0,1), false, false),
        new ViewData(null, "UI"  , new Vector3(214,120,0), 267f, new Quaternion(0,0,0,1), true , true ),
    };
}

[Serializable]
public class ViewData
{
    public GameObject target = null;
    public string name = "";
    public Vector3 pivot = new Vector3();
    public float size = 0;
    public Quaternion rotation = Quaternion.identity;
    public bool is2D = false;
    public bool orthographic = false;

    public ViewData(
        GameObject target, string name, Vector3 pivot, float size, Quaternion rotation, bool is2D, bool orthographic)
    {
        this.target = target;
        this.name = name;
        this.pivot = pivot;
        this.size = size;
        this.rotation = rotation;
        this.is2D = is2D;
        this.orthographic = orthographic;
    }

    public void Change(
         GameObject target, string name, Vector3? pivot, float? size, Quaternion? rotation, bool? is2D, bool? orthographic)
    {
        if (target != null) { this.target = target; }
        if (name != null) { this.name = name; }
        if (pivot != null) { this.pivot = (Vector3)pivot; }
        if (size != null) { this.size = (float)size; }
        if (rotation != null) { this.rotation = (Quaternion)rotation; }
        if (is2D != null) { this.is2D = (bool)is2D; }
        if (orthographic != null) { this.orthographic = (bool)orthographic; }
    }
}

