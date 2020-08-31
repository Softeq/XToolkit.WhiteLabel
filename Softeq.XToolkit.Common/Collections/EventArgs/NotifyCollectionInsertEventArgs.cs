// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.Collections.EventArgs
{
    public class NotifyCollectionInsertEventArgs : NotifyCollectionChangedEventArgs
    {
        public NotifyCollectionInsertEventArgs(List<int> insertedItemsIndexes)
            : base(NotifyCollectionChangedAction.Add, new List<object>(), -1)
        {
            InsertedItemsIndexes = insertedItemsIndexes ?? new List<int>();
        }

        public List<int> InsertedItemsIndexes { get; }
    }
}
