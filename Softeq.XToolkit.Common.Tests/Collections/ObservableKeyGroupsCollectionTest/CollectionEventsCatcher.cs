// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class CollectionEventsCatcher<TKey, TValue>
    {
        private readonly ObservableKeyGroupsCollection<TKey, TValue> _collection;
        private readonly IList<NotifyCollectionChangedEventArgs> _events = new List<NotifyCollectionChangedEventArgs>();

        public int EventCount => _events.Count;

        public static CollectionEventsCatcher<TKey, TValue> Create(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            return new CollectionEventsCatcher<TKey, TValue>(collection);
        }

        private CollectionEventsCatcher(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            _collection = collection;
        }

        public void Subscribe()
        {
            _collection.CollectionChanged += CatchEvents;
        }

        public void Unsubscribe()
        {
            _collection.CollectionChanged -= CatchEvents;
        }

        public bool IsExpectedEvent(NotifyCollectionChangedAction action, IList<KeyValuePair<TKey, IList<TValue>>> pairs)
        {
            return IsExpectedEvent(action, pairs.Select(x => x.Key).ToList());
        }

        public bool IsExpectedEvent(NotifyCollectionChangedAction action, IList<TKey> keys = null)
        {
            try
            {
                var isActionSame = _events[0].Action == action;
                var isKeysMatched = true;
                var actualKeys = new List<TKey>();

                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        actualKeys = _events[0].NewItems != null ? (_events[0].NewItems[0] as Collection<(int Index, IReadOnlyList<TKey> Keys)>)[0].Keys.ToList() : new List<TKey>();
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        actualKeys = _events[0].OldItems != null ? (_events[0].OldItems[0] as Collection<(int Index, IReadOnlyList<TKey> Keys)>)[0].Keys.ToList() : new List<TKey>();
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        actualKeys = _events[0].NewItems != null ? (_events[0].NewItems[0] as Collection<(int Index, IReadOnlyList<TKey> Keys)>)[0].Keys.ToList() : new List<TKey>();
                        break;
                }

                if (keys != null)
                {
                    if (keys.Count == actualKeys.Count)
                    {
                        foreach (var key in keys)
                        {
                            isKeysMatched = isKeysMatched && actualKeys.Contains(key);
                        }
                    }
                    else
                    {
                        isKeysMatched = false;
                    }
                }
                else if (actualKeys.Count != 0)
                {
                    isKeysMatched = false;
                }

                return isActionSame && isKeysMatched;
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