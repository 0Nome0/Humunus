using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NerScript;
using NerScript.Attribute;
using UniRx;

public class PositionClamp : MonoBehaviour
{
    [Serializable] public class ClampEvent
    {
        public UnityEvent onExitEvent = null;
    }

    [SerializeField] private Transform clampObject = null;
    [SerializeField] private Vector3 min = new Vector2();
    [SerializeField] private Vector3 max = new Vector2();

    [SerializeField] private bool clampUpdate = true;
    [SerializeField] private bool useOnExit = false;
    [SerializeField, ShowIf("useOnExit")] private ClampEvent clampEvent = null;
    public IOptimizedObservable<Unit> OnExit => onExit;
    private Subject<Unit> onExit = new Subject<Unit>();

    public bool WithIn
    {
        get
        {
            Vector3 pos = clampObject.position;
            if(pos.x < min.x)
            {
                return false;
            }
            if(pos.y < min.y)
            {
                return false;
            }
            if(pos.z < min.z)
            {
                return false;
            }
            if(max.x < pos.x)
            {
                return false;
            }
            if(max.y < pos.y)
            {
                return false;
            }
            if(max.z < pos.z)
            {
                return false;
            }
            return true;
        }
    }

    public Bounds Bounds => new Bounds((max + min) / 2, (max - min));





    private void Start()
    {
        if(clampUpdate)
        {
            Observable
               .EveryLateUpdate()
               .TakeUntilDestroy(gameObject)
               .TakeUntilDestroy(clampObject.gameObject)
               .Where(_ => gameObject.activeInHierarchy && enabled)
               .Subscribe(_ => { ClampObject(); });
        }
    }

    private void OnValidate()
    {
        if(max.x < min.x) max.x = min.x;
        if(max.y < min.y) max.y = min.y;
        if(max.z < min.z) max.z = min.z;
    }


    public void ClampObject()
    {
        if(useOnExit)
        {
            ClampWithEvent();
        }
        else
        {
            ClampNormal();
        }
    }

    private bool Clamp()
    {
        Vector3 pos = clampObject.position;
        bool res = false;
        if(pos.x < min.x)
        {
            pos.x = min.x;
            res = true;
        }
        if(max.x < pos.x)
        {
            pos.x = max.x;
            res = true;
        }
        if(pos.y < min.y)
        {
            pos.y = min.y;
            res = true;
        }
        if(max.y < pos.y)
        {
            pos.y = max.y;
            res = true;
        }
        if(pos.z < min.z)
        {
            pos.z = min.z;
            res = true;
        }
        if(max.z < pos.z)
        {
            pos.z = max.z;
            res = true;
        }
        if(res) clampObject.position = pos;
        return res;
    }

    private void ClampNormal()
    {
        Clamp();
    }

    private void ClampWithEvent()
    {
        if(Clamp())
        {
            ExitEvent();
        }
    }
    public void ClampEventOnly()
    {
        if(!useOnExit) return;
        if(!WithIn)
        {
            ExitEvent();
        }
    }

    private void ExitEvent()
    {
        onExit.OnNext(Unit.Default);
        clampEvent.onExitEvent.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        using(GizmosLib.Coloring(Colors.Red))
        {
            GizmosLib.DrawCube((max + min) / 2, (max - min));
        }
    }
}
