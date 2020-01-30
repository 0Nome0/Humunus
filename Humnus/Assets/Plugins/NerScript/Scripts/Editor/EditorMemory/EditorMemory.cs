using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{

    public class EditorMemory : SingletonClass<EditorMemory>
    {
        //private static EditorMemory GetorCreate()
        //{
        //    return AssetDatabase.FindAssets("l:EditorMemory")
        //   .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
        //   .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(EditorMemory)))
        //   .Where(obj => obj != null)
        //   .Select(obj => (EditorMemory)obj)
        //   .ToArray()[0];
        //}

        //[SerializeField] private List<Memory> memory = new List<Memory>();

        //private string GetNameForType<T>(T obj) => "_/" + obj.GetType();
        //private string GetNameForType(Type type) => "_/" + type;

        //private void _AddMemory<T>(string name, T obj) where T : Object
        //{
        //    if (_ContainsName(name)) throw new ArgumentException("Already contains that Name.");
        //    memory.Add(new Memory(name, obj));
        //}
        //public static void AddMemory<T>(string name, T obj) where T : Object { Instance._AddMemory(name, obj); }
        //private void _AddObject<T>(T obj) where T : Object
        //{
        //    string name = GetNameForType(obj);
        //    if (_ContainsName(name)) throw new ArgumentException("Already contains that Name.");
        //    memory.Add(new Memory(name, obj));
        //}
        //public static void AddObject<T>(T obj) where T : Object { Instance._AddObject(obj); }

        //private void _SetMemory<T>(string name, T obj) where T : Object
        //{
        //    if (ContainsName(name))
        //    {
        //        Memory m = _GetMemory(name);
        //        m.obj = obj;
        //        return;
        //    }
        //    _AddMemory(name, obj);
        //}
        //public static void SetMemory<T>(string name, T obj) where T : Object { Instance._SetMemory(name, obj); }
        //private void _SetObject<T>(T obj) where T : Object
        //{
        //    string name = GetNameForType(obj);
        //    if (ContainsName(name))
        //    {
        //        Memory m = _GetMemory(name);
        //        m.obj = obj;
        //        return;
        //    }
        //    _AddObject(obj);
        //}
        //public static void SetObject<T>(T obj) where T : Object { Instance._SetObject(obj); }


        //private Memory _GetMemory(string name)
        //{
        //    if (!_ContainsName(name)) throw new KeyNotFoundException("Not found that key.");
        //    return memory.Find(m => m.name == name);
        //}
        //public static Memory GetMemory(string name) { return Instance._GetMemory(name); }
        //private T _GetObject<T>() where T : Object
        //{
        //    if (!_ContainsName(name)) throw new KeyNotFoundException("Not found that key.");
        //    return (T)memory.Find(m => m.name == GetNameForType(typeof(T))).obj;
        //}
        //public static T GetObject<T>() where T : Object { return Instance._GetObject<T>(); }


        //private bool _ContainsName(string name)
        //{
        //    return memory.FindIndex(m => m.name == name) != -1;
        //}
        //public static bool ContainsName(string name) { return Instance._ContainsName(name); }

        //private void _RemoveMemory(string name)
        //{
        //    if (!ContainsName(name)) throw new KeyNotFoundException("Not found that key.");
        //}


        //[Serializable]
        //public struct Memory
        //{
        //    public string name;
        //    public Object obj;
        //    public Memory(string _name, Object _obj)
        //    {
        //        name = _name;
        //        obj = _obj;
        //    }
        //}


    }
}