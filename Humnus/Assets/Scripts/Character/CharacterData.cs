using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Attribute;


public class CharacterData : ScriptableObject, IManagedByID
{
    [SerializeField] public int ID { get; set; }

    public string characterName;
    public Sprite icon;
    public string info;
    public int hp = 1;

    public string skillName;
    public string skillInfo;
}
