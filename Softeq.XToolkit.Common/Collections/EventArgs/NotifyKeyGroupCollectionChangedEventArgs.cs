// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.Collections.EventArgs
{
    public class NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> : NotifyGroupCollectionChangedEventArgs<TKey>
    {
        public NotifyKeyGroupCollectionChangedEventArgs(
            NotifyCollectionChangedAction? action,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)>? newItems,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> OldItems)>? oldItems,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedEventArgs<TValue> Arg)>? groupEvents)
            : base(action, newItems, oldItems)
        {
            GroupEvents = groupEvents;
        }

        public IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedEventArgs<TValue> Arg)>? GroupEvents { get; private set; }
    }
}
