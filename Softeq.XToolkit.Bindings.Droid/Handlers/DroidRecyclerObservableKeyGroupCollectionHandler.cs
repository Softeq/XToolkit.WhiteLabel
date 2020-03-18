// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.Bindings.Handlers;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Weak;

#nullable disable

namespace Softeq.XToolkit.Bindings.Droid.Handlers
{
    internal class DroidRecyclerDataSourceHandler
    {
        public static void Handle<TKey, TItem>(RecyclerView.Adapter adapter,
            IEnumerable<IGrouping<TKey, TItem>> dataSource,
            IList<FlatItem> flatMapping,
            bool withSectionHeader,
            bool withSectionFooter,
            NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            var handler = new DroidRecyclerObservableKeyGroupCollectionHandler<TKey, TItem>(
                adapter,
                dataSource,
                flatMapping,
                withSectionHeader,
                withSectionFooter);

            handler.Handle(args);
        }
    }

    internal sealed class DroidRecyclerObservableKeyGroupCollectionHandler<TKey, TItem>
        : ObservableKeyGroupCollectionHandlerBase<TKey, TItem>
    {
        private readonly WeakReferenceEx<RecyclerView.Adapter> _recyclerViewAdapterRef;
        private readonly IEnumerable<IGrouping<TKey, TItem>> _dataSource;
        private readonly IList<FlatItem> _flatMapping;
        private readonly bool _withSectionHeader;
        private readonly bool _withSectionFooter;

        internal DroidRecyclerObservableKeyGroupCollectionHandler(RecyclerView.Adapter recyclerView,
            IEnumerable<IGrouping<TKey, TItem>> dataSource,
            IList<FlatItem> flatMapping,
            bool withSectionHeader,
            bool withSectionFooter)
        {
            _recyclerViewAdapterRef = WeakReferenceEx.Create(recyclerView);
            _dataSource = dataSource;
            _flatMapping = flatMapping;
            _withSectionHeader = withSectionHeader;
            _withSectionFooter = withSectionFooter;
        }

        protected override void HandleGroupsAdd(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            foreach (var range in args.NewItemRanges)
            {
                Enumerable.Range(range.Index, range.NewItems.Count())
                    .Apply(InsertSection);
            }
        }

        protected override void HandleGroupsRemove(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            foreach (var range in args.OldItemRanges)
            {
                Enumerable.Range(range.Index, range.OldItems.Count())
                    .Apply(RemoveSection);
            }
        }

        protected override void HandleGroupsReplace(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            HandleGroupsAdd(args);
            HandleGroupsRemove(args);
        }

        protected override void HandleGroupsReset()
        {
            _recyclerViewAdapterRef.Target?.NotifyDataSetChanged();
        }

        protected override void HandleItemsAdd(int sectionIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.NewItemRanges)
            {
                InsertItems(sectionIndex, range.Index, range.NewItems.Count());
            }
        }

        protected override void HandleItemsRemove(int sectionIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.OldItemRanges)
            {
                RemoveItems(sectionIndex, range.Index, range.OldItems.Count());
            }
        }

        protected override void HandleItemsReset(int sectionIndex)
        {
            RemoveItems(sectionIndex, 0,
                _flatMapping.Count(x => x.SectionIndex == sectionIndex && x.Type == ItemType.Item));
        }

        private void InsertSection(int sectionIndex)
        {
            int positionStart = default;
            int count = default;

            var flat = _flatMapping.LastOrDefault(x => x.SectionIndex == sectionIndex - 1)
                ?? _flatMapping?.FirstOrDefault(x => x.Type == ItemType.Header);

            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat) + 1;
            }

            if (_withSectionHeader)
            {
                count += 1;
            }

            count += _dataSource.ElementAt(sectionIndex).Count();

            if (_withSectionFooter)
            {
                count += 1;
            }

            _recyclerViewAdapterRef.Target?.NotifyItemRangeInserted(positionStart, count);
        }

        private void RemoveSection(int sectionIndex)
        {
            int positionStart;

            var flat = _flatMapping.FirstOrDefault(x => x.SectionIndex == sectionIndex);

            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat);
            }

            var count = _flatMapping.Count(x => x.SectionIndex == sectionIndex);

            _recyclerViewAdapterRef.Target?.NotifyItemRangeRemoved(positionStart, count);
        }

        private void InsertItems(int sectionIndex, int startIndex, int count)
        {
            int positionStart;

            var flat = _flatMapping?.FirstOrDefault(x => x.SectionIndex == sectionIndex && x.ItemIndex == startIndex - 1)
                ?? _flatMapping?.FirstOrDefault(x => x.SectionIndex == sectionIndex && x.Type == ItemType.SectionHeader)
                ?? _flatMapping?.LastOrDefault(x => x.SectionIndex == sectionIndex - 1)
                ?? _flatMapping?.FirstOrDefault(x => x.Type == ItemType.Header);

            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat) + 1;
            }

            _recyclerViewAdapterRef.Target?.NotifyItemRangeInserted(positionStart, count);
        }

        private void RemoveItems(int sectionIndex, int startIndex, int count)
        {
            int positionStart;

            var flat = _flatMapping?.FirstOrDefault(x => x.SectionIndex == sectionIndex && x.ItemIndex == startIndex);
            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat);
            }

            _recyclerViewAdapterRef.Target?.NotifyItemRangeRemoved(positionStart, count);
        }
    }
}
