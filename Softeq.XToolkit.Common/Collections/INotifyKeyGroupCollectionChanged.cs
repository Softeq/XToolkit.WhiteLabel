// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Collections.EventArgs;

namespace Softeq.XToolkit.Common.Collections
{
    public interface INotifyKeyGroupCollectionChanged<TKey, TValue>
    {
        event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;
    }
}
