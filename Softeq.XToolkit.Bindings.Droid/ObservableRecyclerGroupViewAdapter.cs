// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.EventArguments;
using Softeq.XToolkit.Common.WeakSubscription;

namespace Softeq.XToolkit.Bindings.Droid
{
    public class ObservableRecyclerGroupViewAdapter<TKey, TItem> : RecyclerView.Adapter
    {
        private List<AdapterItem<TKey, TItem>> _plainItems;
        private ObservableKeyGroupsCollection<TKey, TItem> _items;
        private Func<ViewGroup, int, RecyclerView.ViewHolder> _getHolderFunc;
        private Func<ViewGroup, int, RecyclerView.ViewHolder> _getHeaderHolderFunc;
        private Action<RecyclerView.ViewHolder, int, TItem, bool> _bindViewHolderAction;
        private Action<RecyclerView.ViewHolder, int, TKey> _bindHeaderViewHolderAction;
        private IDisposable _subscription;

        public event EventHandler LastItemRequested;

        public ObservableRecyclerGroupViewAdapter(
            ObservableKeyGroupsCollection<TKey, TItem> items,
            Func<ViewGroup, int, RecyclerView.ViewHolder> getHolderFunc,
            Func<ViewGroup, int, RecyclerView.ViewHolder> getHeaderHolderFunc,
            Action<RecyclerView.ViewHolder, int, TItem, bool> bindViewHolderAction,
            Action<RecyclerView.ViewHolder, int, TKey> bindHeaderViewHolderAction)
        {
            _plainItems = new List<AdapterItem<TKey, TItem>>();
            _items = items;
            _getHolderFunc = getHolderFunc;
            _getHeaderHolderFunc = getHeaderHolderFunc;
            _bindViewHolderAction = bindViewHolderAction;
            _bindHeaderViewHolderAction = bindHeaderViewHolderAction;
            _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(_items, NotifierCollectionChanged);
            BuildPlainList();
        }

        public override int ItemCount => _plainItems.Count;

        public int GetHeaderPosition(TKey key, Func<TKey, TKey, bool> predicate)
        {
            var headerItem = _plainItems.FirstOrDefault(x => x.IsHeader && predicate(x.Key, key));
            return _plainItems.IndexOf(headerItem);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _plainItems[position];
            if (item.IsHeader)
            {
                _bindHeaderViewHolderAction(holder, position, item.Key);
            }
            else
            {
                var isLast = true;
                var nextPosition = position + 1;
                if (nextPosition > 0 && nextPosition < _plainItems.Count)
                {
                    var nextItem = _plainItems[nextPosition];
                    isLast = nextItem.IsHeader;
                }

                _bindViewHolderAction(holder, position, item.Item, isLast);
            }

            if (position == _plainItems.Count - 1)
            {
                LastItemRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return viewType == 0
                ? _getHeaderHolderFunc(parent, viewType)
                : _getHolderFunc(parent, viewType);
        }

        public override int GetItemViewType(int position)
        {
            return _plainItems[position].IsHeader ? 0 : 1;
        }

        protected void NotifierCollectionChanged(object sender, NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            BuildPlainList();
            NotifyOnMainThread();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription?.Dispose();
                _plainItems = null;
                _items = null;
                _subscription = null;
                _getHolderFunc = null;
                _getHeaderHolderFunc = null;
                _bindViewHolderAction = null;
                _bindHeaderViewHolderAction = null;
            }

            base.Dispose(disposing);
        }

        private void BuildPlainList()
        {
            _plainItems.Clear();
            foreach (var item in _items)
            {
                _plainItems.Add(AdapterItem<TKey, TItem>.CreateFromKey(item.Key));
                foreach (var child in item)
                {
                    _plainItems.Add(AdapterItem<TKey, TItem>.CreateFromValue(child));
                }
            }
        }

        private void NotifyOnMainThread()
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                NotifyDataSetChanged();
            }
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(NotifyDataSetChanged);
            }
        }

        private class AdapterItem<TK, TI>
        {
            public static AdapterItem<TK, TI> CreateFromKey(TK key)
            {
                return new AdapterItem<TK, TI>
                {
                    IsHeader = true,
                    Key = key
                };
            }

            public static AdapterItem<TK, TI> CreateFromValue(TI item)
            {
                return new AdapterItem<TK, TI>
                {
                    Item = item
                };
            }

            public TK Key { get; private set; }

            public TI Item { get; private set; }

            public bool IsHeader { get; private set; }
        }
    }
}