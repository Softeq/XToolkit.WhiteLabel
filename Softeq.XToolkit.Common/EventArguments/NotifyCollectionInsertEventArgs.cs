using System.Collections.Generic;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.EventArguments
{
    public class NotifyCollectionInsertEventArgs : NotifyCollectionChangedEventArgs
    {
        public NotifyCollectionInsertEventArgs(List<int> insertedItemsIndexes) : base(NotifyCollectionChangedAction.Add, new List<object>(), -1)
        {
            InsertedItemsIndexes = insertedItemsIndexes;
        }

        public List<int> InsertedItemsIndexes { get; } = new List<int>();
    }
}
