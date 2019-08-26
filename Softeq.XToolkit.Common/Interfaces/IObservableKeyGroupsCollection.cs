// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IObservableKeyGroupsCollection<TKey, TValue>
        : IEnumerable<KeyValuePair<TKey, IList<TValue>>>
    {
        void AddGroups(IEnumerable<TKey> keys);

        void AddGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        void InsertGroups(int index, IEnumerable<TKey> keys);

        void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, IList<TValue>>> items);

        void RemoveGroups(IEnumerable<TKey> items);

        void ClearGroups();

        void ClearGroup(TKey key);

        void AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);

        void InsertItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector);

        void RemoveItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);
    }
}
