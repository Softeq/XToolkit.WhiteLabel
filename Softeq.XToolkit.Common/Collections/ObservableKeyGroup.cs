// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     Defines a key/value pair that can be set or retrieved when value is ObservableRangeCollection.
    /// </summary>
    /// <typeparam name="TKey">The element type of the key</typeparam>
    /// <typeparam name="TValue">The element type of the collection item</typeparam>
    public class ObservableKeyGroup<TKey, TValue> : ObservableRangeCollection<TValue>,
        IGrouping<TKey, TValue>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Softeq.XToolkit.Common.Collections.ObservableKeyGroup`2" /> class with the
        ///     specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public ObservableKeyGroup(TKey key)
        {
            Key = key;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Softeq.XToolkit.Common.Collections.ObservableKeyGroup`2" /> class with the
        ///     specified key and values.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="collection">Collection.</param>
        public ObservableKeyGroup(TKey key, IEnumerable<TValue> collection) : base(collection)
        {
            Key = key;
        }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public TKey Key { get; set; }
    }
}
