using UnityEngine;
using NerScript.Resource;

public class AudioAPIUser : MonoBehaviour
{
    public void PlayBGM(string name) => AudioAPI.PlayBGM(name);
    public void StopBgm() => AudioAPI.StopBgm();
    public void PlaySE(string name) => AudioAPI.PlaySE(name);
    public void PlayVoice(string name) => AudioAPI.PlayVoice(name);
}