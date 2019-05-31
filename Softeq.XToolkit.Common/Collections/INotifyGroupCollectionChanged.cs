// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.EventArguments;

namespace Softeq.XToolkit.Common.Collections
{
    public interface INotifyGroupCollectionChanged : INotifyCollectionChanged
    {
        event EventHandler<NotifyKeyGroupsCollectionChangedEventArgs> ItemsChanged;
    }
}