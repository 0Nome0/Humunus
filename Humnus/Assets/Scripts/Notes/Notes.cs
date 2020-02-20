using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct Notes : IEquatable<Notes>
{
    public float time;
    public float score;
    public NotesType type;
    public int Damage
    {
        get
        {
            switch(type)
            {

                case NotesType.Tap:
                    break;
                case NotesType.Start:
                    break;
                case NotesType.End:
                    break;
                case NotesType.Slide:
                    break;
                case NotesType.Dont:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return 10;
        }
    }
    public bool isHiSpeed;

    public Notes(float _time, float _score, NotesType _type, bool _isHiSpeed)
    {
        time = _time;
        score = _score;
        type = _type;
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
    Slide,
    Dont,
}
