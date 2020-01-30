using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerEvent2D : ColliderEvent
{
    private void OnTriggerEnter2D(Collider2D collider) { onEnter.Invoke(); }
    private void OnTriggerStay2D(Collider2D collider) { onStay.Invoke(); }
    private void OnTriggerExit2D(Collider2D collider) { onExit.Invoke(); }
}
