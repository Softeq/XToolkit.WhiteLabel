// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.EventArguments;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Common.Collections
{
    public sealed class ObservableKeyGroupsCollectionNew<TKey, TValue> : IObservableKeyGroupsCollection<TKey, TValue>,
        INotifyKeyGroupCollectionChanged<TKey, TValue>
    {
        private readonly IList<KeyValuePair<TKey, ICollection<TValue>>> _items;

        public event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;

        public ObservableKeyGroupsCollectionNew()
        {
            _items = new List<KeyValuePair<TKey, ICollection<TValue>>>();
        }

        #region IObservableKeyGroupCollection

        public void AddGroups(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items)
        {
            var index = _items.Count;

            InsertGroups(index, items);
        }

        public void ReplaceGroups(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items)
        {
            var index = 0;

            var toRemove = new Dictionary<IReadOnlyList<TKey>, int> { [_items.Select(x => x.Key).ToList()] = index };

            _items.Clear();

            foreach (var item in items)
            {
                _items.Add(item);
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Replace,
                new Dictionary<IReadOnlyList<TKey>, int> { [items.Select(x => x.Key).ToList()] = index },
                toRemove,
                items.Select(x => new KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>(items.ToList().IndexOf(x),
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Dictionary<IReadOnlyList<TValue>, int>
                    {
                        [x.Value.ToList()] = 0
                    },
                    default))).ToList()));
        }

        public void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items)
        {
            int i = index;

            foreach (var item in items)
            {
                _items.Insert(i++, item);
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Add,
                new Dictionary<IReadOnlyList<TKey>, int> { [items.Select(x => x.Key).ToList()] = index },
                default,
                items.Select(x => new KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>(_items.ToList().IndexOf(x),
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Dictionary<IReadOnlyList<TValue>, int>
                    {
                        [x.Value.ToList()] = 0
                    },
                    default))).ToList()));
        }

        public void RemoveGroups(IEnumerable<TKey> keys)
        {
            var indexes = new List<KeyValuePair<TKey, int>>();

            for(int i = 0; i < _items.Count; i++)
            {
                if(keys.Any(x => x.Equals(_items[i].Key)))
                {
                    indexes.Add(new KeyValuePair<TKey, int>(_items[i].Key, i));
                }
            }

            indexes = indexes.OrderBy(x => x.Value).ToList();

            while(!IsIndexesGrouped())
            {
                DecrementIndex();
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Remove,
                default,
                indexes.GroupBy(x => x.Value)
                    .ToDictionary(x => (IReadOnlyList<TKey>) x.Select(y => y.Key).ToList(), x => x.Key),
                default));

            bool IsIndexesGrouped()
            {
                for (var i = 0; i < indexes.Count - 1; i++)
                {
                    if (indexes[i].Value == indexes[i + 1].Value - 1)
                    {
                        return false;
                    }
                }

                return true;
            }

            void DecrementIndex()
            {
                for (int i = indexes.Count - 1; i > 0; i--)
                {
                    if (indexes[i].Value - 1 == indexes[i - 1].Value)
                    {
                        indexes[i] = new KeyValuePair<TKey, int>(indexes[i].Key, indexes[i].Value - 1);
                        return;
                    }
                }
            }
        }

        public void ClearGroups()
        {
            _items.Clear();

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Reset,
                default,
                default,
                default));
        }

        public void AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var index = _items.Count;

            var result = AddItemsWithoutNotify(items, keySelector, valueSelector);

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Add,
                new Dictionary<IReadOnlyList<TKey>, int> { [result.keysToAdd] = index },
                default,
                result.itemsToAdd.Select(
                    x => new KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>(_items.IndexOf(_items.First(y => y.Key.Equals(x.ItemKey))), NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Dictionary<IReadOnlyList<TValue>, int> { [x.ItemValue.ToList()] = x.Index },
                        default
                ))).ToList()));
        }

        public void ReplaceAllItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var index = 0;

            var toRemove = new Dictionary<IReadOnlyList<TKey>, int> { [_items.Select(x => x.Key).ToList()] = index };

            _items.Clear();

            var result = AddItemsWithoutNotify(items, keySelector, valueSelector);

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Replace,
                new Dictionary<IReadOnlyList<TKey>, int> { [result.keysToAdd] = index },
                toRemove,
                result.itemsToAdd.Select(
                    x => new KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>(_items.IndexOf(_items.First(y => y.Key.Equals(x.ItemKey))), NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Dictionary<IReadOnlyList<TValue>, int> { [x.ItemValue.ToList()] = x.Index },
                        default
                ))).ToList()));
        }

        public void ClearGroup(TKey key)
        {
            var item = _items.First(x => x.Key.Equals(key));

            item.Value.Clear();

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                null,
                null,
                null,
                new Collection<KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>>
                {
                    new KeyValuePair<int, NotifyGroupCollectionChangedArgs<TValue>>(_items.IndexOf(item),
                        NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Reset, null, null))
                }));
        }

        #endregion

        //public ICollection<TKey> Keys => _items.Keys;

        //public ICollection<ICollection<TValue>> Values => _items.Values;

        //public ICollection<TValue> this[TKey key]
        //{
        //    get => _items[key];
        //    set => _items[key] = value;
        //}

        //public bool ContainsKey(TKey key)
        //{
        //    return _items.ContainsKey(key);
        //}

        //public bool TryGetValue(TKey key, out ICollection<TValue> value)
        //{
        //    return _items.TryGetValue(key, out value);
        //}

        //public int Count => _items.Count;

        //public bool Contains(KeyValuePair<TKey, ICollection<TValue>> item)
        //{
        //    return _items.Contains(item);
        //}

        //public void CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, int arrayIndex)
        //{
        //    _items.CopyTo(array, arrayIndex);
        //}

        #region IEnumerable

        public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        private void OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> args)
        {
            ItemsChanged?.Invoke(this, args);
        }

        private (IReadOnlyList<TKey> keysToAdd,
            IReadOnlyList<(TKey ItemKey, ICollection<TValue> ItemValue, int Index)> itemsToAdd) AddItemsWithoutNotify<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var keysToAdd = new Collection<TKey>();

            var itemsToAdd = new List<(TKey ItemKey, ICollection<TValue> ItemValue, int Index)>();

            foreach (var item in items)
            {
                var key = keySelector(item);
                var value = valueSelector(item);

                if (itemsToAdd.Any(x => x.ItemKey.Equals(key)))
                {
                    itemsToAdd.First(x => x.ItemKey.Equals(key)).ItemValue.Add(value);
                }
                else
                {
                    int index = 0;

                    if(_items.Any(x => x.Key.Equals(key)))
                    {
                        index = _items.First(x => x.Key.Equals(key)).Value.Count;
                    }

                    itemsToAdd.Add((key, new Collection<TValue> { value }, index));
                }
            }

            foreach (var item in itemsToAdd)
            {
                if (!_items.Any(x => x.Key.Equals(item.ItemKey)))
                {
                    keysToAdd.Add(item.ItemKey);
                    _items.Add(new KeyValuePair<TKey, ICollection<TValue>>(item.ItemKey, item.ItemValue));
                }
                else
                {
                    _items.First(x => x.Key.Equals(item.ItemKey)).Value.AddRange(
                        itemsToAdd.First(x => x.ItemKey.Equals(item.ItemKey)).ItemValue);
                }
            }

            return (keysToAdd, itemsToAdd);
        }
    }
}
