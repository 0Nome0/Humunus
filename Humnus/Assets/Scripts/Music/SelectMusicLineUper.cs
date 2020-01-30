using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using NerScript;

[RequireComponent(typeof(UIObjectLayout))]
public class SelectMusicLineUper : MonoBehaviour
{
    [SerializeField] private ScriptableObjectDatabase database = null;
    [SerializeField, HideInInspector] private UIObjectLayout layouter = null;
    [SerializeField] private RectTransform iconPrefab = null;
    [SerializeField] private float lengthOffset = 25;
    [SerializeField] private float iconSpace = 55;

    private void OnValidate()
    {
        if (layouter == null)
        {
            layouter = GetComponent<UIObjectLayout>();
        }
        layouter.hideFlags |= HideFlags.NotEditable;
        layouter.startWithLayout = false;

        if (iconPrefab == null) return;
        layouter.original = iconPrefab;

        Vector2Layout layout = layouter.layout;
        layout.space = new Vector2(iconSpace, 0);
        layout.direction = Directional.RightDown;
        layout.elementLayoutCount = 1;
        layout.horizontalLayout = false;
    }

    public void Start()
    {
        LineUp();
    }

    private void LineUp()
    {
        List<MusicData> datas = database.GetListT<MusicData>();
        OnValidate();
        layouter.Layout(
            datas.Count,
            (obj, i) =>
            {
                obj.GetComponent<Image>().sprite = datas[i].icon;
            });

        transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((database.Count) * iconSpace + lengthOffset, 0);
    }

    public MusicData GetMusicData(int index) => (MusicData)database.objects[index];
}
