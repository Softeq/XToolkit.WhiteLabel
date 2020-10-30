// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.Common.Collections
{
    public interface IObservableKeyGroupsCollection<TKey, TValue>
        : IEnumerable<IGrouping<TKey, TValue>>
        where TKey : notnull
        where TValue : notnull
    {
        /// <summary>
        ///     Add groups with the specified keys and empty items at the end of the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <param name="keys">List of group keys to add.</param>
        void AddGroups(IEnumerable<TKey> keys);

        /// <summary>
        ///     Add groups with the specified keys and items at the end of the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <param name="items">List of group keys and items to add.</param>
        void AddGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        /// <summary>
        ///     Insert groups with the specified keys and empty items at the specified index of the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which groups should be inserted.</param>
        /// <param name="keys">Group keys to insert.</param>
        void InsertGroups(int index, IEnumerable<TKey> keys);

        /// <summary>
        ///     Insert groups with the specified keys and items at the specified index of the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which items should be inserted.</param>
        /// <param name="items">List of group keys and items to add.</param>
        void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        /// <summary>
        ///     Clear the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> and add groups with the specified keys and empty items.
        /// </summary>
        /// <param name="keys">List of group keys to add.</param>
        void ReplaceAllGroups(IEnumerable<TKey> keys);

        /// <summary>
        ///     Clear the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> and add groups with the specified keys and items.
        /// </summary>
        /// <param name="items">List of group keys and items to add.</param>
        void ReplaceAllGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        /// <summary>
        ///     Remove the specified groups from the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <param name="items">Group keys to remove.</param>
        void RemoveGroups(IEnumerable<TKey> items);

        /// <summary>
        ///     Clear current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Clear group with the specified key.
        /// </summary>
        /// <param name="key">Group key.</param>
        void ClearGroup(TKey key);

        /// <summary>
        ///     Add items to the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="items">List of items to add.</param>
        /// <param name="keySelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Key.</param>
        /// <param name="valueSelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Value.</param>
        void AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);

        /// <summary>
        ///     Insert items to the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="items">List of items to insert.</param>
        /// <param name="keySelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Key.</param>
        /// <param name="valueSelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Value.</param>
        /// <param name="valueIndexSelector">Function that converts input list item to the index inside <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> group.</param>
        void InsertItems<T>(
            IEnumerable<T> items,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector,
            Func<T, int> valueIndexSelector);

        /// <summary>
        ///     Clear current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> and add the items from the specified collection.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="items">List of items to add.</param>
        /// <param name="keySelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Key.</param>
        /// <param name="valueSelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Value.</param>
        void ReplaceAllItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);

        /// <summary>
        ///     Remove the specified items from the current <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="items">List of items to remove.</param>
        /// <param name="keySelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Key.</param>
        /// <param name="valueSelector">Function that converts input list item to the <see cref="IObservableKeyGroupsCollection{TKey, TValue}"/> Value.</param>
        void RemoveItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);
    }
}
