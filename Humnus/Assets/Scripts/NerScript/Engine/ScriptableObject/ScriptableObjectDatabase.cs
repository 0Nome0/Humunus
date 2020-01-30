using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Attribute;
using Object = UnityEngine.Object;

namespace NerScript
{
    [CreateAssetMenu(fileName = "ScriptableObjectDatabase", menuName = "NerScript/ScriptableObjectDatabase", order = 1000)]
    public class ScriptableObjectDatabase : ScriptableObject, IManagedByID
    {
        [SerializeField] int IManagedByID.ID { get; set; }
        public bool enable = false;

        [SerializeField] public List<ScriptableObject> objects = new List<ScriptableObject>();
        public int Count => objects.Count();
        public List<T> GetListT<T>()
        {
            return objects.Select(o => (T)(object)o).ToList();
        }

        public ScriptableObject GetObjectByID(int id) => objects[id];
        public T GetObjectByID<T>(int id) => GetListT<T>()[id];
    }
}
