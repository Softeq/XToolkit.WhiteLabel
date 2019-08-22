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
        private readonly bool _withoutEmptyGroups;

        public event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;

        public ObservableKeyGroupsCollectionNew(bool withoutEmptyGroups)
        {
            _withoutEmptyGroups = withoutEmptyGroups;
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

            var toInsert = items.Where(x => _withoutEmptyGroups ? x.Value.Count > 0 : true);

            if (toInsert.Count() == 0)
            {
                return;
            }

            foreach (var item in toInsert)
            {
                _items.Insert(i++, item);
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                NotifyCollectionChangedAction.Add,
                new Collection<(int, IReadOnlyList<TKey>)> { (index, toInsert.Select(x => x.Key).ToList()) },
                default,
                toInsert.Select(x => (_items.ToList().IndexOf(x),
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Collection<(int, IReadOnlyList<TValue>)> { (0, x.Value.ToList()) },
                    default))).ToList()));
        }

        public void RemoveGroups(IEnumerable<TKey> keys)
        {
            var oldItemsRange = RemoveGroupsWithoutNotify(keys);

            if (oldItemsRange.Count == 0)
            {
                return;
            }

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

            if (result.Count() == 0)
            {
                return;
            }

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

        public void InsertItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector)
        {
            var result = AddItemsWithoutNotify(items, keySelector, valueSelector, valueIndexSelector);

            if (result.Count() == 0)
            {
                return;
            }

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
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> groupsToRemove = null;

            if(items.Count() == 0)
            {
                return;
            }

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

            if (_withoutEmptyGroups)
            {
                groupsToRemove = RemoveGroupsWithoutNotify(_items.Where(x => x.Value.Count == 0)
                    .Select(x => x.Key));
            }

            OnChanged(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                groupsToRemove?.Count > 0 ? NotifyCollectionChangedAction.Remove : (NotifyCollectionChangedAction?)null,
                default,
                groupsToRemove?.Count > 0 ? groupsToRemove : null,
                groupedIndexes.Select(x => (x.Key, NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Remove,
                    default,
                    (IReadOnlyList<(int, IReadOnlyList<TValue>)>) x.Value))).ToList()));
        }

        public void ClearGroup(TKey key)
        {
            if (_withoutEmptyGroups)
            {
                RemoveGroups(new Collection<TKey> { key });
            }
            else
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
        }

        #endregion

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

        private IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> RemoveGroupsWithoutNotify(IEnumerable<TKey> keys)
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

            return indexes.GroupBy(x => x.Value)
                    .Select(x => (x.Key, (IReadOnlyList<TKey>) x.Select(y => y.Key).ToList())).ToList();
        }

        private bool IsIndexesGrouped<T>(IList<KeyValuePair<T, int>> indexList)
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

        private void DecrementIndex<T>(IList<KeyValuePair<T, int>> indexes)
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
