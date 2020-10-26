// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Collections.EventArgs;

namespace Softeq.XToolkit.Common.Weak
{
    public class NotifyCollectionChangedEventSubscription
        : WeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
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

    //public class NotifyCollectionKeyGroupChangedEventSubscription
    //    : WeakEventSubscription<INotifyGroupCollectionChanged, NotifyKeyGroupsCollectionChangedEventArgs>
    //{
    //    public NotifyCollectionKeyGroupChangedEventSubscription(
    //        INotifyGroupCollectionChanged source,
    //        EventHandler<NotifyKeyGroupsCollectionChangedEventArgs> targetEventHandler)
    //        : base(source, "ItemsChanged", targetEventHandler)
    //    {
    //    }

    //    protected override Delegate CreateEventHandler()
    //    {
    //        return new EventHandler<NotifyKeyGroupsCollectionChangedEventArgs>(OnSourceEvent);
    //    }
    //}

    public class NotifyCollectionKeyGroupNewChangedEventSubscription<TKey, TItem>
        : WeakEventSubscription<INotifyKeyGroupCollectionChanged<TKey, TItem>,
            NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem>>
    {
        public NotifyCollectionKeyGroupNewChangedEventSubscription(
            INotifyKeyGroupCollectionChanged<TKey, TItem> source,
            EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem>> targetEventHandler)
            : base(source, "ItemsChanged", targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem>>(OnSourceEvent);
        }
    }
}
