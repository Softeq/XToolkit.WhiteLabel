// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IObservableKeyGroupsCollection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>
    {
        void AddGroups(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items);

        void InsertGroups(int index, IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items);

        void ReplaceGroups(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items);

        void RemoveGroups(IEnumerable<TKey> items);

        void ClearGroups();

        void ClearGroup(TKey key);

        void AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);

        void ReplaceAllItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector);
    }
}
