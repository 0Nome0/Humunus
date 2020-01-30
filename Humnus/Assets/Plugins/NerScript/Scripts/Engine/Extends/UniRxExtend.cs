using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

namespace NerScript
{
    public static class UniRxExtend
    {
        public static IObservable<T> Action<T>(this IObservable<T> source, Action<Unit> action)
        {
            action(Unit.Default);
            return source;
        }
    }
}
