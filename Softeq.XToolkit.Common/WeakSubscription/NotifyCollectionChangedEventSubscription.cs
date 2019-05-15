// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.EventArguments;

namespace Softeq.XToolkit.Common.WeakSubscription
{
    public class NotifyCollectionChangedEventSubscription : WeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        public NotifyCollectionChangedEventSubscription(
            INotifyCollectionChanged source,
            EventHandler<NotifyCollectionChangedEventArgs> targetEventHandler)
            : base(source, "CollectionChanged", targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new NotifyCollectionChangedEventHandler(OnSourceEvent);
        }
    }
    
    public class NotifyCollectionKeyGroupChangedEventSubscription : WeakEventSubscription<INotifyGroupCollectionChanged, NotifyKeyGroupsCollectionChangedEventArgs>
    {
        public NotifyCollectionKeyGroupChangedEventSubscription(
            INotifyGroupCollectionChanged source,
            EventHandler<NotifyKeyGroupsCollectionChangedEventArgs> targetEventHandler)
            : base(source, "ItemsChanged", targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<NotifyKeyGroupsCollectionChangedEventArgs>(OnSourceEvent);
        }
    }
}
