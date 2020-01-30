using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NerScript;
using NerScript.Anime;
using NerScript.Input;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class ObjectSelector<T> : MonoBehaviour where T : UnityEngine.Object
{
    [SerializeField] private List<T> objects;
    public List<T> Objects => objects;

    protected Memorable<SelectInfo[]> SelectObjects { get; set; } = new Memorable<SelectInfo[]>();
    private AreaSelector areaSelector;
    public Map<T> ObjectMap { get; private set; }

    public IOptimizedObservable<SelectInfo[]> OnSelect => onSelect;
    public IObservable<SelectInfo[]> OnTSelectOnce => onSelect.Where(info => info != null).FirstOrDefault();
    public IObservable<SelectInfo[]> OnSelectOnce => onSelect.FirstOrDefault();
    private readonly Subject<SelectInfo[]> onSelect = new Subject<SelectInfo[]>();

    public IOptimizedObservable<Memorable<SelectInfo[]>> OnSelectChanged => onSelectChanged;
    private readonly Subject<Memorable<SelectInfo[]>> onSelectChanged = new Subject<Memorable<SelectInfo[]>>();

    public IOptimizedObservable<bool> OnEnableChanged => onEnableChanged;
    private readonly Subject<bool> onEnableChanged = new Subject<bool>();

    public class SelectInfo
    {
        public readonly Vector2Int selectIndex;
        public readonly T selectT;
        public SelectInfo(Vector2Int selectIndex, T selectT)
        {
            this.selectIndex = selectIndex;
            this.selectT = selectT;
        }
    }

    private void Awake()
    {
        ObjectMap = new Map<T>(3, 3)
        {
            [0, 0] = objects[0], [1, 0] = objects[1], [2, 0] = objects[2],
            [0, 1] = objects[3], [1, 1] = objects[4], [2, 1] = objects[5],
            [0, 2] = objects[6], [1, 2] = objects[7], [2, 2] = objects[8]
        };

        areaSelector = new AreaSelector(ObjectMap.size.x, ObjectMap.size.y, true);

        OnAwake();

        MemorySelect();
        MemorySelect();

        onSelectChanged.OnNext(SelectObjects);
    }

    protected virtual void OnAwake() { }

    public void SelectOn()
    {
        Disable();
        OnSelectOn();
    }
    protected virtual void OnSelectOn() { }

    public void SelectOff()
    {
        Enable();
        OnSelectOff();
    }
    protected virtual void OnSelectOff() { }

    public void Enable()
    {
        enabled = true;
        onEnableChanged.OnNext(enabled);
    }

    public void Disable()
    {
        enabled = false;
        onEnableChanged.OnNext(enabled);
    }

    public void SetRect(int xSize, int ySize)
    {
        Vector2Int position = areaSelector.SelectPoint;
        areaSelector.SetRect(xSize, ySize);
        areaSelector.SetPosition(position);
        MemorySelect();
    }
    public void SetRect(Vector2Int size)
    {
        SetRect(size.x, size.y);
    }

    private void Update()
    {
        bool change = false;
        if(Input.GetKeyDown(KeyCode.W))
        {
            areaSelector.MoveSelect(0, -1);
            change = true;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            areaSelector.MoveSelect(-1, 0);
            change = true;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            areaSelector.MoveSelect(0, 1);
            change = true;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            areaSelector.MoveSelect(1, 0);
            change = true;
        }
        else if(InputManager.GetKeyDown(KeyCode.Backspace))
        {
            onSelect.OnNext(null);
        }
        else if(InputManager.GetKeyDown(KeyCode.Return))
        {
            onSelect.OnNext(SelectObjects.Current.ToArray());
        }

        if(change)
        {
            MemorySelect();
            onSelectChanged.OnNext(SelectObjects);
        }
        OnUpdated();
    }
    private void MemorySelect()
    {
        SelectObjects.Value =
        areaSelector
        .SelectPoints
        .Select(p => new SelectInfo(p, ObjectMap[p.x, p.y]))
        .ToArray();
    }
    protected virtual void OnUpdated() { }
}
