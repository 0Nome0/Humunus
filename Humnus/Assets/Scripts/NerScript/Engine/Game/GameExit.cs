using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameExit: MonoBehaviour
{
    public void ExitGame() => Exit();
    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
