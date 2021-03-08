using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllowOnlyOneOpen.Extensions
{
    public static class Ext
    {
        public static IDisposable AllowOnlyOneTrue(this IEnumerable<IReactiveProperty<bool>> source)
            => source
            .CombineLatest()
            .Pairwise()
            .Where(x => x.NewItem.SkipWhile(n => !n).Skip(1).Any(n => n))
            .Subscribe(p => p.OldItem
                .Select((v, i) => (v, i))
                .Where(x => x.v)
                .Select(x => source.ElementAt(x.i).Value = false)
                .ToList());
    }
}
