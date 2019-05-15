// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.EventArguments;

namespace Softeq.XToolkit.Common.Collections
{
    public class ObservableKeyGroupsCollection<TKey, TValue> :
        ObservableRangeCollection<ObservableKeyGroup<TKey, TValue>>, INotifyGroupCollectionChanged
    {
        private readonly Func<TValue, TKey> _defaultSelector;
        private readonly Comparison<TKey> _defaultKeyComparison;
        private readonly Comparison<TValue> _defaultValueComparison;

        public event EventHandler<NotifyKeyGroupsCollectionChangedEventArgs> ItemsChanged;

        public ObservableKeyGroupsCollection(Func<TValue, TKey> selector = null,
            Comparison<TKey> keyComparison = null,
            Comparison<TValue> valueComparison = null)
        {
            Keys = new ObservableRangeCollection<TKey>();
            _defaultSelector = selector;
            _defaultKeyComparison = keyComparison;
            _defaultValueComparison = valueComparison;
        }

        public ObservableRangeCollection<TKey> Keys { get; }

        public IEnumerable<TValue> Values => Items.SelectMany(x => x);

        public void AddRangeToGroups<T>(IList<T> listItem, Func<T, TValue> itemSelector,
            Func<T, TKey> keySelector = null)
        {
            var keySelectorInstance = GetSelector(itemSelector, keySelector);

            var keysToAdd = listItem.Select(keySelectorInstance).Distinct().ToArray();

            var newKeys = keysToAdd.Where(x => !Keys.Contains(x)).ToArray();
            var oldKeys = keysToAdd.Where(x => Keys.Contains(x)).ToArray();

            var list = new List<ObservableKeyGroup<TKey, TValue>>();
            foreach (var key in newKeys)
            {
                var items = listItem.Where(x => keySelectorInstance(x).Equals(key));
                var item = new ObservableKeyGroup<TKey, TValue>(key);
                item.AddRange(items.Select(itemSelector));
                list.Add(item);
            }

            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Add);
            if (newKeys.Any())
            {
                var oldSectionsCount = Count;
                AddRange(list);
                Keys.AddRange(newKeys);
                eventArgs.ModifiedSectionsIndexes.AddRange(Enumerable.Range(oldSectionsCount, list.Count));
            }

            foreach (var oldKey in oldKeys)
            {
                var items = listItem.Where(x => keySelectorInstance(x).Equals(oldKey)).Select(itemSelector).ToArray();
                var item = this.First(x => x.Key.Equals(oldKey));
                var oldSectionItemsCount = item.Count;
                item.AddRange(items);
                var oldSectionIndex = Items.IndexOf(item);
                eventArgs.ModifiedItemsIndexes.Add((oldSectionIndex,
                    Enumerable.Range(oldSectionItemsCount, items.Length).ToList()));
            }

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void AddRangeToGroups(IList<TValue> listItem, Func<TValue, TKey> selector = null)
        {
            AddRangeToGroups(listItem, x => x, selector);
        }

        public void AddGroup(ObservableKeyGroup<TKey, TValue> group)
        {
            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Add);

            Keys.Add(group.Key);
            Add(group);

            eventArgs.ModifiedSectionsIndexes.Add(Keys.IndexOf(group.Key));
            eventArgs.ModifiedItemsIndexes.Add((Keys.IndexOf(group.Key),
                Enumerable.Range(0, group.Count).ToList()));

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void AddGroups(IEnumerable<ObservableKeyGroup<TKey, TValue>> groups)
        {
            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Add);

            foreach (var group in groups)
            {
                Keys.Add(group.Key);
                Add(group);
                eventArgs.ModifiedSectionsIndexes.Add(Keys.IndexOf(group.Key));
                eventArgs.ModifiedItemsIndexes.Add((Keys.IndexOf(group.Key),
                    Enumerable.Range(0, group.Count).ToList()));
            }

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void ClearGroup(TKey key)
        {
            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Remove);
            var sectionIndex = Keys.IndexOf(key);
            eventArgs.ModifiedItemsIndexes.Add((sectionIndex, Enumerable.Range(0, Items[sectionIndex].Count).ToList()));

            this.FirstOrDefault(x => x.Key.Equals(key))?.Clear();

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void RemoveGroup(ObservableKeyGroup<TKey, TValue> group)
        {
            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Remove);
            var sectionIndex = Keys.IndexOf(group.Key);
            eventArgs.ModifiedSectionsIndexes.Add(sectionIndex);
            eventArgs.ModifiedItemsIndexes.Add((sectionIndex, Enumerable.Range(0, group.Count).ToList()));

            Remove(group);
            Keys.Remove(group.Key);

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void AddRangeToGroupsSorted<T>(IEnumerable<T> items,
            Func<T, TValue> itemSelector,
            Comparison<TValue> valueComparison = null,
            Comparison<TKey> keyComparison = null,
            Func<T, TKey> keySelector = null)
        {
            var keySelectorInstance = GetSelector(itemSelector, keySelector);
            var valueComparisonInstance = GetValueComparison(valueComparison);
            var keyComparisonInstance = GetKeyComparison(keyComparison);

            var itemsArray = items.ToArray();

            var keysToAdd = itemsArray.Select(keySelectorInstance).Distinct().ToArray();

            var newKeys = keysToAdd.Where(x => !Keys.Contains(x)).ToArray();
            var oldKeys = keysToAdd.Where(x => Keys.Contains(x)).ToArray();

            var newSections = new List<ObservableKeyGroup<TKey, TValue>>();
            foreach (var key in newKeys)
            {
                var itemsToAdd = itemsArray.Where(x => keySelectorInstance(x).Equals(key));
                var item = new ObservableKeyGroup<TKey, TValue>(key);
                item.InsertRangeSorted(itemsToAdd.Select(itemSelector), valueComparisonInstance);
                newSections.Add(item);
            }

            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Add);
            if (newKeys.Any())
            {
                var insertedSectionsIndexes =
                    InsertRangeSorted(newSections, (x, y) => keyComparisonInstance(x.Key, y.Key));
                eventArgs.ModifiedSectionsIndexes.AddRange(insertedSectionsIndexes);
                Keys.InsertRangeSorted(newKeys, keyComparisonInstance);
            }

            foreach (var oldKey in oldKeys)
            {
                var itemsToInsert = itemsArray.Where(x => keySelectorInstance(x).Equals(oldKey)).ToArray();
                var item = this.First(x => x.Key.Equals(oldKey));
                var insertedItemsIndexes =
                    item.InsertRangeSorted(itemsToInsert.Select(itemSelector), valueComparisonInstance);
                var oldSectionIndex = Items.IndexOf(item);
                eventArgs.ModifiedItemsIndexes.Add((oldSectionIndex, insertedItemsIndexes));
            }

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void AddRangeToGroupsSorted(IList<TValue> listItem,
            Comparison<TValue> valueComparison = null,
            Comparison<TKey> keyComparison = null,
            Func<TValue, TKey> keySelector = null)
        {
            if (listItem.Count == 0)
            {
                return;
            }

            AddRangeToGroupsSorted(listItem, x => x, valueComparison, keyComparison, keySelector);
        }

        public void ReplaceRangeGroup(IList<TValue> listItem, Func<TValue, TKey> selector = null)
        {
            var selectorInstance = GetSelector(selector);

            var keysToAdd = listItem.Select(selectorInstance).Distinct().ToArray();
            Keys.ReplaceRange(keysToAdd);

            var valuesToAdd = listItem
                .GroupBy(selectorInstance)
                .Select(x => new ObservableKeyGroup<TKey, TValue>(x.Key, x));

            ReplaceRange(valuesToAdd);

            ItemsChanged?.Invoke(this, CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void ClearAll()
        {
            var sectionsCount = Count;
            Keys.Clear();
            Clear();

            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Remove);
            eventArgs.ModifiedSectionsIndexes = Enumerable.Range(0, sectionsCount).ToList();
            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void RemoveFromGroups(TValue removeItem, Func<TValue, TKey> selector = null)
        {
            var selectorInstance = GetSelector(selector);

            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Remove);

            var item = this.FirstOrDefault(a => a.Key.Equals(selectorInstance(removeItem)));
            if (item == null || item.IndexOf(removeItem) < 0)
            {
                return;
            }

            var itemIndex = item.IndexOf(removeItem);
            item.Remove(removeItem);

            if (item.Count == 0)
            {
                eventArgs.ModifiedSectionsIndexes.Add(Items.IndexOf(item));
                Remove(item);
                Keys.Remove(item.Key);
            }
            else
            {
                eventArgs.ModifiedItemsIndexes.Add((Items.IndexOf(item), new List<int> {itemIndex}));
            }

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public void RemoveAllFromGroups(TValue removeItem)
        {
            RemoveAllFromGroups(new List<TValue> {removeItem});
        }

        public void RemoveAllFromGroups(IEnumerable<TValue> items)
        {
            var eventArgs = CreateItemsChangedEventArgs(NotifyCollectionChangedAction.Remove);
            for (int sectionIndex = Count - 1; sectionIndex >= 0; sectionIndex--)
            {
                var section = Items[sectionIndex];
                var indexesToRemove =
                    Enumerable.Range(0, section.Count).Where(i => items.Contains(section[i])).ToList();
                if (indexesToRemove.Count == 0)
                {
                    continue;
                }

                for (int j = indexesToRemove.Count - 1; j >= 0; j--)
                {
                    section.RemoveAt(indexesToRemove[j]);
                }

                if (section.Count == 0)
                {
                    eventArgs.ModifiedSectionsIndexes.Add(sectionIndex);
                    RemoveAt(sectionIndex);
                    Keys.Remove(section.Key);
                }
                else
                {
                    eventArgs.ModifiedItemsIndexes.Add((sectionIndex, indexesToRemove));
                }
            }

            ItemsChanged?.Invoke(this, eventArgs);
        }

        public TValue FirstOrDefaultValue(Func<TValue, bool> predicate = null)
        {
            if (predicate != null)
            {
                var section = this.FirstOrDefault(x => x.Any(predicate));
                return section != null ? section.FirstOrDefault(predicate) : default(TValue);
            }

            return Count > 0 ? this.First(x => x.Count > 0).FirstOrDefault() : default(TValue);
        }

        private Func<TValue, TKey> GetSelector(Func<TValue, TKey> selector = null)
        {
            if (selector != null)
            {
                return selector;
            }

            if (_defaultSelector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return _defaultSelector;
        }

        private Func<T, TKey> GetSelector<T>(Func<T, TValue> valueSelector, Func<T, TKey> keySelector = null)
        {
            if (keySelector != null)
            {
                return keySelector;
            }

            if (_defaultSelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return arg => _defaultSelector.Invoke(valueSelector.Invoke(arg));
        }

        private Comparison<TKey> GetKeyComparison(Comparison<TKey> comparison)
        {
            if (comparison == null && _defaultKeyComparison == null)
            {
                throw new ArgumentNullException(nameof(_defaultKeyComparison), "No key comparison found");
            }

            return comparison ?? _defaultKeyComparison;
        }

        private Comparison<TValue> GetValueComparison(Comparison<TValue> comparison)
        {
            if (comparison == null && _defaultValueComparison == null)
            {
                throw new ArgumentNullException(nameof(_defaultValueComparison), "No value comparison found");
            }

            return comparison ?? _defaultValueComparison;
        }

        private NotifyKeyGroupsCollectionChangedEventArgs CreateItemsChangedEventArgs(
            NotifyCollectionChangedAction action)
        {
            return new NotifyKeyGroupsCollectionChangedEventArgs(action, Items.Select(x => x.Count).ToList());
        }
    }
}