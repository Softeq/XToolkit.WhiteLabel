// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     <see cref="ObservableRangeCollection{TValue}"/> with the additional Key property.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the collection items.</typeparam>
    public class ObservableKeyGroup<TKey, TValue> : ObservableRangeCollection<TValue>,
        IGrouping<TKey, TValue>
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="ObservableKeyGroup{TKey, TValue}"/> class with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public ObservableKeyGroup(TKey key)
        {
            Key = key;
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="ObservableKeyGroup{TKey, TValue}"/> class with the specified key and collection.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="collection">Collection.</param>
        public ObservableKeyGroup(TKey key, IEnumerable<TValue> collection)
            : base(collection)
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
