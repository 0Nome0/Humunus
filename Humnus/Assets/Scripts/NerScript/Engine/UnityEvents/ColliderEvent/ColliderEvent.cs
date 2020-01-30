using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class ColliderEvent : MonoBehaviour
{
    [SerializeField] private ColliderEvents events = null;

    protected UnityEvent onEnter => events.onEnter;
    protected UnityEvent onStay => events.onStay;
    protected UnityEvent onExit => events.onExit;

    [Serializable]
    private class ColliderEvents
    {
        public UnityEvent onEnter = null;
        public UnityEvent onStay = null;
        public UnityEvent onExit = null;
    }
}