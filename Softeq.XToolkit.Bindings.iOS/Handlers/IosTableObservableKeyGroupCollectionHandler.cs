// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.Handlers;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Weak;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Handlers
{
    internal sealed class IosTableObservableKeyGroupCollectionHandler<TKey, TItem>
        : ObservableKeyGroupCollectionHandlerBase<TKey, TItem>
    {
        private readonly WeakReferenceEx<UITableView> _tableViewRef;

        internal IosTableObservableKeyGroupCollectionHandler(UITableView tableView)
        {
            _tableViewRef = WeakReferenceEx.Create(tableView);
        }

        protected override void HandleGroupsAdd(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            foreach (var sectionsRange in args.NewItemRanges)
            {
                int sectionIndex = sectionsRange.Index;

                foreach (var section in sectionsRange.NewItems)
                {
                    _tableViewRef.Target?.InsertSections(NSIndexSet.FromIndex(sectionIndex), UITableViewRowAnimation.Automatic);

                    sectionIndex++;
                }
            }
        }

        protected override void HandleGroupsRemove(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            foreach (var sectionsRange in args.OldItemRanges)
            {
                int sectionIndex = sectionsRange.Index;

                foreach (var section in sectionsRange.OldItems)
                {
                    _tableViewRef.Target?.DeleteSections(NSIndexSet.FromIndex(sectionIndex), UITableViewRowAnimation.Automatic);

                    sectionIndex++;
                }
            }
        }

        protected override void HandleGroupsReplace(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            HandleGroupsAdd(args);
            HandleGroupsRemove(args);
        }

        protected override void HandleGroupsReset()
        {
            _tableViewRef.Target?.ReloadData();
        }

        protected override void HandleItemsAdd(int sectionIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.NewItemRanges)
            {
                var indexPaths = Enumerable.Range(range.Index, range.NewItems.Count)
                    .Select(x => NSIndexPath.FromRowSection(x, sectionIndex))
                    .ToArray();
                _tableViewRef.Target?.InsertRows(indexPaths, UITableViewRowAnimation.Automatic);
            }
        }

        protected override void HandleItemsRemove(int sectionIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.OldItemRanges)
            {
                var indexPaths = Enumerable.Range(range.Index, range.OldItems.Count)
                    .Select(x => NSIndexPath.FromRowSection(x, sectionIndex))
                    .ToArray();
                _tableViewRef.Target?.DeleteRows(indexPaths, UITableViewRowAnimation.Automatic);
            }
        }

        protected override void HandleItemsReset(int sectionIndex)
        {
            _tableViewRef.Target?.ReloadSections(NSIndexSet.FromIndex(sectionIndex), UITableViewRowAnimation.Automatic);
        }

        protected override Action<Action> BatchAction => Batch;

        private void Batch(Action action)
        {
            _tableViewRef.Target?.BeginUpdates();

            action.Invoke();

            _tableViewRef.Target?.EndUpdates();
        }
    }
}
