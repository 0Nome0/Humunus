using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NerScript;
using NerScript.Attribute;
using UniRx;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private ScrollRect scroll;

    [SerializeField] private float gridSize = 1;
    [SerializeField] private float gridPower = 0.1f;
    [SerializeField, Range(0, 1)] private float antiScrollPower = 0;

    [SerializeField] private SpaceType spaceType = SpaceType.Local;
    [SerializeField] private Axis axis = Axis.X;

    public int CurrentGrid { get; private set; }

    private bool isGriding = true;


    private Subject<int> onGrid = new Subject<int>();
    public IOptimizedObservable<int> OnGrid => onGrid;

    private Subject<int> onGridChange = new Subject<int>();
    public IOptimizedObservable<int> OnGridChange => onGridChange;

    private Vector3 Position
    {
        get => transform.GetPosition(spaceType);
        set => transform.SetPosition(spaceType, value);
    }



    public void StopGrig()
    {
        isGriding = false;
    }

    public void StartGrid()
    {
        isGriding = true;
    }

    public void Start()
    {
        isGriding = true;
    }

    public void Update()
    {
        scroll.velocity -= scroll.velocity * antiScrollPower;
        int grid = GetGrid();
        float distance = (CurrentGrid * gridSize) - Position.GetAxis(axis);


        if (grid != CurrentGrid)
        {
            CurrentGrid = grid;
            onGridChange.OnNext(CurrentGrid);
        }

        if (!ShouldGrid())
        {
            if (gridPower * gridPower < Mathf.Abs(distance))
            {
                StartGrid();
            }
            return;
        }

        scroll.velocity = Vector2.zero;

        if (Mathf.Abs(distance) < gridPower * gridPower)
        {
            OnGridEnd(CurrentGrid);
        }
        transform.AddPosition(spaceType, axis, distance * gridPower);
    }

    public bool ShouldGrid()
    {
        return
            isGriding &&
            scroll.velocity.magnitude < gridSize * gridPower
            ;
    }

    public int GetGrid() => Mathf.RoundToInt(Position.GetAxis(axis) / gridSize);

    public void OnGridEnd(int grid)
    {
        transform.SetPosition(spaceType, axis, (grid * gridSize));
        CurrentGrid = grid;
        onGrid.OnNext(CurrentGrid);
        StopGrig();
    }
}
