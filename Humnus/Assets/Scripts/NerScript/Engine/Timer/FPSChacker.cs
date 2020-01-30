using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FPSChacker : MonoBehaviour
{
    public Text text = null;
    public List<float> fpss = null;
    public int fps
    {
        get {
            return (int)fpss.Average();
        }
    }

    private void Update()
    {
        fpss.Add(1.0f / Time.deltaTime);
        if (100 < fpss.Count) fpss.RemoveAt(0);
        if (text != null) text.text = $"FPS:{fps}";
    }
}
