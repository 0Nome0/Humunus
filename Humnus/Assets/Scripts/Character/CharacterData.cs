using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Attribute;


public class CharacterData : ScriptableObject, IManagedByID
{
    [SerializeField] public int ID { get; set; }

    public bool isOpened = false;

    public string characterName;
    public Sprite icon;
    public Sprite icon2;
    public Sprite icon3;
    public Sprite iconAu;
    public string info;
    public int hp = 1;

    public string skillName;
    public string skillInfo;

    public AudioClip SVoice = null;
    public AudioClip voice1 = null;
    public AudioClip voice2 = null;
    public AudioClip voice3 = null;
}