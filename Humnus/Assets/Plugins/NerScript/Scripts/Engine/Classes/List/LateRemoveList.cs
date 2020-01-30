using System;
using System.Collections.Generic;
using NerScript;
using UnityEngine;

public class LateRemoveList<T>
{
    public List<T> list = null;
    private readonly List<T> removeList = new List<T>();

    public LateRemoveList(List<T> _list = null)
    {
        list = _list ?? new List<T>();
    }
    public void AddRemoveList(T item)
    {
        if(!list.Contains(item)) return;
        removeList.Add(item);
    }
    public void Remove(Action<T> onRemove=null)
    {
        foreach(T item in removeList)
        {
            list.Remove(item);
            onRemove?.Invoke(item);
        }
        removeList.Clear();
    }

}
