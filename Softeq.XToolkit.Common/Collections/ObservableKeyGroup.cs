// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Collections
{
    public class ObservableKeyGroup<TKey, TValue> : ObservableRangeCollection<TValue>
    {
        public ObservableKeyGroup(TKey key)
        {
            Key = key;
        }

        public ObservableKeyGroup(TKey key, IEnumerable<TValue> collection) : base(collection)
        {
            Key = key;
        }

        public TKey Key { get; set; }
    }
}