// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Softeq.XToolkit.Common.WeakSubscription;

namespace Softeq.XToolkit.Bindings.Droid
{
    public class ObservableRecyclerViewAdapter<T> : RecyclerView.Adapter
    {
        private readonly Action<RecyclerView.ViewHolder, int, T> _bindViewHolderAction;
        private readonly Func<ViewGroup, int, RecyclerView.ViewHolder> _getHolderFunc;
        private IList<T> _dataSource;
        private INotifyCollectionChanged _notifier;
        private IDisposable _subscription;

        public event EventHandler LastItemRequested;
        public event EventHandler DataReloaded;

        public ObservableRecyclerViewAdapter(
            IList<T> items,
            Func<ViewGroup, int, RecyclerView.ViewHolder> getHolderFunc,
            Action<RecyclerView.ViewHolder, int, T> bindViewHolderAction)
        {
            DataSource = items;
            _getHolderFunc = getHolderFunc;
            _bindViewHolderAction = bindViewHolderAction;
        }

        public IList<T> DataSource
        {
            get => _dataSource;
            private set
            {
                if (Equals(_dataSource, value))
                {
                    return;
                }

                _dataSource = value;
                _notifier = _dataSource as INotifyCollectionChanged;

                if (_notifier != null)
                {
                    _subscription = new NotifyCollectionChangedEventSubscription(_notifier, NotifierCollectionChanged);
                }
            }
        }

        public bool ShouldNotifyByAction { get; set; }

        public override int ItemCount => _dataSource.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (_bindViewHolderAction == null)
            {
                return;
            }

            var item = DataSource[position];
            _bindViewHolderAction.Invoke(holder, position, item);

            if (position > 0 && position == DataSource.Count - 1)
            {
                LastItemRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _getHolderFunc?.Invoke(parent, viewType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected virtual void NotifierCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedOnMainThread(e);
        }

        private void NotifyCollectionChangedOnMainThread(NotifyCollectionChangedEventArgs e)
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                NotifyAdapterSource(e);
            }
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() => NotifyAdapterSource(e));
            }
        }

        private void NotifyAdapterSource(NotifyCollectionChangedEventArgs e)
        {
            if (ShouldNotifyByAction)
            {
                NotifyCollectionChangedByAction(e);
            }
            else
            {
                NotifyDataSetChanged();
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                DataReloaded?.Invoke(this, EventArgs.Empty);
            }
        }

        private void NotifyCollectionChangedByAction(NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        for (int i = 0; i < e.NewItems.Count; i++)
                        {
                            NotifyItemMoved(e.OldStartingIndex + i, e.NewStartingIndex + i);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        NotifyDataSetChanged();
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.Warn(nameof(ObservableRecyclerViewAdapter<T>),
                        "Exception masked during Adapter RealNotifyDataSetChanged {0}. Are you trying to update your collection from a background task? See http://goo.gl/0nW0L6",
                        exception.ToString());
            }
        }
    }
}