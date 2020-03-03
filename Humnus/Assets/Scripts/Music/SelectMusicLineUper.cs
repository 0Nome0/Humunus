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
        layouter.SetSpace(new Vector2(0, iconSpace));

        Vector2Layout layout = layouter.layout;
        layout.direction = Directional.RightDown;
        layout.elementLayoutCount = 1;
        layouter.layout.horizontalLayout = true;
        layouter.Validate();
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
                MusicSelectIcon icon = obj.GetComponent<MusicSelectIcon>();
                icon.cover.enabled = !datas[i].openFlag[0];
                icon.text.text = datas[i].musicName;
            });

        transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (database.Count) * iconSpace + lengthOffset);
    }

    public MusicData GetMusicData(int index) => (MusicData)database.objects[index];

    private void OnDrawGizmosSelected() { OnValidate(); }
}
