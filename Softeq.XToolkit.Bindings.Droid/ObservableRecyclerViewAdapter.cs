// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Softeq.XToolkit.Common.WeakSubscription;

namespace Softeq.XToolkit.Bindings.Droid
{
    public interface IBindableViewHolder
    {
        event EventHandler Click;

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
        void OnViewRecycled();
    }

    public class ObservableRecyclerViewAdapter<T> : RecyclerView.Adapter
    {
        private readonly Action<RecyclerView.ViewHolder, int, T> _bindViewHolderAction;
        private readonly Func<ViewGroup, int, RecyclerView.ViewHolder> _getHolderFunc;
        private IList<T> _dataSource;
        private INotifyCollectionChanged _notifier;
        private IDisposable _subscription;
        private ICommand _itemClick;

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

        public ICommand ItemClick
        {
            get => _itemClick;
            set
            {
                if (ReferenceEquals(_itemClick, value))
                {
                    return;
                }

                if (_itemClick != null && value != null)
                {
                    System.Diagnostics.Debug.WriteLine("Changing ItemClick may cause inconsistencies where some items still call the old command.");

                    _itemClick = value;
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
            var viewHolder = (IBindableViewHolder) _getHolderFunc?.Invoke(parent, viewType);

            viewHolder.SetCommand(nameof(viewHolder.Click), ItemClick);

            return (RecyclerView.ViewHolder) viewHolder;// TODO!!
        }

        #region IBindableViewHolder

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            if (holder is IBindableViewHolder bindableViewHolder)
            {
                bindableViewHolder.OnAttachedToWindow();
            }
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            if (holder is IBindableViewHolder bindableViewHolder)
            {
                bindableViewHolder.OnDetachedFromWindow();
            }

            base.OnViewDetachedFromWindow(holder);
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            if (holder is IBindableViewHolder bindableViewHolder)
            {
                bindableViewHolder.OnViewRecycled();
            }
        }

        /// <summary>
        /// By default, force recycling a view if it has animations
        /// </summary>
        public override bool OnFailedToRecycleView(Java.Lang.Object holder) => true;

        #endregion

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

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            var holder = (IBindableViewHolder) sender;

            // TODO YP: add DataContext to the IBindableViewHolder
            // set index as tag for ViewHolder
            // get model from source by tag
            var dataContext = DataSource[0];

            ExecuteCommandOnItem(ItemClick, dataContext);
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, object itemDataContext)
        {
            if (command != null && itemDataContext != null && command.CanExecute(itemDataContext))
            {
                command.Execute(itemDataContext);
            }
        }
    }
}