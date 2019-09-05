// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IObservableKeyGroupsCollection<TKey, TValue>
        : IEnumerable<IGrouping<TKey, TValue>>
    {
        /// <summary>
        /// Add groups with specified keys and empty items.
        /// </summary>
        /// <param name="keys">List of group keys to add</param>
        void AddGroups(IEnumerable<TKey> keys);

        /// <summary>
        /// Add groups with specified keys and items.
        /// </summary>
        /// <param name="items">List of group keys and items to add</param>
        void AddGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        /// <summary>
        /// Insert groups with specified keys and empty items at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="keys">Group keys to insert</param>
        void InsertGroups(int index, IEnumerable<TKey> keys);

        /// <summary>
        /// Insert groups with specified keys and items at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="items">List of group keys and items to add</param>
        void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        /// <summary>
        ///  Clear collection and add groups with specified keys and empty items.
        /// </summary>
        /// <param name="keys">List of group keys to add</param>
        void ReplaceGroups(IEnumerable<TKey> keys);

        /// <summary>
        ///  Clear collection and add groups with specified keys and items.
        /// </summary>
        /// <param name="items">List of group keys and items to add</param>
        void ReplaceGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        /// <summary>
        /// Remove groups from collection
        /// </summary>
        /// <param name="keys">Group keys to remove</param>
        void RemoveGroups(IEnumerable<TKey> items);

        /// <summary>
        /// Clear collection
        /// </summary>
        void ClearGroups();

        /// <summary>
        /// Clear group with specified key.
        /// </summary>
        /// <param name="key">Group keys</param>
        void ClearGroup(TKey key);

        /// <summary>
        /// Add items to collectios at specified group, add group when group key is not exists
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="items">List of objects</param>
        /// <param name="keySelector">Function returning a key from the object</param>
        /// <param name="valueSelector">>Function returning an item from the object</param>
        void AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);

        /// <summary>
        /// Insert items to collectios at specified group
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="items">List of objects</param>
        /// <param name="keySelector">Function returning a key from the object</param>
        /// <param name="valueSelector">>Function returning an item from the object</param>
        /// <param name="valueIndexSelector">Function returning an index at which item should be inserted.</param>
        void InsertItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector);

        /// <summary>
        /// Clear collection and add items to collectios at specified group, add group when group key is not exists
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="items">List of objects</param>
        /// <param name="keySelector">Function returning a key from the object</param>
        /// <param name="valueSelector">>Function returning an item from the object</param>
        void ReplaceItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);

        /// <summary>
        /// Remove items to collectios frmom specified group
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="items">List of objects</param>
        /// <param name="keySelector">Function returning a key from the object</param>
        /// <param name="valueSelector">>Function returning an item from the object</param>
        void RemoveItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);
    }
}
