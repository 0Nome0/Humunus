using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;


namespace NerScript
{
    public class Map<T>
    {
        private T[,] map;
        public T[,] TMap => map;
        public Vector2Int size;
        public int Area => size.x * size.y;

        public List<T> GetList()
        {
            T[] array = new T[Area];
            for(int y = 0, i = 0; y < size.x; y++)
            {
                for(int x = 0; x < size.y; x++, i++)
                {
                    array[i] = map[x, y];
                }
            }
            return array.ToList();
        }

        public List<(T t, Vector2Int point)> GetListWithPoint()
        {
            (T, Vector2Int point)[] array = new (T, Vector2Int point)[Area];
            for(int y = 0, i = 0; y < size.x; y++)
            {
                for(int x = 0; x < size.y; x++, i++)
                {
                    array[i] = (map[x, y], new Vector2Int(x, y));
                }
            }
            return array.ToList();
        }

        public Map(int x, int y)
        {
            map = new T[x, y];
            size = new Vector2Int(x, y);
        }
        public T this[int x, int y]
        {
            get => map[x, y];
            set => map[x, y] = value;
        }
        public T this[Vector2Int point]
        {
            get => map[point.x, point.y];
            set => map[point.x, point.y] = value;
        }


    }
}
