// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.EventArguments
{
    public class NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> : NotifyGroupCollectionChangedArgs<TKey>
    {
        public IReadOnlyList<KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>> GroupEvents { get; private set; }

        public static NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> Create(
            NotifyCollectionChangedAction? action,
            IDictionary<IReadOnlyList<TKey>, int> newItems,
            IDictionary<IReadOnlyList<TKey>, int> oldItems,
            IReadOnlyList<KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>> groupEvents)
        {
            return new NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>
            {
                Action = action,
                NewItems = newItems,
                OldItems = oldItems,
                GroupEvents = groupEvents
            };
        }
    }

    public class NotifyGroupCollectionChangedArgs<T> : EventArgs
    {
        public NotifyCollectionChangedAction? Action { get; protected set; }

        public IDictionary<IReadOnlyList<T>, int> NewItems { get; protected set; }

        public IDictionary<IReadOnlyList<T>, int> OldItems { get; protected set; }

        public static NotifyGroupCollectionChangedArgs<T> Create(
            NotifyCollectionChangedAction action,
            IDictionary<IReadOnlyList<T>, int> newItems,
            IDictionary<IReadOnlyList<T>, int> oldItems)
        {
            return new NotifyGroupCollectionChangedArgs<T>
            {
                Action = action,
                NewItems = newItems,
                OldItems = oldItems
            };
        }
    }
}
