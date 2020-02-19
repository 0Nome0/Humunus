using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct Notes : IEquatable<Notes>
{
    public float time;
    public NotesType type;
    public int damage;
    public bool isHiSpeed;

    public Notes(float _time, NotesType _type, int _damage, bool _isHiSpeed)
    {
        time = _time;
        type = _type;
        damage = _damage;
        isHiSpeed = _isHiSpeed;
    }

    public bool Equals(Notes other) => time == other.time && type == other.type;
    public static bool operator ==(Notes self, Notes other) => self.Equals(other);
    public static bool operator !=(Notes self, Notes other) => !self.Equals(other);
}


public enum NotesType
{
    Tap,
    Start,
    End,
    Double,
    sLeft,
    sRight,
    sUp,
    Dont,
}
