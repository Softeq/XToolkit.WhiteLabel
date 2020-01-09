using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class CollectionEventsCatcher<TKey, TValue>
    {
        private readonly ObservableKeyGroupsCollection<TKey, TValue> _collection;
        private readonly IList<NotifyCollectionChangedEventArgs> _events = new List<NotifyCollectionChangedEventArgs>();

        private CollectionEventsCatcher(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            _collection = collection;
        }

        public static CollectionEventsCatcher<TKey, TValue> Create(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            return new CollectionEventsCatcher<TKey, TValue>(collection);
        }

        public void Subscribe()
        {
            _collection.CollectionChanged += CatchEvents;
        }

        public bool IsSame(NotifyCollectionChangedAction action, IList<TKey> keys)
        {
            _collection.CollectionChanged -= CatchEvents;

            return _events.Count == 1 && _events[0].Action == action;
        }

        private void CatchEvents(object sender, NotifyCollectionChangedEventArgs e)
        {
            _events.Add(e);
        }
    }
}