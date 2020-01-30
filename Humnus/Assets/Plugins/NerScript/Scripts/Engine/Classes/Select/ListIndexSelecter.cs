using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NerScript
{
    public class ListIndexSelector : IndexSelector
    {
        private IList list = null;
        public virtual IList List
        {
            get => list;
            set
            {
                list = value;
                Optimize();
            }
        }

        protected override int Count => List.Count;

        public ListIndexSelector(IList _list, bool allowNonSelect = true, bool loopSelect = false)
        : base(_list.Count, allowNonSelect, loopSelect)
        {
            list = _list;
        }

        public List<T> GetSelectList<T>()
        {
            List<T> result = new List<T>();
            foreach(var index in selectIndexes)
            {
                result.Add((T)List[index]);
            }
            return result;
        }

    }
}
