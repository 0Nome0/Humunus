using System;
using UnityEngine;
using UniRx;
using NerScript.Attribute;


public class Instantiater : MonoBehaviour
{
    [SerializeField] private bool startInstantiate = false;
    [SerializeField] private GameObject original = null;
    [SerializeField] private Transform parent = null;

    [SerializeField] private bool loopInstantiate = false;
    [SerializeField, ShowIf("loopInstantiate")] private int loopCount = 1;

    public void Start()
    {
        OnValidate();
        if (startInstantiate) { Instantiate(); }
    }
    private void OnValidate()
    {
        if (!loopInstantiate) { loopCount = 1; }
    }

    private GameObject _Instantiate()
    {
        if (original == null) return null;
        return Instantiate(original, transform.position, transform.rotation, parent ?? transform);
    }
    public void Instantiate()
    {
        OnValidate();
        for (int i = 0; i < loopCount; i++)
            _Instantiate();
    }
    public void Instantiate(Action<int, GameObject> onInstantiate)
    {
        OnValidate();
        for (int i = 0; i < loopCount; i++)
            onInstantiate.Invoke(i, _Instantiate());
    }
}
