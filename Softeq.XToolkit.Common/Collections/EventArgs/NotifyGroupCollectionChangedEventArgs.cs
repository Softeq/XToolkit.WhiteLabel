// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.Collections.EventArgs
{
    public class NotifyGroupCollectionChangedEventArgs<T> : System.EventArgs
    {
        public NotifyGroupCollectionChangedEventArgs(
            NotifyCollectionChangedAction? action,
            IReadOnlyList<(int Index, IReadOnlyList<T> NewItems)>? newItems,
            IReadOnlyList<(int Index, IReadOnlyList<T> OldItems)>? oldItems)
            : base()
        {
            Action = action;
            NewItemRanges = newItems;
            OldItemRanges = oldItems;
        }

        public NotifyCollectionChangedAction? Action { get; protected set; }

        public IReadOnlyList<(int Index, IReadOnlyList<T> NewItems)>? NewItemRanges { get; protected set; }

        public IReadOnlyList<(int Index, IReadOnlyList<T> OldItems)>? OldItemRanges { get; protected set; }
    }
}
