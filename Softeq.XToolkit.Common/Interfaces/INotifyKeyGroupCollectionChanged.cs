// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.EventArguments;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface INotifyKeyGroupCollectionChanged<TKey, TValue>
    {
        event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;
    }
}
