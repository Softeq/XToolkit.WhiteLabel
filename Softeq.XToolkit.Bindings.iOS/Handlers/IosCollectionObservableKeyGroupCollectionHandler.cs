// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.Handlers;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Softeq.XToolkit.Common.Weak;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS.Handlers
{
    internal sealed class IosCollectionObservableKeyGroupCollectionHandler<TKey, TItem>
        : ObservableKeyGroupCollectionHandlerBase<TKey, TItem>
    {
        private readonly WeakReferenceEx<UICollectionView> _collectionViewRef;

        internal IosCollectionObservableKeyGroupCollectionHandler(UICollectionView collectionView)
        {
            _collectionViewRef = WeakReferenceEx.Create(collectionView);
        }

        protected override void HandleGroupsAdd(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            foreach (var sectionsRange in args.NewItemRanges)
            {
                int sectionIndex = sectionsRange.Index;

                foreach (var section in sectionsRange.NewItems)
                {
                    _collectionViewRef.Target?.InsertSections(NSIndexSet.FromIndex(sectionIndex));

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
                    _collectionViewRef.Target?.DeleteSections(NSIndexSet.FromIndex(sectionIndex));

                    sectionIndex++;
                }
            }
        }

        protected override void HandleGroupsReplace(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            _collectionViewRef.Target?.ReloadData();
        }

        protected override void HandleGroupsReset()
        {
            _collectionViewRef.Target?.ReloadData();
        }

        protected override void HandleItemsAdd(int sectionIndex, NotifyGroupCollectionChangedEventArgs<TItem> args)
        {
            foreach (var range in args.NewItemRanges)
            {
                var indexPaths = Enumerable.Range(range.Index, range.NewItems.Count)
                    .Select(x => NSIndexPath.FromRowSection(x, sectionIndex))
                    .ToArray();
                _collectionViewRef.Target?.InsertItems(indexPaths);
            }
        }

        protected override void HandleItemsRemove(int sectionIndex, NotifyGroupCollectionChangedEventArgs<TItem> args)
        {
            foreach (var range in args.OldItemRanges)
            {
                var indexPaths = Enumerable.Range(range.Index, range.OldItems.Count)
                    .Select(x => NSIndexPath.FromRowSection(x, sectionIndex))
                    .ToArray();
                _collectionViewRef.Target?.DeleteItems(indexPaths);
            }
        }

        protected override void HandleItemsReset(int sectionIndex)
        {
            _collectionViewRef.Target?.ReloadSections(NSIndexSet.FromIndex(sectionIndex));
        }

        protected override Action<Action> BatchAction => Batch;

        private void Batch(Action action)
        {
            _collectionViewRef.Target?.PerformBatchUpdates(action, null);
        }
    }
}
