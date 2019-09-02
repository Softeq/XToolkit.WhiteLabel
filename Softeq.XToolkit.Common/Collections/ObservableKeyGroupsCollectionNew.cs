﻿// Developed by Softeq Development Corporation
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
        private readonly IList<KeyValuePair<TKey, IList<TValue>>> _items;
        private readonly bool _withoutEmptyGroups;

        public event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;

        public ObservableKeyGroupsCollectionNew(bool withoutEmptyGroups)
        {
            _withoutEmptyGroups = withoutEmptyGroups;
            _items = new List<KeyValuePair<TKey, IList<TValue>>>();
        }

        #region IObservableKeyGroupCollection

        public void AddGroups(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException();
            }

            AddGroups(keys.Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue> { })));
        }

        public void AddGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }

            var index = _items.Count;

            InsertGroups(index, items);
        }

        public void InsertGroups(int index, IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException();
            }

            InsertGroups(index, keys.Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue> { })));
        }

        public void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }

            var insertedGroups = InsertGroupsWithoutNotify(index, items, _withoutEmptyGroups);
            if(insertedGroups == null)
            {
                return;
            }

            OnChanged(
                NotifyCollectionChangedAction.Add,
                new Collection<(int, IReadOnlyList<TKey>)> { (index, insertedGroups.Select(x => x.Key).ToList()) },
                default,
                insertedGroups.Select(x => (_items.ToList().IndexOf(x),
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Collection<(int, IReadOnlyList<TValue>)> { (0, x.Value.ToList()) },
                    default))).ToList());
        }

        public void ReplaceGroups(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException();
            }

            _items.Clear();

            ReplaceGroups(keys.Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue> { })));
        }

        public void ReplaceGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }

            var toRemove = _items.Select(x => x.Key).ToList();

            _items.Clear();

            int index = 0;

            var insertedGroups = InsertGroupsWithoutNotify(index, items, _withoutEmptyGroups);
            if (insertedGroups == null)
            {
                return;
            }

            OnChanged(
                NotifyCollectionChangedAction.Replace,
                new Collection<(int, IReadOnlyList<TKey>)> { (index, insertedGroups.Select(x => x.Key).ToList()) },
                new Collection<(int, IReadOnlyList<TKey>)> { (index, toRemove) },
                insertedGroups.Select(x => (_items.ToList().IndexOf(x),
                    NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Add,
                    new Collection<(int, IReadOnlyList<TValue>)> { (0, x.Value.ToList()) },
                    default))).ToList());
        }

        public void RemoveGroups(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException();
            }

            var oldItemsRange = RemoveGroupsWithoutNotify(keys);

            if (oldItemsRange.Count == 0)
            {
                return;
            }

            OnChanged(
                NotifyCollectionChangedAction.Remove,
                default,
                oldItemsRange,
                default);
        }

        public void ClearGroups()
        {
            _items.Clear();

            OnChanged(
                NotifyCollectionChangedAction.Reset,
                default,
                default,
                default);
        }

        public void ClearGroup(TKey key)
        {
            if (_withoutEmptyGroups)
            {
                RemoveGroups(new Collection<TKey> { key });
                return;
            }

            if (!_items.Any(x => x.Key.Equals(key)))
            {
                throw new KeyNotFoundException();
            }

            var item = _items.FirstOrDefault(x => x.Key.Equals(key));

            item.Value.Clear();

            OnChanged(
                default,
                default,
                default,
                new Collection<(int, NotifyGroupCollectionChangedArgs<TValue>)>
                {
                    (_items.IndexOf(item),
                        NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Reset, null, null))
                });

        }

        public void AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            if (items == null || keySelector == null || valueSelector == null)
            {
                throw new ArgumentNullException();
            }

            int index = _items.Count;

            var keysToAdd = InsertGroupsWithoutNotify(index, items
                .Select(x => keySelector.Invoke(x))
                .Distinct()
                .Where(x => _items.All(y => !y.Key.Equals(x)))
                .Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue> { })), false)
                ?.Select(x => x.Key).ToList();

            var result = InsertItemsWithoutNotify(items, keySelector, valueSelector, null);

            if (result.Count() == 0)
            {
                return;
            }

            OnChanged(
                keysToAdd == null ? default(NotifyCollectionChangedAction?) : NotifyCollectionChangedAction.Add,
                keysToAdd == null ? default : new Collection<(int, IReadOnlyList<TKey>)> { (index, keysToAdd) },
                default,
                result.Select(
                    x => (_items.IndexOf(_items.First(y => y.Key.Equals(x.Key))), NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                        default
                ))).ToList());
        }

        public void InsertItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector)
        {
            if (items == null || keySelector == null || valueSelector == null || valueIndexSelector == null)
            {
                throw new ArgumentNullException();
            }

            var result = InsertItemsWithoutNotify(items, keySelector, valueSelector, valueIndexSelector);

            if (result.Count() == 0)
            {
                return;
            }

            OnChanged(
                default,
                default,
                default,
                result.Select(
                    x => (_items.IndexOf(_items.First(y => y.Key.Equals(x.Key))), NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                        default
                ))).ToList());
        }

        public void ReplaceItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            if (items == null || keySelector == null || valueSelector == null)
            {
                throw new ArgumentNullException();
            }

            var toRemove = _items.Select(x => x.Key).ToList();

            _items.Clear();

            int index = 0;

            var keysToAdd = InsertGroupsWithoutNotify(index, items
                .Select(x => keySelector.Invoke(x))
                .Distinct()
                .Where(x => _items.All(y => !y.Key.Equals(x)))
                .Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue> { })), false)
                ?.Select(x => x.Key).ToList();

            var result = InsertItemsWithoutNotify(items, keySelector, valueSelector, null);

            if (result.Count() == 0)
            {
                return;
            }

            OnChanged(
                NotifyCollectionChangedAction.Replace,
                new Collection<(int, IReadOnlyList<TKey>)> { (index, keysToAdd) },
                new Collection<(int, IReadOnlyList<TKey>)> { (index, toRemove) },
                result.Select(
                    x => (_items.IndexOf(_items.First(y => y.Key.Equals(x.Key))), NotifyGroupCollectionChangedArgs<TValue>.Create(
                        NotifyCollectionChangedAction.Add,
                        new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                        default
                ))).ToList());
        }

        public void RemoveItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            if (items == null || keySelector == null || valueSelector == null)
            {
                throw new ArgumentNullException();
            }

            if (items.Count() == 0)
            {
                return;
            }

            var rangesToRemove = new Dictionary<int, IList<(int ValIndex, IReadOnlyList<TValue> Vals)>>();
            var groupsInfos = new List<(int GroupIndex, List<KeyValuePair<TValue, int>> Items)>();
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> groupsToRemove = null;

            foreach (var item in items)
            {
                var key = keySelector(item);
                if (!_items.Any(x => x.Key.Equals(key)))
                {
                    throw new KeyNotFoundException();
                }

                var groupIndex = _items.IndexOf(_items.First(x => x.Key.Equals(key)));
                var val = valueSelector(item);
                var valIndex = _items.ElementAt(groupIndex)
                    .Value.ToList().IndexOf(val);

                if (!_items.ElementAt(groupIndex).Value.Any(x => x.Equals(val)))
                {
                    throw new KeyNotFoundException();
                }

                if (!groupsInfos.Any(x => x.GroupIndex == groupIndex))
                {
                    groupsInfos.Add((groupIndex, new List<KeyValuePair<TValue, int>> { }));
                }

                groupsInfos.First(x => x.GroupIndex == groupIndex)
                    .Items.Add(new KeyValuePair<TValue, int>(val, valIndex));
            }

            foreach(var groupInfo in groupsInfos)
            {
                if (!rangesToRemove.ContainsKey(groupInfo.GroupIndex))
                {
                    rangesToRemove.Add(groupInfo.GroupIndex, new List<(int, IReadOnlyList<TValue>)> { });
                }
                rangesToRemove[groupInfo.GroupIndex].AddRange(GroupByIndex(groupInfo.Items).ToList());
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

            OnChanged(
                groupsToRemove?.Count > 0 ? NotifyCollectionChangedAction.Remove : (NotifyCollectionChangedAction?) null,
                default,
                groupsToRemove?.Count > 0 ? groupsToRemove : null,
                rangesToRemove.Select(x => (x.Key, NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Remove,
                    default,
                    (IReadOnlyList<(int, IReadOnlyList<TValue>)>) x.Value))).ToList());
        }

        #endregion

        #region IEnumerable

        public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        private void OnChanged(
            NotifyCollectionChangedAction? action,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> newItems,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> oldItems,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedArgs<TValue> Arg)> groupEvents)
        {
            ItemsChanged?.Invoke(this, NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                action,
                newItems,
                oldItems,
                groupEvents));
        }

        private IEnumerable<KeyValuePair<TKey, IList<TValue>>> InsertGroupsWithoutNotify
            (int index, IEnumerable<KeyValuePair<TKey, IList<TValue>>> items, bool withoutEmptryGroups)
        {
            if (items.Count() == 0)
            {
                return null;
            }

            if (items.Any(x => x.Value == null))
            {
                throw new ArgumentNullException();
            }

            if (index > _items.Count + items.Count() - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            var toInsert = items.Where(x => withoutEmptryGroups ? x.Value.Count > 0 : true).ToList();

            if (toInsert.Count == 0)
            {
                return null;
            }

            int i = index;

            foreach (var item in toInsert)
            {
                _items.Insert(i++, item);
            }

            return toInsert;
        }

        private IEnumerable<(TKey Key, IEnumerable<(int Index, IReadOnlyList<TValue> Values)> ValuesGroups)>
            InsertItemsWithoutNotify<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, Func<T, int> indexSelector)
        {
            var groupedItemsToAdd = new List<(TKey Key, IEnumerable<(int Index, IReadOnlyList<TValue> Values)> Ranges)>();

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
                groupedItemsToAdd.Add((item.Key, GroupByIndex(item.Values)));
            }

            foreach (var gr in groupedItemsToAdd)
            {
                foreach (var val in gr.Ranges)
                {
                    _items.First(x => x.Key.Equals(gr.Key))
                        .Value.InsertRange(val.Index, val.Values.ToList());
                }
            }

            return groupedItemsToAdd;
        }

        private IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> RemoveGroupsWithoutNotify(IEnumerable<TKey> keys)
        {
            if (keys.Any(key => _items.All(item => !item.Key.Equals(key))))
            {
                throw new KeyNotFoundException();
            }

            var indexes = new List<KeyValuePair<TKey, int>>();

            for (int i = 0; i < _items.Count; i++)
            {
                if (keys.Any(x => x.Equals(_items[i].Key)))
                {
                    indexes.Add(new KeyValuePair<TKey, int>(_items[i].Key, i));
                }
            }

            foreach (var key in keys)
            {
                _items.Remove(_items.First(x => x.Key.Equals(key)));
            }

            return GroupByIndex(indexes);
        }

        private IReadOnlyList<(int Index, IReadOnlyList<T> NewItems)> GroupByIndex<T>(IList<KeyValuePair<T, int>> items)
        {
            var sortedItems = items.OrderBy(x => x.Value).ToList();

            while (!IsIndexesGrouped(sortedItems))
            {
                DecrementIndex(sortedItems);
            }

            return sortedItems.GroupBy(x => x.Value)
                    .Select(x => (x.Key, (IReadOnlyList<T>) x.Select(y => y.Key).ToList())).ToList();
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
