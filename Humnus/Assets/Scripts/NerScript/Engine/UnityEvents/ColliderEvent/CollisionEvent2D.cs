using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CollisionEvent2D : ColliderEvent
{
    private void OnCollisionEnter2D(Collision2D collision) { onEnter.Invoke(); }
    private void OnCollisionStay2D(Collision2D collision) { onStay.Invoke(); }
    private void OnCollisionExit2D(Collision2D collision) { onExit.Invoke(); }
}
