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

        public void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items)
        {
            int i = index;

            foreach (var item in items)
            {
                _items.Insert(i++, item);
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Add,
                new Collection<(int, IReadOnlyList<TKey>)> { (index, items.Select(x => x.Key).ToList()) },
                default,
                items.Select(x => (_items.ToList().IndexOf(x),
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Collection<(int, IReadOnlyList<TValue>)> { (0, x.Value.ToList()) },
                    default))).ToList()));
        }

        public void RemoveGroups(IEnumerable<TKey> keys)
        {
            var indexes = new List<KeyValuePair<TKey, int>>();

            for (int i = 0; i < _items.Count; i++)
            {
                if (keys.Any(x => x.Equals(_items[i].Key)))
                {
                    indexes.Add(new KeyValuePair<TKey, int>(_items[i].Key, i));
                }
            }

            indexes = indexes.OrderBy(x => x.Value).ToList();

            while (!IsIndexesGrouped(indexes))
            {
                DecrementIndex(indexes);
            }

            var oldItemsRange = indexes.GroupBy(x => x.Value)
                    .Select(x => (x.Key, (IReadOnlyList<TKey>) x.Select(y => y.Key).ToList())).ToList();

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Remove,
                default,
                oldItemsRange,
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
            var result = AddItemsWithoutNotify(items, keySelector, valueSelector, null);

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                default,
                default,
                default,
                result.Select(
                    x => (_items.IndexOf(_items.First(y => y.Key.Equals(x.Key))), NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Collection<(int, IReadOnlyList<TValue>)> (x.Item2.ToList()),
                        default
                ))).ToList()));
        }

        public void InsertItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector)
        {
            var result = AddItemsWithoutNotify(items, keySelector, valueSelector, valueIndexSelector);

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                default,
                default,
                default,
                result.Select(
                    x => (_items.IndexOf(_items.First(y => y.Key.Equals(x.Key))), NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Collection<(int, IReadOnlyList<TValue>)>(x.Item2.ToList()),
                        default
                ))).ToList()));
        }

        public void RemoveItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var groupedIndexes = new Dictionary<int, IList<(int ValIndex, IReadOnlyList<TValue> Vals)>>();
            var groupsInfos = new List<(int GroupIndex, List<KeyValuePair<TValue, int>> Items)>();

            foreach (var item in items)
            {
                var key = keySelector(item);
                var val = valueSelector(item);
                var groupIndex = _items.IndexOf(_items.First(x => x.Key.Equals(key)));
                var valIndex = _items.ElementAt(groupIndex)
                    .Value.ToList().IndexOf(val);

                if (!groupsInfos.Any(x => x.GroupIndex == groupIndex))
                {
                    groupsInfos.Add((groupIndex, new List<KeyValuePair<TValue, int>> { }));
                }

                groupsInfos.First(x => x.GroupIndex == groupIndex)
                    .Items.Add(new KeyValuePair<TValue, int>(val, valIndex));
            }

            for (int i = 0; i < groupsInfos.Count; i++)
            {
                groupsInfos[i] = (groupsInfos[i].GroupIndex, groupsInfos[i].Items.OrderBy(x => x.Value).ToList());
            }

            foreach (var groupInfo in groupsInfos)
            {
                while (!IsIndexesGrouped(groupInfo.Items))
                {
                    DecrementIndex(groupInfo.Items);
                }

                if (!groupedIndexes.ContainsKey(groupInfo.GroupIndex))
                {
                    groupedIndexes.Add(groupInfo.GroupIndex, new List<(int, IReadOnlyList<TValue>)> { });
                }

                var oldItemsRange = groupInfo.Items.GroupBy(x => x.Value)
                        .Select(x => (x.Key, (IReadOnlyList<TValue>) x.Select(y => y.Key).ToList())).ToList();

                groupedIndexes[groupInfo.GroupIndex].AddRange(oldItemsRange);
            }

            foreach (var groupInfo in groupsInfos)
            {
                foreach (var item in groupInfo.Items)
                {
                    _items[groupInfo.GroupIndex].Value.Remove(item.Key);
                }
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                default,
                default,
                default,
                groupedIndexes.Select(x => (x.Key, NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Remove,
                    default,
                    (IReadOnlyList<(int, IReadOnlyList<TValue>)>) x.Value))).ToList()));
        }

        public void ClearGroup(TKey key)
        {
            var item = _items.First(x => x.Key.Equals(key));

            item.Value.Clear();

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                null,
                null,
                null,
                new Collection<(int, NotifyGroupCollectionChangedArgs<TValue>)>
                {
                    (_items.IndexOf(item),
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

        //private IReadOnlyList<(TKey ItemKey, ICollection<TValue> ItemValue, int Index)>
        //        AddItemsWithoutNotify<T>(IEnumerable<T> items,
        //            Func<T, TKey> keySelector, Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector)
        //{
        //    var keysToAdd = new Collection<TKey>();

        //    var itemsToAdd = new List<(TKey ItemKey, ICollection<TValue> ItemValue, int Index)>();

        //    foreach (var item in items)
        //    {
        //        var key = keySelector(item);
        //        var value = valueSelector(item);

        //        if (!_items.Any(x => x.Key.Equals(key)))
        //        {
        //            throw new KeyNotFoundException();
        //        }

        //        if (itemsToAdd.Any(x => x.ItemKey.Equals(key)))
        //        {
        //            itemsToAdd.First(x => x.ItemKey.Equals(key)).ItemValue.Add(value);
        //        }
        //        else
        //        {
        //            int index = valueIndexSelector.Invoke(item);

        //            if (_items.Any(x => x.Key.Equals(key)))
        //            {
        //                index = _items.First(x => x.Key.Equals(key)).Value.Count;
        //            }

        //            itemsToAdd.Add((key, new Collection<TValue> { value }, index));
        //        }
        //    }

        //    foreach (var item in itemsToAdd)
        //    {
        //        _items.First(x => x.Key.Equals(item.ItemKey)).Value.AddRange(
        //            itemsToAdd.First(x => x.ItemKey.Equals(item.ItemKey)).ItemValue);
        //    }

        //    return itemsToAdd;
        //}

        private IEnumerable<(TKey Key, IEnumerable<(int Index, IReadOnlyList<TValue> Values)>)>
            AddItemsWithoutNotify<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, Func<T, int> indexSelector)
        {
            var groupedItemsToAdd = new List<(TKey Key, IEnumerable<(int Index, IReadOnlyList<TValue>)>)>();

            var itemsToAdd = new List<(TKey Key, IList<KeyValuePair<TValue, int>> Values)>();

            foreach (var item in items)
            {
                var key = keySelector.Invoke(item);

                if (!_items.Any(x => x.Key.Equals(key)))
                {
                    throw new KeyNotFoundException();
                }

                var val = valueSelector.Invoke(item);

                int index;

                if (indexSelector == null)
                {
                    index = _items.First(x => x.Key.Equals(key)).Value.Count;
                }
                else
                {
                    index = indexSelector.Invoke(item);
                }

                if (!itemsToAdd.Any(x => x.Key.Equals(key)))
                {
                    itemsToAdd.Add((key, new List<KeyValuePair<TValue, int>>()));
                }

                itemsToAdd.First(x => x.Key.Equals(key)).Values.Add(new KeyValuePair<TValue, int>(val, index));
            }

            foreach (var item in itemsToAdd)
            {
                while (!IsIndexesGrouped(item.Values))
                {
                    DecrementIndex(item.Values);
                }
            }

            foreach (var item in itemsToAdd)
            {
                var t = item.Values.GroupBy(x => x.Value).Select(x => (x.Key, (IReadOnlyList<TValue>)
                    x.Select(y => y.Key).ToList()));
                groupedItemsToAdd.Add((item.Key, t));
            }

            return groupedItemsToAdd;
        }

        bool IsIndexesGrouped<T>(IList<KeyValuePair<T, int>> indexList)
        {
            for (var i = 0; i < indexList.Count - 1; i++)
            {
                if (indexList[i].Value == indexList[i + 1].Value - 1)
                {
                    return false;
                }
            }

            return true;
        }

        void DecrementIndex<T>(IList<KeyValuePair<T, int>> indexes)
        {
            for (int i = indexes.Count - 1; i > 0; i--)
            {
                if (indexes[i].Value - 1 == indexes[i - 1].Value)
                {
                    indexes[i] = new KeyValuePair<T, int>(indexes[i].Key, indexes[i].Value - 1);
                    return;
                }
            }
        }
    }
}
