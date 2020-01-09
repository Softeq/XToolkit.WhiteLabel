// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Extensions;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     Represents groups of items that provides notifications when groups or
    ///     it's items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="TKey">The group type of the collection</typeparam>
    /// <typeparam name="TValue">The item type of the collection</typeparam>
    public sealed class ObservableKeyGroupsCollection<TKey, TValue>
        : IObservableKeyGroupsCollection<TKey, TValue>,
            INotifyKeyGroupCollectionChanged<TKey, TValue>,
            INotifyCollectionChanged
    {
        private readonly IList<Group> _items;
        private readonly bool _withoutEmptyGroups;

        public event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IList<TKey> Keys => _items?.Select(item => item.Key).ToList();
        //public IList<IList<TValue>> Values => _items?.Select(item => _items).ToList();

        public int Count => _items.Count;

        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        /// <param name="withoutEmptyGroups">If true empty groups will be removed</param>
        public ObservableKeyGroupsCollection(bool withoutEmptyGroups = true)
        {
            _withoutEmptyGroups = withoutEmptyGroups;
            _items = new List<Group>();
        }

        #region IObservableKeyGroupCollection

        /// <inheritdoc />
        public void AddGroups(IEnumerable<TKey> keys)
        {
            if (_withoutEmptyGroups)
            {
                throw new InvalidOperationException();
            }

            if (keys == null)
            {
                throw new ArgumentNullException();
            }

            AddGroups(keys.Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>())));
        }

        /// <inheritdoc />
        public void AddGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var insertionIndex = _items.Count;

            InsertGroups(insertionIndex, items);
        }

        /// <inheritdoc />
        public void InsertGroups(int index, IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            InsertGroups(index, keys.Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>())));
        }

        /// <inheritdoc />
        public void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var insertedGroups = InsertGroupsWithoutNotify(index, items, _withoutEmptyGroups);
            if (insertedGroups == null)
            {
                return;
            }

            OnChanged(
                NotifyCollectionChangedAction.Add,
                new Collection<(int, IReadOnlyList<TKey>)> { (index, insertedGroups.Select(x => x.Key).ToList()) },
                default,
                default);
        }

        /// <inheritdoc />
        public void ReplaceGroups(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            _items.Clear();

            ReplaceGroups(keys.Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>())));
        }

        /// <inheritdoc />
        public void ReplaceGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var toRemove = _items.Select(x => x.Key).ToList();

            _items.Clear();

            const int insertionIndex = 0;

            var insertedGroups = InsertGroupsWithoutNotify(insertionIndex, items, _withoutEmptyGroups);
            if (insertedGroups == null)
            {
                return;
            }

            OnChanged(
                NotifyCollectionChangedAction.Replace,
                new Collection<(int, IReadOnlyList<TKey>)> { (insertionIndex, insertedGroups.Select(x => x.Key).ToList()) },
                new Collection<(int, IReadOnlyList<TKey>)> { (insertionIndex, toRemove) },
                default);
        }

        /// <inheritdoc />
        public void RemoveGroups(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
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

        /// <inheritdoc />
        public void ClearGroups()
        {
            _items.Clear();

            OnChanged(
                NotifyCollectionChangedAction.Reset,
                default,
                default,
                default);
        }

        /// <inheritdoc />
        public void ClearGroup(TKey key)
        {
            if (_withoutEmptyGroups)
            {
                RemoveGroups(new Collection<TKey> { key });
                return;
            }

            var item = _items.FirstOrDefault(x => x.Key.Equals(key));

            if (item == null)
            {
                throw new KeyNotFoundException($"Can't be found key: {key.ToString()}");
            }

            item.Clear();

            var groupEvents =
                new Collection<(int, NotifyGroupCollectionChangedArgs<TValue>)>
                {
                    (_items.IndexOf(item),
                        NotifyGroupCollectionChangedArgs<TValue>.Create(NotifyCollectionChangedAction.Reset, null, null))
                };

            OnChanged(
                default,
                default,
                default,
                groupEvents);

        }

        /// <inheritdoc />
        public void AddItems<T>(
            IEnumerable<T> items,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector)
        {
            if (items == null || keySelector == null || valueSelector == null)
            {
                throw new ArgumentNullException();
            }

            int insertionIndex = _items.Count;

            var groups = items
                .Select(keySelector.Invoke)
                .Distinct()
                .Where(x => _items.All(y => !y.Key.Equals(x)))
                .Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>()));

            var keysToAdd = InsertGroupsWithoutNotify(insertionIndex, groups, false)?.Select(x => x.Key).ToList();

            var result = InsertItemsWithoutNotify(items, keySelector, valueSelector, null);

            if (!result.Any())
            {
                return;
            }

            var groupEvents = result
                .Where(x => keysToAdd == null ? true : keysToAdd.All(y => !y.Equals(x.Key)))
                .Select(x =>
                    (
                        _items.IndexOf(_items.First(y => y.Key.Equals(x.Key))),
                        NotifyGroupCollectionChangedArgs<TValue>.Create(
                            NotifyCollectionChangedAction.Add,
                            new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                            default
                        )
                    ))
                .ToList();

            groupEvents = groupEvents.Count > 0 ? groupEvents : default;

            OnChanged(
                keysToAdd == null ? default(NotifyCollectionChangedAction?) : NotifyCollectionChangedAction.Add,
                keysToAdd == null ? default : new Collection<(int, IReadOnlyList<TKey>)> { (insertionIndex, keysToAdd) },
                default,
                groupEvents);
        }

        /// <inheritdoc />
        public void InsertItems<T>(
            IEnumerable<T> items,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector,
            Func<T, int> valueIndexSelector)
        {
            if (items == null || keySelector == null || valueSelector == null || valueIndexSelector == null)
            {
                throw new ArgumentNullException();
            }

            var result = InsertItemsWithoutNotify(items, keySelector, valueSelector, valueIndexSelector);

            if (!result.Any())
            {
                return;
            }

            var groupEvents = result
                .Select(x =>
                    (
                        _items.IndexOf(_items.First(y => y.Key.Equals(x.Key))),
                        NotifyGroupCollectionChangedArgs<TValue>.Create(
                            NotifyCollectionChangedAction.Add,
                            new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                            default)
                    ))
                .ToList();

            OnChanged(
                default,
                default,
                default,
                groupEvents);
        }

        /// <inheritdoc />
        public void ReplaceItems<T>(
            IEnumerable<T> items,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector)
        {
            if (items == null || keySelector == null || valueSelector == null)
            {
                throw new ArgumentNullException();
            }

            var toRemove = _items.Select(x => x.Key).ToList();

            _items.Clear();

            const int insertionIndex = 0;

            var groups = items
                .Select(keySelector.Invoke)
                .Distinct()
                .Where(x => _items.All(y => !y.Key.Equals(x)))
                .Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>()));

            var keysToAdd = InsertGroupsWithoutNotify(insertionIndex, groups, false)?.Select(x => x.Key).ToList();

            var result = InsertItemsWithoutNotify(items, keySelector, valueSelector, null);

            if (!result.Any())
            {
                return;
            }

            var groupEvents = result
                .Where(x => keysToAdd == null ? true : keysToAdd.All(y => !y.Equals(x.Key)))
                .Select(x =>
                    (
                        _items.IndexOf(_items.First(y => y.Key.Equals(x.Key))),
                        NotifyGroupCollectionChangedArgs<TValue>.Create(
                            NotifyCollectionChangedAction.Add,
                            new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                            default)
                    ))
                .ToList();

            groupEvents = groupEvents.Count > 0 ? groupEvents : default;

            OnChanged(
                NotifyCollectionChangedAction.Replace,
                new Collection<(int, IReadOnlyList<TKey>)> { (insertionIndex, keysToAdd) },
                new Collection<(int, IReadOnlyList<TKey>)> { (insertionIndex, toRemove) },
                groupEvents);
        }

        /// <inheritdoc />
        public void RemoveItems<T>(
            IEnumerable<T> items,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector)
        {
            if (items == null || keySelector == null || valueSelector == null)
            {
                throw new ArgumentNullException();
            }

            if (!items.Any())
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
                var valIndex = _items.ElementAt(groupIndex).IndexOf(val);

                if (!_items.ElementAt(groupIndex).Any(x => x.Equals(val)))
                {
                    throw new KeyNotFoundException();
                }

                if (groupsInfos.All(x => x.GroupIndex != groupIndex))
                {
                    groupsInfos.Add((groupIndex, new List<KeyValuePair<TValue, int>>()));
                }

                groupsInfos
                    .First(x => x.GroupIndex == groupIndex)
                    .Items
                    .Add(new KeyValuePair<TValue, int>(val, valIndex));
            }

            foreach (var groupInfo in groupsInfos)
            {
                if (!rangesToRemove.ContainsKey(groupInfo.GroupIndex))
                {
                    rangesToRemove.Add(groupInfo.GroupIndex, new List<(int, IReadOnlyList<TValue>)>());
                }
                rangesToRemove[groupInfo.GroupIndex].AddRange(GroupByIndex(groupInfo.Items).ToList());
            }

            foreach (var groupInfo in groupsInfos)
            {
                foreach (var item in groupInfo.Items)
                {
                    _items[groupInfo.GroupIndex].Remove(item.Key);
                }
            }

            if (_withoutEmptyGroups)
            {
                var keysToRemove = _items.Where(x => x.Count == 0).Select(x => x.Key);
                groupsToRemove = RemoveGroupsWithoutNotify(keysToRemove);
            }

            var keyIndexesToRemove = groupsToRemove?
                .Select(x => Enumerable.Range(x.Index, x.NewItems.Count))
                .SelectMany(x => x)
                .ToList();

            var groupEvents = rangesToRemove
                .Where(x => keyIndexesToRemove == null ? true : keyIndexesToRemove.All(y => !y.Equals(x.Key)))
                .Select(x =>
                    (
                        x.Key,
                        NotifyGroupCollectionChangedArgs<TValue>.Create(
                            NotifyCollectionChangedAction.Remove,
                            default,
                            (IReadOnlyList<(int, IReadOnlyList<TValue>)>) x.Value)
                    ))
                .ToList();

            groupEvents = groupEvents.Count > 0 ? groupEvents : default;

            OnChanged(
                groupsToRemove?.Count > 0 ? NotifyCollectionChangedAction.Remove : (NotifyCollectionChangedAction?) null,
                default,
                groupsToRemove?.Count > 0 ? groupsToRemove : null,
                groupEvents);
        }

        #endregion

        #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        private void OnChanged(
            NotifyCollectionChangedAction? action,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)> newItems,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> OldItems)> oldItems,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedArgs<TValue> Arg)> groupEvents)
        {
            RaiseEvents(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>.Create(
                action,
                newItems,
                oldItems,
                groupEvents));
        }

        private void RaiseEvents(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> args)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            ItemsChanged?.Invoke(this, args);
        }

        private IEnumerable<Group> InsertGroupsWithoutNotify(
            int index,
            IEnumerable<KeyValuePair<TKey, IList<TValue>>> items,
            bool withoutEmptyGroups)
        {
            if (!items.Any())
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

            var toInsert = items
                .Where(x => !withoutEmptyGroups || x.Value.Count > 0)
                .Select(x => new Group(x))
                .ToList();

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
            InsertItemsWithoutNotify<T>(
                IEnumerable<T> items,
                Func<T, TKey> keySelector,
                Func<T, TValue> valueSelector,
                Func<T, int> indexSelector)
        {
            var groupedItemsToAdd = new List<(TKey Key, IEnumerable<(int Index, IReadOnlyList<TValue> Values)> Ranges)>();

            var itemsToAdd = new List<(TKey Key, IList<KeyValuePair<TValue, int>> Values)>();

            foreach (var item in items)
            {
                var key = keySelector.Invoke(item);

                if (!_items.Any(x => x.Key.Equals(key)))
                {
                    throw new KeyNotFoundException($"Can't be found key: {key.ToString()}");
                }

                var val = valueSelector.Invoke(item);

                int index;

                if (indexSelector == null)
                {
                    index = _items.First(x => x.Key.Equals(key)).Count;
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

            foreach (var (key, values) in itemsToAdd)
            {
                groupedItemsToAdd.Add((key, GroupByIndex(values)));
            }

            foreach (var (key, ranges) in groupedItemsToAdd)
            {
                foreach (var (index, values) in ranges)
                {
                    _items
                        .First(x => x.Key.Equals(key))
                        .InsertRange(index, values.ToList());
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

            foreach (var key in keys.ToList())
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

            return sortedItems
                .GroupBy(x => x.Value)
                .Select(x => (x.Key, (IReadOnlyList<T>) x.Select(y => y.Key).ToList()))
                .ToList();
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

        private class Group : List<TValue>, IGrouping<TKey, TValue>
        {
            public Group(KeyValuePair<TKey, IList<TValue>> keyValuePair)
            {
                Key = keyValuePair.Key;
                AddRange(keyValuePair.Value);
            }

            public TKey Key { get; }
        }
    }
}
