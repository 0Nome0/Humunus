using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private List<GameObject> destroyObjects = null;

    public void Destroy()
    {
        foreach(var destroyObject in destroyObjects)
        {
            Destroy(destroyObject);
        }
    }
}
