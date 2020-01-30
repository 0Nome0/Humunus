using System;
using System.Collections;
using System.Collections.Generic;
using NerScript;
using NerScript.Attribute;
using UnityEngine;


public class LongImage : MonoBehaviour
{

    public Transform tr1;
    public Transform tr2;





    private void Start()
    {

    }


    private void Update()
    {
        OnDrawGizmos2();
    }

    public void OnDrawGizmos2()
    {
        if(tr1 == null || tr2 == null)
        {
            return;
        }

        Vector3 dist = tr2.localPosition - tr1.localPosition;
        transform.position = (tr2.position + tr1.position) / 2.0f;
        RectTransform rte = (RectTransform)transform;
        rte.sizeDelta = rte.sizeDelta.SetedY(dist.magnitude);
        transform.SetLclRotZ(-dist.ToVec2().XY.VectorToEuler());


    }
}
