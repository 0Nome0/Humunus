using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValidateEvent : MonoBehaviour
{
    [SerializeField] private bool InvokeTrigger = false;
    [SerializeField] private UnityEvent Event = null;

    private void OnValidate()
    {
        Event.Invoke();
    }
}
