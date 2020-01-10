using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;
using System.Collections.ObjectModel;

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

        public bool IsExpectedEvent(NotifyCollectionChangedAction action, IList<TKey> keys)
        {
            _collection.CollectionChanged -= CatchEvents;

            try
            {
                var actualEvent = _events[0];
                var actualKeys = (actualEvent.NewItems[0] as Collection<(int Index, IReadOnlyList<TKey> Keys)>)[0].Keys.ToList();
                var keysExist = true;

                foreach (var key in keys)
                {
                    keysExist = keysExist && actualKeys.Contains(key);
                }

                return keysExist && _events.Count == 1 && actualEvent.Action == action;
            }
            catch
            {
                return false;
            }
        }

        private void CatchEvents(object sender, NotifyCollectionChangedEventArgs e)
        {
            _events.Add(e);
        }
    }
}