using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Attribute;


public class CharacterData : ScriptableObject, IManagedByID
{
    [SerializeField] public int ID { get; set; }

    public string characterName = "";
    public Sprite icon = null;
    public string info = "";
}
