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
    public sealed class ObservableKeyGroupsCollectionNew<TKey, TValue> : IObservableKeyGroupCollection<TKey, TValue>,
        INotifyKeyGroupCollectionChanged<TKey, TValue>
    {
        private readonly IDictionary<TKey, ICollection<TValue>> _items;

        public event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;

        public ObservableKeyGroupsCollectionNew()
        {
            _items = new Dictionary<TKey, ICollection<TValue>>();
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
                _items.Add(item.Key, item.Value);
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Replace,
                new Dictionary<IReadOnlyList<TKey>, int> { [items.Select(x => x.Key).ToList()] = index },
                toRemove,
                items.Select(x => new KeyValuePair<TKey, NotifyGroupCollectionChangedArgs<TValue>>(x.Key,
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Dictionary<IReadOnlyList<TValue>, int>
                    {
                        [x.Value.ToList()] = 0
                    },
                    default))).ToList()));
        }

        public void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items)
        {
            foreach (var item in items)
            {
                _items.Add(item.Key, item.Value);
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Add,
                new Dictionary<IReadOnlyList<TKey>, int> { [items.Select(x => x.Key).ToList()] = index },
                default,
                items.Select(x => new KeyValuePair<TKey, NotifyGroupCollectionChangedArgs<TValue>>(x.Key,
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Dictionary<IReadOnlyList<TValue>, int>
                    {
                        [x.Value.ToList()] = 0
                    },
                    default))).ToList()));
        }

        public void RemoveGroups(IEnumerable<TKey> keys)
        {
            var indexes = keys.ToDictionary(x => x, x => _items.Keys.ToList().IndexOf(x));

            foreach (var item in keys)
            {
                _items.Remove(item);
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Remove,
                default,
                keys.ToDictionary(x => (IReadOnlyList<TKey>) (new Collection<TKey> { x }), x => indexes[x]),
                default));
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
                    x => new KeyValuePair<TKey, NotifyGroupCollectionChangedArgs<TValue>>(x.Key, NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Dictionary<IReadOnlyList<TValue>, int> { [x.Value.ToList()] = result.indexes[x.Key] },
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
                    x => new KeyValuePair<TKey, NotifyGroupCollectionChangedArgs<TValue>>(x.Key, NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Dictionary<IReadOnlyList<TValue>, int> { [x.Value.ToList()] = result.indexes[x.Key] },
                        default
                ))).ToList()));
        }

        public void ClearGroup(TKey key)
        {
            _items[key].Clear()
                ;

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                null,
                null,
                null,
                new Collection<KeyValuePair<TKey, NotifyGroupCollectionChangedArgs<TValue>>>
                {
                    new KeyValuePair<TKey, NotifyGroupCollectionChangedArgs<TValue>>(key,
                        NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Reset, null, null))
                }));
        }

        public void RemoveItems(IEnumerable<TValue> values)
        {
            foreach(var val in values)
            {
                var item = _items.FirstOrDefault(x => x.Value.Contains(val));
                if(!item.Equals(default))
                {
                    item.Value.Remove(val);
                }
            }
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
            return GetEnumerator();
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

        private (
            IReadOnlyDictionary<TKey, int> indexes,
            IReadOnlyList<TKey> keysToAdd,
            IReadOnlyDictionary<TKey, IList<TValue>> itemsToAdd) AddItemsWithoutNotify<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var keysToAdd = new Collection<TKey>();

            var indexes = new Dictionary<TKey, int>();
            var itemsToAdd = new Dictionary<TKey, IList<TValue>>();

            foreach (var item in items)
            {
                var key = keySelector(item);
                var value = valueSelector(item);

                if (itemsToAdd.Keys.Contains(key))
                {
                    itemsToAdd[key].Add(value);
                }
                else
                {
                    itemsToAdd.Add(key, new Collection<TValue> { value });
                }
            }

            foreach (var key in itemsToAdd.Keys)
            {
                if (!_items.Keys.Contains(key))
                {
                    keysToAdd.Add(key);
                    AddGroups(new Collection<KeyValuePair<TKey, ICollection<TValue>>>
                    {
                        new KeyValuePair<TKey, ICollection<TValue>>(key, itemsToAdd[key])
                    });
                }
                else
                {
                    _items[key].AddRange(itemsToAdd[key]);
                }
            }

            return (indexes, keysToAdd, itemsToAdd);
        }
    }
}
