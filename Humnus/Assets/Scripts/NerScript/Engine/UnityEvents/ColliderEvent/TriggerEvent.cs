using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerEvent : ColliderEvent
{
    private void OnTriggerEnter(Collider collider) { onEnter.Invoke(); }
    private void OnTriggerStay(Collider collider) { onStay.Invoke(); }
    private void OnTriggerExit(Collider collider) { onExit.Invoke(); }
}
