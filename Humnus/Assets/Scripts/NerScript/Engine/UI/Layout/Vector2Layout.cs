using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using NerScript.UI;

[Serializable]
public class Vector2Layout
{
    public Vector2 origin = Vector2.zero;
    public Vector2 space = Vector2.zero;
    public bool horizontalLayout = true;
    public int elementCount = 10;
    public int elementLayoutCount = 3;
    public Directional direction = Directional.RightDown;


    public void Layout(int? count, Action<Vector2, int> onLayout)
    {
        Vector2 position = origin;
        Vector2 velosity;

        int element = 0;
        if (count != null) elementCount = count.Value;
        for (int i = 0; i < elementCount; i++)
        {
            onLayout.Invoke(position, i);
            element++;

            velosity = Vector2.zero;
            if (horizontalLayout) { velosity.x += space.x; }
            else { velosity.y += space.y; }

            if (elementLayoutCount <= element)
            {
                element = 0;
                if (horizontalLayout)
                {
                    velosity.x -= elementLayoutCount * space.x;
                    velosity.y += space.y;
                }
                else
                {
                    velosity.y -= elementLayoutCount * space.y;
                    velosity.x += space.x;
                }
            }

            velosity *= direction.ToVector2();
            position += velosity;
        }

    }
}
