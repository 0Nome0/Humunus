using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using NerScript;
using NerScript.RiValuer;


public class SimpleRivaluer : MonoBehaviour
{
    public Component provider = null;
    public Component demander = null;

    public IRiValuerProvider Provider => provider as IRiValuerProvider;
    public IRiValuerDemander Demander => demander as IRiValuerDemander;

    public bool valueTypeMatch => Provider.providerInfo.ValueType == Demander.ValueType;



    private void Start()
    {

    }

    private void Update()
    {
        if(!valueTypeMatch) return;

        if(Provider.providerInfo.UpdateFlow)
        {
            //Provider.providerInfo.
        }

        Demander.Draw(Provider.Flow());
    }
}
