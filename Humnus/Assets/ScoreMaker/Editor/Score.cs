using System;
[Serializable]
public class Score 
{
    public float time;
    public Pattern pattern;

    
}
[Serializable]
public enum Pattern
{
    Tap,
    Lflick,
    Rflick,
    LongStart,
    LongEnd,

}