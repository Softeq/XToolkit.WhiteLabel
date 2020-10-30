// Developed by Softeq Development Corporation
// http://www.softeq.com

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

        private ItemsEventsCatcher(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            _collection = collection;
        }

        public int EventCount => _events.Count;

        public static ItemsEventsCatcher<TKey, TValue> Create(ObservableKeyGroupsCollection<TKey, TValue> collection)
        {
            return new ItemsEventsCatcher<TKey, TValue>(collection);
        }

        public void Subscribe()
        {
            _collection.ItemsChanged += CatchEvents;
        }

        public void Unsubscribe()
        {
            _collection.ItemsChanged -= CatchEvents;
        }

        public bool IsExpectedEvent(NotifyCollectionChangedAction action, IList<TValue> values = null)
        {
            try
            {
                var isActionSame = _events[0].GroupEvents[0].Arg.Action == action;
                var isValuesMatched = true;
                var actualValues = new List<TValue>();

                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        actualValues = _events[0].GroupEvents
                            .SelectMany(x => x.Arg.NewItemRanges.SelectMany(y => y.NewItems))
                            .ToList();
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        actualValues = _events[0].GroupEvents
                            .SelectMany(x => x.Arg.OldItemRanges.SelectMany(y => y.OldItems))
                            .ToList();
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        break;
                }

                if (values != null)
                {
                    if (values.Count == actualValues.Count)
                    {
                        foreach (var value in values)
                        {
                            isValuesMatched = isValuesMatched && actualValues.Contains(value);
                        }
                    }
                    else
                    {
                        isValuesMatched = false;
                    }
                }
                else if (actualValues.Count != 0)
                {
                    isValuesMatched = false;
                }

                return isActionSame && isValuesMatched;
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
