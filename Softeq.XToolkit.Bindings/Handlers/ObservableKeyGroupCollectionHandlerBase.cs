// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections.EventArgs;

#nullable disable

namespace Softeq.XToolkit.Bindings.Handlers
{
    public abstract class ObservableKeyGroupCollectionHandlerBase<TKey, TItem>
    {
        public void Handle(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> arg)
        {
            switch (arg.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    HandleGroupsReset();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    HandleGroupsReplace(arg);
                    break;
                default:
                    if (BatchAction == null)
                    {
                        Update(arg);
                    }
                    else
                    {
                        BatchAction.Invoke(() => Update(arg));
                    }

                    break;
            }
        }

        protected virtual Action<Action> BatchAction { get; }

        protected abstract void HandleGroupsAdd(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args);

        protected abstract void HandleGroupsRemove(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args);

        protected abstract void HandleGroupsReplace(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args);

        protected abstract void HandleGroupsReset();

        protected abstract void HandleItemsAdd(int sectionIndex, NotifyGroupCollectionChangedEventArgs<TItem> args);

        protected abstract void HandleItemsRemove(int sectionIndex, NotifyGroupCollectionChangedEventArgs<TItem> args);

        protected abstract void HandleItemsReset(int sectionIndex);

        private void Update(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    HandleGroupsAdd(e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    HandleGroupsRemove(e);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    HandleGroupsReplace(e);
                    break;
            }

            if (e.GroupEvents != null)
            {
                foreach (var groupEvent in e.GroupEvents)
                {
                    switch (groupEvent.Arg.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            HandleItemsAdd(groupEvent.GroupIndex, groupEvent.Arg);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            HandleItemsRemove(groupEvent.GroupIndex, groupEvent.Arg);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            HandleItemsReset(groupEvent.GroupIndex);
                            break;
                    }
                }
            }
        }
    }
}
