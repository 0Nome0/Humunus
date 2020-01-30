using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Anime;
using UnityEngine.UI;

public class NotesObject : MonoBehaviour
{
    public Image image;
    public Notes notes = default;
    public ObjectAnimationController animController = null;
    public bool endFlag = false;

    public void Destroy()
    {
        animController?.Destroy();
    }
}
