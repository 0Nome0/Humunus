using System;
using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace NerScript.Editor
{
    public static class EditorAudioUtility
    {
        public static AudioClip currentClip = null;



        public static void PlayClip(AudioClip clip)
        {
            StopClip();
            currentClip = clip;
            Type audioUtilClass = EditorLib.UnityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod("PlayClip", ReflectionLib.flag);
            method.Invoke(null, new object[] { currentClip, 0, false });
        }

        public static void StopClip()
        {
            if (currentClip == null) return;
            Type audioUtilClass = EditorLib.UnityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod("StopClip", ReflectionLib.flag);
            method.Invoke(null, new object[] { currentClip });
            currentClip = null;
        }
    }
}