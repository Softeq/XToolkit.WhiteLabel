// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     Notifies listeners of dynamic changes, such as when a list of items is added and removed or the whole list is cleared.
    /// </summary>
    public interface INotifyGroupCollectionChanged : INotifyCollectionChanged
    {
        /// <summary>
        ///     Occurs when the collection changes.
        /// </summary>
        event EventHandler<NotifyKeyGroupsCollectionChangedEventArgs> ItemsChanged;
    }
}
