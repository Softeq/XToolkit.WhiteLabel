// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Softeq.XToolkit.Common.Extensions;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     Represents groups of items that provides notifications when groups or
    ///     their items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="TKey">The group type of the collection.</typeparam>
    /// <typeparam name="TValue">The item type of the collection.</typeparam>
    public sealed class ObservableKeyGroupsCollection<TKey, TValue>
        : IObservableKeyGroupsCollection<TKey, TValue>,
            INotifyKeyGroupCollectionChanged<TKey, TValue>,
            INotifyCollectionChanged,
            INotifyPropertyChanged
        where TKey : notnull
        where TValue : notnull
    {
        private const string UnsupportEmptyGroupExceptionMessage = "Empty group isn't supported";

        private readonly IList<Group> _groups;
        private readonly bool _emptyGroupsDisabled;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableKeyGroupsCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="allowEmptyGroups">If <see langword="false"/> empty groups will be removed.</param>
        public ObservableKeyGroupsCollection(bool allowEmptyGroups = true)
        {
            _emptyGroupsDisabled = !allowEmptyGroups;
            _groups = new List<Group>();
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>>? ItemsChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public IList<TKey> Keys => _groups.Select(item => item.Key).ToList();

        public int Count => _groups.Count;

        /// <inheritdoc />
        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() => _groups.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => _groups.GetEnumerator();

        /// <inheritdoc />
        public void AddGroups(IEnumerable<TKey> keys)
        {
            InsertGroups(_groups.Count, keys);
        }

        /// <inheritdoc />
        public void AddGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            InsertGroups(_groups.Count, items);
        }

        /// <inheritdoc />
        public void InsertGroups(int index, IEnumerable<TKey> keys)
        {
            if (_emptyGroupsDisabled)
            {
                throw new InvalidOperationException(UnsupportEmptyGroupExceptionMessage);
            }

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

            if (items.Any(x => x.Key == null))
            {
                throw new ArgumentNullException(nameof(items), "One of the keys is null.");
            }

            if (_emptyGroupsDisabled && items.Any(x => x.Value?.Count == 0))
            {
                throw new InvalidOperationException(UnsupportEmptyGroupExceptionMessage);
            }

            var insertedGroups = InsertGroupsWithoutNotify(index, items, _emptyGroupsDisabled);
            if (insertedGroups == null)
            {
                return;
            }

            var newItems = new Collection<(int, IReadOnlyList<TKey>)>
            {
                (index, insertedGroups.Select(x => x.Key).ToList())
            };

            var itemsEvents = insertedGroups
                .Select(x => (
                    _groups.IndexOf(x),
                    new NotifyGroupCollectionChangedEventArgs<TValue>(
                        NotifyCollectionChangedAction.Add, new Collection<(int, IReadOnlyList<TValue>)>(), default)))
             .ToList();

            OnChanged(
                NotifyCollectionChangedAction.Add,
                newItems,
                default,
                itemsEvents);
        }

        /// <inheritdoc />
        public void ReplaceAllGroups(IEnumerable<TKey> keys)
        {
            if (_emptyGroupsDisabled)
            {
                throw new InvalidOperationException(UnsupportEmptyGroupExceptionMessage);
            }

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            ReplaceAllGroups(keys.Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>())));
        }

        /// <inheritdoc />
        public void ReplaceAllGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (_emptyGroupsDisabled && items.Any(x => x.Value?.Count == 0))
            {
                throw new InvalidOperationException(UnsupportEmptyGroupExceptionMessage);
            }

            var oldItems = new Collection<(int, IReadOnlyList<TKey>)>
            {
                (0, _groups.Select(x => x.Key).ToList())
            };

            _groups.Clear();

            var insertedGroups = InsertGroupsWithoutNotify(0, items, _emptyGroupsDisabled);
            var insertedGroupKeys = insertedGroups?.Select(x => x.Key).ToList() ?? new List<TKey>();

            var newItems = new Collection<(int, IReadOnlyList<TKey>)>
            {
                (0, insertedGroupKeys)
            };

            OnChanged(
                NotifyCollectionChangedAction.Replace,
                newItems,
                oldItems,
                default);
        }

        /// <inheritdoc />
        public void RemoveGroups(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var removedGroups = RemoveGroupsWithoutNotify(keys);
            if (removedGroups.Count == 0)
            {
                return;
            }

            var oldItems = new Collection<(int, IReadOnlyList<TKey>)>
            {
                (0, removedGroups[0].Keys)
            };

            OnChanged(
                NotifyCollectionChangedAction.Remove,
                default,
                oldItems,
                default);
        }

        /// <inheritdoc />
        public void Clear()
        {
            if (_groups.Count == 0)
            {
                return;
            }

            _groups.Clear();

            OnChanged(
                NotifyCollectionChangedAction.Reset,
                default,
                default,
                default);
        }

        /// <inheritdoc />
        public void ClearGroup(TKey key)
        {
            if (_emptyGroupsDisabled)
            {
                throw new InvalidOperationException($"{UnsupportEmptyGroupExceptionMessage}. Group with key '{key}' can't be clear.");
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var item = _groups.FirstOrDefault(x => x.Key.Equals(key));
            if (item == null)
            {
                throw new KeyNotFoundException();
            }

            item.Clear();

            var clearGroupEvent =
                new Collection<(int, NotifyGroupCollectionChangedEventArgs<TValue>)>
                {
                    (_groups.IndexOf(item),
                        new NotifyGroupCollectionChangedEventArgs<TValue>(NotifyCollectionChangedAction.Reset, default, default))
                };

            OnChanged(
                default,
                default,
                default,
                clearGroupEvent);
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

            if (!items.Any())
            {
                return;
            }

            int insertionIndex = _groups.Count;

            var groups = items
                .Select(keySelector.Invoke)
                .Distinct()
                .Where(x => _groups.All(y => !y.Key.Equals(x)))
                .Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>()));

            var keysOfAddedGroups = InsertGroupsWithoutNotify(insertionIndex, groups, false)?.Select(x => x.Key).ToList();
            var insertedItems = InsertItemsWithoutNotify(items, keySelector, valueSelector, null);

            if (!insertedItems.Any())
            {
                return;
            }

            var addedItemsEvents = insertedItems
                .Where(x => keysOfAddedGroups == null || keysOfAddedGroups.All(y => !y.Equals(x.Key)))
                .Select(x => (
                    _groups.IndexOf(_groups.First(y => y.Key.Equals(x.Key))),
                    new NotifyGroupCollectionChangedEventArgs<TValue>(
                            NotifyCollectionChangedAction.Add,
                            new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                            default)))
                .ToList();

            addedItemsEvents = addedItemsEvents.Count > 0 ? addedItemsEvents : default;

            OnChanged(
                keysOfAddedGroups == null ? (NotifyCollectionChangedAction?) null : NotifyCollectionChangedAction.Add,
                keysOfAddedGroups == null ? default : new Collection<(int, IReadOnlyList<TKey>)> { (insertionIndex, keysOfAddedGroups) },
                default,
                addedItemsEvents);
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

            var insertedItems = InsertItemsWithoutNotify(items, keySelector, valueSelector, valueIndexSelector);

            if (!insertedItems.Any())
            {
                return;
            }

            var insertedItemsEvents = insertedItems
                .Select(x => (
                    _groups.IndexOf(_groups.First(y => y.Key.Equals(x.Key))),
                    new NotifyGroupCollectionChangedEventArgs<TValue>(
                        NotifyCollectionChangedAction.Add,
                        new Collection<(int, IReadOnlyList<TValue>)>(x.ValuesGroups.ToList()),
                        default)))
                .ToList();

            OnChanged(
                default,
                default,
                default,
                insertedItemsEvents);
        }

        /// <inheritdoc />
        public void ReplaceAllItems<T>(
            IEnumerable<T> items,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector)
        {
            if (items == null || keySelector == null || valueSelector == null)
            {
                throw new ArgumentNullException();
            }

            var keysToRemove = _groups.Select(x => x.Key).ToList();

            _groups.Clear();

            var groups = items
                .Select(keySelector.Invoke)
                .Distinct()
                .Where(x => _groups.All(y => !y.Key.Equals(x)))
                .Select(x => new KeyValuePair<TKey, IList<TValue>>(x, new List<TValue>()));

            var keysOfAddedGroup = InsertGroupsWithoutNotify(0, groups, false)?.Select(x => x.Key)?.ToList() ?? new List<TKey>();

            InsertItemsWithoutNotify(items, keySelector, valueSelector, null);

            OnChanged(
                NotifyCollectionChangedAction.Replace,
                new Collection<(int, IReadOnlyList<TKey>)> { (0, keysOfAddedGroup) },
                new Collection<(int, IReadOnlyList<TKey>)> { (0, keysToRemove) },
                default);
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
            IReadOnlyList<(int Index, IReadOnlyList<TKey> Keys)>? groupsToRemove = null;

            foreach (var item in items)
            {
                if (item == null)
                {
                    throw new ArgumentNullException();
                }

                var key = keySelector.Invoke(item);

                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (!_groups.Any(x => x.Key.Equals(key)))
                {
                    throw new KeyNotFoundException();
                }

                var groupIndex = _groups.IndexOf(_groups.First(x => x.Key.Equals(key)));
                var val = valueSelector(item);
                var valIndex = _groups.ElementAt(groupIndex).IndexOf(val);

                if (!_groups.ElementAt(groupIndex).Any(x => x.Equals(val)))
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
                    _groups[groupInfo.GroupIndex].Remove(item.Key);
                }
            }

            if (_emptyGroupsDisabled)
            {
                var keysToRemove = _groups.Where(x => x.Count == 0).Select(x => x.Key);
                groupsToRemove = RemoveGroupsWithoutNotify(keysToRemove);
            }

            var keyIndexesToRemove = groupsToRemove?
                .Select(x => Enumerable.Range(x.Index, x.Keys.Count))
                .SelectMany(x => x)
                .ToList();

            var removedItemsEvents = rangesToRemove
                .Where(x => keyIndexesToRemove == null || keyIndexesToRemove.All(y => !y.Equals(x.Key)))
                .Select(x => (
                    x.Key,
                    new NotifyGroupCollectionChangedEventArgs<TValue>(
                        NotifyCollectionChangedAction.Remove,
                        default,
                        (IReadOnlyList<(int, IReadOnlyList<TValue>)>) x.Value)))
                .ToList();

            removedItemsEvents = removedItemsEvents.Count > 0 ? removedItemsEvents : default;

            var oldItems = groupsToRemove?.Count > 0
                ? new Collection<(int, IReadOnlyList<TKey>)> { (0, groupsToRemove[0].Keys) }
                : null;

            OnChanged(
                groupsToRemove?.Count > 0 ? NotifyCollectionChangedAction.Remove : (NotifyCollectionChangedAction?) null,
                default,
                oldItems,
                removedItemsEvents);
        }

        private void OnChanged(
            NotifyCollectionChangedAction? action,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> NewItems)>? newItems,
            IReadOnlyList<(int Index, IReadOnlyList<TKey> OldItems)>? oldItems,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedEventArgs<TValue> Arg)>? groupEvents)
        {
            RaiseEvents(new NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>(
                action,
                newItems,
                oldItems,
                groupEvents));
        }

        private void RaiseEvents(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> args)
        {
            if (args.Action != null)
            {
                NotifyCollectionChangedEventArgs notifyArgs;

                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        notifyArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.NewItemRanges);
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        notifyArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, args.NewItemRanges, args.OldItemRanges);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        notifyArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, args.OldItemRanges);
                        break;

                    case NotifyCollectionChangedAction.Move:
                        notifyArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, args.OldItemRanges);
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        notifyArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                CollectionChanged?.Invoke(this, notifyArgs);
            }

            ItemsChanged?.Invoke(this, args);

            NotifyCountIfNeeded(args);
        }

        private IEnumerable<Group>? InsertGroupsWithoutNotify(
            int index,
            IEnumerable<KeyValuePair<TKey, IList<TValue>>> items,
            bool withoutEmptyGroups)
        {
            if (!items.Any())
            {
                return null;
            }

            if (items.Any(x => x.Key == null || x.Value == null))
            {
                throw new ArgumentNullException(nameof(items), "One of the keys or values is null.");
            }

            if (index > _groups.Count + items.Count() - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (_groups.Select(x => x.Key).Concat(items.Select(x => x.Key)).GroupBy(x => x).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }

            var toInsert = items
                .Where(x => !withoutEmptyGroups || x.Value.Count > 0)
                .Select(x => new Group(x))
                .ToList();

            if (toInsert.Count == 0)
            {
                return null;
            }

            foreach (var item in toInsert)
            {
                _groups.Insert(index++, item);
            }

            return toInsert;
        }

        private IEnumerable<(TKey Key, IEnumerable<(int Index, IReadOnlyList<TValue> Values)> ValuesGroups)>
            InsertItemsWithoutNotify<T>(
                IEnumerable<T> items,
                Func<T, TKey> keySelector,
                Func<T, TValue> valueSelector,
                Func<T, int>? indexSelector)
        {
            var groupedItemsToAdd = new List<(TKey Key, IEnumerable<(int Index, IReadOnlyList<TValue> Values)> Ranges)>();
            var itemsToAdd = new List<(TKey Key, IList<KeyValuePair<TValue, int>> Values)>();

            foreach (var item in items)
            {
                var key = keySelector.Invoke(item);

                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (!_groups.Any(x => x.Key.Equals(key)))
                {
                    throw new KeyNotFoundException();
                }

                var val = valueSelector.Invoke(item);
                var index = indexSelector == null ? _groups.First(x => x.Key.Equals(key)).Count : indexSelector.Invoke(item);

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
                    _groups
                        .First(x => x.Key.Equals(key))
                        .InsertRange(index, values.ToList());
                }
            }

            return groupedItemsToAdd;
        }

        private IReadOnlyList<(int Index, IReadOnlyList<TKey> Keys)> RemoveGroupsWithoutNotify(IEnumerable<TKey> keys)
        {
            if (keys.Any(key => _groups.All(item => !item.Key.Equals(key))))
            {
                throw new KeyNotFoundException();
            }

            var indexes = new List<KeyValuePair<TKey, int>>();

            for (int i = 0; i < _groups.Count; i++)
            {
                if (keys.Any(x => x.Equals(_groups[i].Key)))
                {
                    indexes.Add(new KeyValuePair<TKey, int>(_groups[i].Key, i));
                }
            }

            foreach (var key in keys.ToList())
            {
                _groups.Remove(_groups.First(x => x.Key.Equals(key)));
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

        private void NotifyCountIfNeeded(NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue> args)
        {
            var countOfGroupsChanged = IsActionCanModifyGroup(args.Action);
            if (countOfGroupsChanged)
            {
                OnPropertyChanged(nameof(Count));
            }
        }

        private bool IsActionCanModifyGroup(NotifyCollectionChangedAction? action)
        {
            return action
                is NotifyCollectionChangedAction.Add
                or NotifyCollectionChangedAction.Remove
                or NotifyCollectionChangedAction.Replace
                or NotifyCollectionChangedAction.Reset;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
