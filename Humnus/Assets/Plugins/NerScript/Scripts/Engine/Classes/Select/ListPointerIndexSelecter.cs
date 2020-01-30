using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NerScript
{
    public sealed class ListPointerIndexSelector<T> : ListIndexSelector
    {
        private readonly IListPointer<T> pList = null;
        public override IList List
        {
            get => (IList)pList.Value;
            set
            {
                pList.Value = (IList<T>)value;
                Optimize();
            }
        }

        public ListPointerIndexSelector(IListPointer<T> _pList) : base((IList)_pList.Value)
        {
            pList = _pList;
        }
    }
}
