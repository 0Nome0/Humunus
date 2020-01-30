using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Attribute;


namespace NerScript
{
    public class TransformMemory : MonoBehaviour
    {
        public List<GameObject> gameObjects = null;
        public List<Memorys> memoryGroup = null;
        public int MemoryCount => memoryGroup.Count;

        private void Awake()
        {



        }



        public void SetGameObject(List<GameObject> _gameobjects) { gameObjects = _gameobjects; }
        public void AddMemory() { memoryGroup.Add(new Memorys(gameObjects)); }
        public void Save(int index) { memoryGroup[index].Save(gameObjects); }
        public void Load(int index) { memoryGroup[index].Load(gameObjects); }






        [Serializable]
        public struct Memorys
        {
            [SerializeField] public List<Memory> memorys;
            public string name;

            public Memorys(List<GameObject> objects)
            {
                memorys =
                    objects
                    .Select(obj =>
                    {
                        Memory m = new Memory();
                        m.Save(obj);
                        return m;
                    })
                    .ToList();
                name = "new Memory";
            }
            public void Save(List<GameObject> gameObjects)
            {
                for (int i = 0; i < memorys.Count; i++)
                {
                    memorys[i].Save(gameObjects[i]);
                }
            }
            public void Load(List<GameObject> gameObjects)
            {
                for (int i = 0; i < memorys.Count; i++)
                {
                    memorys[i].Load(gameObjects[i]);
                }
            }
        }

        [Serializable]
        public struct Memory
        {
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
            public void Save(GameObject obj)
            {
                position = obj.transform.localPosition;
                rotation = obj.transform.localEulerAngles;
                scale = obj.transform.localScale;
            }
            public void Load(GameObject obj)
            {
                obj.transform.localPosition = position;
                obj.transform.localEulerAngles = rotation;
                obj.transform.localScale = scale;
            }
        }
    }
}