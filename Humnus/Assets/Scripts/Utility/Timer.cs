using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    public float currentTime { get; private set; }
    public float limitTime { get; private set; }

    public Timer(float time = 0)
    {
        Initialize();
        limitTime = time;
    }

    public void Initialize()
    {
        currentTime = 0;
    }

    public void UpdateTime()
    {
        currentTime += Time.deltaTime;
    }

    public bool IsTime()
    {
        return currentTime >= limitTime;
    }
}
