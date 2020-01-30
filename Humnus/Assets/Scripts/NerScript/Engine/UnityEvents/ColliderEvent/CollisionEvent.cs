using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CollisionEvent : ColliderEvent
{
    private void OnCollisionEnter(Collision collision) { onEnter.Invoke(); }
    private void OnCollisionStay(Collision collision) { onStay.Invoke(); }
    private void OnCollisionExit(Collision collision) { onExit.Invoke(); }
}
