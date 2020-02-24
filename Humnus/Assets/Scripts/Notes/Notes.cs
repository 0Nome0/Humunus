using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct Notes : IEquatable<Notes>
{
    public float time;
    public float Score
    {
        get
        {
            switch(type)
            {
                case NotesType.Tap:   return 100;
                case NotesType.Start: return 50;
                case NotesType.End:   return 50;
                case NotesType.Slide: return 100;
            }
            return 0;
        }
    }
    public NotesType type;
    public int Damage
    {
        get
        {
            switch(type)
            {

                case NotesType.Tap: return 20;
                case NotesType.Start: return 10;
                case NotesType.End: return 10;
                case NotesType.Slide: return 20;
            }
            return 0;
        }
    }

    public Notes(float _time, NotesType _type)
    {
        time = _time;
        type = _type;
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
