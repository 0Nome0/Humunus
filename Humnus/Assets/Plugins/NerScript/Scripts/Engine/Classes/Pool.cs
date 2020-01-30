using System;
using System.Collections.Generic;

namespace NerScript
{
    public class Pool<T> where T : class
    {
        private readonly List<T> all = new List<T>();

        private readonly Queue<T> pool = new Queue<T>();
        private readonly List<T> @using = new List<T>();

        private readonly Func<int, T> create = null;
        private readonly Action<T> initialize = null;
        private readonly Action<T> finalize = null;

        public int Count => all.Count;

        /// <summary>
        ///
        /// </summary>
        /// <param name="createFunc">(int):(index),</param>
        /// <param name="initializeAct"></param>
        /// <param name="finalizeAct"></param>
        public Pool(Func<int, T> createFunc, Action<T> initializeAct, Action<T> finalizeAct)
        {
            create = createFunc;
            initialize = initializeAct;
            finalize = finalizeAct;
        }

        public T Get()
        {
            if (0 < pool.Count) { return GetFromPool(); }
            return GetWithCreate();
        }

        private void Using(T t)
        {
            @using.Add(t);
            initialize(t);
        }
        public void Used(T used)
        {
            @using.Remove(used);
            finalize(used);
            pool.Enqueue(used);
        }

        private T GetFromPool()
        {
            T t = pool.Dequeue();
            Using(t);
            return t;
        }

        private T GetWithCreate()
        {
            T t = Create();
            all.Add(t);
            Using(t);
            return t;
        }

        private T Create() { return create(all.Count); }


        public void ClearUsing()
        {
            while (0 < @using.Count) { Used(@using[0]); }
        }
        public void FillPool(int count)
        {
            while (pool.Count < count) { pool.Enqueue(Create()); }
        }
        public void AllInitialize()
        {
            foreach (var t in all) { initialize(t); }
        }
        public void AllFinalize()
        {
            foreach (var t in all) { finalize(t); }
        }

    }
}
