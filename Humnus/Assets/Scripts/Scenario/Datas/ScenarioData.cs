using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;


public class ScenarioData : ScriptableObject, IManagedByID
{
    public int ID { get => episode; set => episode = value; }
    public Sprite icon = null;
    public int episode = 0;
    public string scenarioTitle = "";


    public bool openFlag = false;
    public bool clearFlag = false;
}
