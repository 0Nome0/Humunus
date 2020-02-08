using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///uGUIのRaycastをfalseにするクラス
//クリック判定などの弊害をなくすため
public class UGUIRaycastController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var uGUI = GetComponentsInChildren<Graphic>();
        foreach (var g in uGUI)
            g.raycastTarget = false;
    }
}
