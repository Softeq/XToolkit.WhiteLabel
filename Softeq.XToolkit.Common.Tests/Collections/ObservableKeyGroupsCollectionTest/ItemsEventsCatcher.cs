using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Collections.EventArgs;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ItemsEventsCatcher<TKey, TValue>
    {
        private readonly ObservableKeyGroupsCollection<TKey, TValue> _collection;
        private readonly IList<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> _events = new List<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>>();

        public int EventCount => _events.Count;

        public static ItemsEventsCatcher<TKey, TValue> Create(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            return new ItemsEventsCatcher<TKey, TValue>(collection);
        }

        private ItemsEventsCatcher(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            _collection = collection;
        }

        public void Subscribe()
        {
            _collection.ItemsChanged += CatchEvents;
        }

        public void Unsubscribe()
        {
            _collection.ItemsChanged -= CatchEvents;
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
                        actualKeys = _events[0].NewItemRanges != null ? _events[0].NewItemRanges[0].NewItems.ToList() : new List<TKey>();
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        actualKeys = _events[0].OldItemRanges != null ? _events[0].OldItemRanges[0].OldItems.ToList() : new List<TKey>();
                        break;
                        //default:
                        //    break;
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

        private void CatchEvents(object sender, NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> e)
        {
            _events.Add(e);
        }
    }
}
