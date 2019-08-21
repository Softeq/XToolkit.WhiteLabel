// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.EventArguments
{
    public class NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> : NotifyGroupCollectionChangedArgs<TKey>
    {
        public IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedArgs<TValue> Arg)> GroupEvents { get; private set; }

        public static NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> Create(
            NotifyCollectionChangedAction? action,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> newItems,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> oldItems,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedArgs<TValue> Arg)> groupEvents)
        {
            return new NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>
            {
                Action = action,
                NewItemRanges = newItems,
                OldItemRanges = oldItems,
                GroupEvents = groupEvents
            };
        }
    }

    public class NotifyGroupCollectionChangedArgs<T> : EventArgs
    {
        public NotifyCollectionChangedAction? Action { get; protected set; }

        public IReadOnlyList<(int Index, IReadOnlyList<T> NewItems)> NewItemRanges { get; protected set; }

        public IReadOnlyList<(int Index, IReadOnlyList<T> NewItems)> OldItemRanges { get; protected set; }

        public static NotifyGroupCollectionChangedArgs<T> Create(
            NotifyCollectionChangedAction action,
            IReadOnlyList<(int Index, IReadOnlyList<T> NewItems)> newItems,
            IReadOnlyList<(int Index, IReadOnlyList<T> NewItems)> oldItems)
        {
            return new NotifyGroupCollectionChangedArgs<T>
            {
                Action = action,
                NewItemRanges = newItems,
                OldItemRanges = oldItems
            };
        }
    }
}
